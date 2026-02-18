using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

public class TagBombHazardColliderSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private static readonly Vector2Int[] s_crossOffsets =
    {
          Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
    };

    private static readonly Vector2Int[] s_diagonalOffsets =
    {
        new(1, 1), new(1, -1), new(-1, -1), new(-1, 1)
    };

    private EcsEvent _ecsEvent =
        new EcsEvent<DicePresenter>()
            .SubscribeWhere<IsTriggerColliderEnter>(c => c.entityHit.Has<TagBombHazard>(), added: OnDiceCollider);

    private static void OnDiceCollider(EcsEntity entity)
    {
        var collision = entity.Get<IsTriggerColliderEnter>();
        var damage = collision.entityHit.Get<DamageConstComponent>().damage;
        collision.entityHit.AddFrameToEvent<IsSoundPlay>();

        collision.entityHit.Destroy();

        var gridComponent = entity.Get<GridComponent>();
        ref var mainRoll = ref entity.Get<RollComponent>();
        mainRoll.value -= damage;

        if (mainRoll.value > 0)
        {
            UpdateVisual(entity, mainRoll.value);
            return;
        }

        var overflowDamage = Mathf.Abs(mainRoll.value);
        entity.AddFrameToEvent<IsDestroy>(new());

        if (overflowDamage <= 0) return;

        ProcessExplosion(gridComponent, overflowDamage);
    }

    private static void ProcessExplosion(GridComponent gridComponent, int damagePool)
    {
        var grid = gridComponent.gridPresenter;
        var centerPos = gridComponent.currentPosition;

        ApplyDamageToGroup(grid, centerPos, s_crossOffsets, ref damagePool);

        if (damagePool > 0)
        {
            ApplyDamageToGroup(grid, centerPos, s_diagonalOffsets, ref damagePool);
        }
    }

    private static void ApplyDamageToGroup(MonoGridPresenter grid, Vector2Int center, Vector2Int[] offsets, ref int currentDamage)
    {
        var neighbors = grid.GetNeighbors(center, offsets, e => e != null);

        foreach (var index in neighbors)
        {
            if (currentDamage <= 0) break;

            if (grid.TryGetValue(index, out var neighborProvider))
            {
                TryDamageNeighbor(neighborProvider.Entity, ref currentDamage);
            }
        }
    }

    private static void TryDamageNeighbor(EcsEntity neighbor, ref int damagePool)
    {
        if (!neighbor.Has<RollComponent>()) return;

        ref var roll = ref neighbor.Get<RollComponent>();
        var absorbedDamage = Mathf.Min(roll.value, damagePool);

        roll.value -= damagePool;
        damagePool -= absorbedDamage;

        if (roll.value <= 0)
        {
            neighbor.AddFrameToEvent<IsDestroy>(new());
        }
        else
        {
            UpdateVisual(neighbor, roll.value);
        }
    }

    private static void UpdateVisual(EcsEntity entity, int value)
    {
        entity.GetProvider<DiceProvider>().spriteRoll.Select(value);
        entity.AddFrameToEvent<IsTargetingEvent>();
    }
}

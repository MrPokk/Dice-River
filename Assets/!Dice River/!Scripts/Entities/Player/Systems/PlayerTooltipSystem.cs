using BitterECS.Core;
using UnityEngine;

public class PlayerTooltipSystem : IEcsRunSystem
{
    public Priority Priority => Priority.Low;

    private readonly EcsFilter _playerFilter =
        Build.For<EntitiesPresenter>()
             .Filter()
             .WhereProvider<PlayerProvider>()
             .Include<FacingComponent>();

    private const float IdleThreshold = 2.0f;

    public void Run()
    {
        foreach (var entity in _playerFilter)
        {
            ref var state = ref entity.GetOrAdd<TooltipStateComponent>();

            if (entity.Has<IsMovingComponent>())
            {
                ResetTooltip(ref state);
                continue;
            }

            if (state.activeTooltip == null)
            {
                state.idleTime += Time.deltaTime;

                if (state.idleTime >= IdleThreshold)
                {
                    SpawnTooltip(entity, ref state);
                }
            }
        }
    }

    private void SpawnTooltip(EcsEntity entity, ref TooltipStateComponent state)
    {
        if (state.activeTooltip != null) return;

        var facing = entity.Get<FacingComponent>().direction;
        var provider = entity.GetProvider<PlayerProvider>();
        var monoGrid = Startup.GridRaft.monoGrid;

        var checkPosition = provider.transform.position + (facing.normalized * 0.75f);
        var targetGridPos = monoGrid.ConvertingPosition(checkPosition);

        if (monoGrid.IsWithinGrid(targetGridPos))
        {
            var spawnPos = monoGrid.ConvertingPosition(targetGridPos) + new Vector3(0, 2f, 0);
            var isGetDice = monoGrid.TryGetValue(targetGridPos, out var providerEcs);
            if (isGetDice && providerEcs is DiceProvider diceProvider)
            {
                var infoDice = diceProvider.spriteIcon.Prefab().ToEntity();
                var nameComponent = infoDice.Get<NameComponent>();
                var descriptorComponent = infoDice.Get<DescriptionComponent>();
                var abilityDescriptorComponent = infoDice.Get<AbilityDescriptorComponent>();
                infoDice.Destroy();

                var uITooltipCanvas = InstantiateTooltip(spawnPos);
                uITooltipCanvas.tooltipPopup.Bind(nameComponent, descriptorComponent, abilityDescriptorComponent);
                state.activeTooltip = uITooltipCanvas;
            }
        }
    }

    private void ResetTooltip(ref TooltipStateComponent state)
    {
        state.idleTime = 0;

        if (state.activeTooltip != null)
        {
            Object.Destroy(state.activeTooltip.gameObject);
            state.activeTooltip = null;
        }
    }

    private UITooltipCanvas InstantiateTooltip(Vector3 position)
    {
        return new Loader<UITooltipCanvas>(UiPopupsPaths.UITOOLTIP_CANVAS).New(position, Quaternion.identity);
    }
}

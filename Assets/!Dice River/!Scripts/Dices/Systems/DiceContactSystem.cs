using BitterECS.Core;
using UnityEngine;

public class DiceContactSystem
{
    public Priority Priority => Priority.Medium;

    private readonly EcsFilter _diceFilter = new EcsFilter<DicePresenter>()
        .WhereProvider<DiceProvider>()
        .Include<WaveComponent>();

    private readonly EcsFilter _playerFilter = new EcsFilter<EntitiesPresenter>()
        .WhereProvider<PlayerProvider>();

    public void Run()
    {
        foreach (var diceEntity in _diceFilter)
        {
            ref var waveComp = ref diceEntity.Get<WaveComponent>();

            waveComp.isDepressed = false;
        }

        // 2. Обрабатываем столкновения игроков (обычно один игрок)
        foreach (var playerEntity in _playerFilter)
        {
            // Проверяем, есть ли событие столкновения с контроллером
            if (playerEntity.Has<IsColliderHit>())
            {
                ref var hitComponent = ref playerEntity.Get<IsColliderHit>();
                var hitCollider = hitComponent.hit.collider;

                if (hitCollider != null)
                {
                    // Пытаемся найти компонент DiceProvider на коллайдере или его родителе
                    var diceProvider = hitCollider.GetComponentInParent<DiceProvider>();
                    if (diceProvider != null)
                    {
                        ref var waveComp = ref diceProvider.Entity.Get<WaveComponent>();

                        waveComp.isDepressed = true;
                    }
                }
            }

            // (Опционально) обработка триггеров, если требуется
            // if (playerEntity.Has<IsTriggerColliderEnter>())
            // {
            //     ref var trigger = ref playerEntity.Get<IsTriggerColliderEnter>();
            //     var diceProvider = trigger.other.GetComponentInParent<DiceProvider>();
            //     if (diceProvider != null) diceProvider.isDepressed = true;
            // }
        }
    }
}

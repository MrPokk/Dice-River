using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

[RequireComponent(typeof(RollComponentProvider), typeof(GridComponentProvider))]
public class DiceProvider : ProviderEcs<DicePresenter>
{
    [SerializeField] private SpriteRollComponent _spriteRoll;
    protected override void Awake()
    {
        _spriteRoll ??= GetComponentInChildren<SpriteRollComponent>();

        base.Awake();
        Entity.AddFrame<IsRollingProcess>(new(), () =>
        {
            ReRolling();
        });
    }

    public int ReRolling()
    {
        var randomValue = Random.Range(1, 6);
        _spriteRoll.Select(randomValue);
        return Entity.Get<RollComponent>().currentRole = randomValue;
    }

}

public class DicePresenter : EcsPresenter
{
}

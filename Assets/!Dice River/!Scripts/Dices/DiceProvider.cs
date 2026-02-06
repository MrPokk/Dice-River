using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

[RequireComponent(typeof(RollComponentProvider), typeof(GridComponentProvider))]
public class DiceProvider : ProviderEcs<DicePresenter>
{
    private SpriteRollComponent _spriteRoll;
    protected override void Awake()
    {
        _spriteRoll = GetComponent<SpriteRollComponent>();

        base.Awake();
        Entity.AddFrame<IsRollingProcess>(new(), () =>
        {
            ReRolling();
        });
    }

    public int ReRolling()
    {
        return Entity.Get<RollComponent>().currentRole = Random.Range(1, 6);
    }

}

public class DicePresenter : EcsPresenter
{
}

using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

[RequireComponent(typeof(RollComponentProvider), typeof(GridComponentProvider))]
public class DiceProvider : ProviderEcs<DicePresenter>
{
    public SpriteRollComponent spriteRoll;
    public SpriteSideComponent spriteSide;
    protected override void Awake()
    {
        spriteRoll ??= GetComponentInChildren<SpriteRollComponent>();

        base.Awake();
        Entity.AddFrame<IsRollingProcess>(new(), () =>
        {
            ReRolling();
        });
    }

    public int ReRolling()
    {
        var randomValue = Random.Range(1, 6);
        spriteRoll.Select(randomValue);
        return Entity.Get<RollComponent>().currentRole = randomValue;
    }

}

public class DicePresenter : EcsPresenter
{
}

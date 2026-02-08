using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

[RequireComponent(typeof(RollComponentProvider), typeof(GridComponentProvider))]

public class DiceProvider : ProviderEcs<DicePresenter>
{
    public SpriteIconComponent spriteIcon;
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

    private void OnTriggerEnter(Collider other)
    {
        var entity = other.GetComponent<ProviderEcs>().Entity;
        Entity.AddFrame<IsTriggerColliderEnter>(new(other, entity));
    }

    public int ReRolling()
    {
        var randomValue = Random.Range(1, 6);
        spriteRoll.Select(randomValue);
        return Entity.Get<RollComponent>().value = randomValue;
    }

}

public class DicePresenter : EcsPresenter
{
    protected override void Registration()
    { }
}

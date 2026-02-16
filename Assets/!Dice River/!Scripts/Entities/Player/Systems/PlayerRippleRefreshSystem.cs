using BitterECS.Core;

public class PlayerRippleRefreshSystem : IEcsFixedRunSystem
{
    public Priority Priority => Priority.High;

    private EcsFilter _ecsEntitiesOn = new EcsFilter<EntitiesPresenter>()
        .Include<GravityComponent>(c => c.isGrounded && c.verticalVelocity == 0)
        .Include<SpriteRipplesComponent>();

    private EcsFilter _ecsEntitiesOff = new EcsFilter<EntitiesPresenter>()
        .Include<GravityComponent>(c => !(c.isGrounded && c.verticalVelocity == 0))
        .Include<SpriteRipplesComponent>();

    public void FixedRun()
    {
        foreach (var entity in _ecsEntitiesOff)
        {
            var spriteRipplesComponent = entity.Get<SpriteRipplesComponent>();
            var ripplesObj = spriteRipplesComponent.ripplesObject;

            if (ripplesObj && ripplesObj.gameObject.activeSelf)
                ripplesObj.gameObject.SetActive(false);
        }

        foreach (var entity in _ecsEntitiesOn)
        {
            var spriteRipplesComponent = entity.Get<SpriteRipplesComponent>();

            var ripplesObj = spriteRipplesComponent.ripplesObject;

            if (ripplesObj && !ripplesObj.gameObject.activeSelf)
                ripplesObj.gameObject.SetActive(true);
        }
    }
}

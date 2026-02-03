using BitterECS.Core;
using UnityEngine;

public class PlayerGravitySystem : IEcsFixedRunSystem
{
    public Priority Priority => Priority.Medium;

    private EcsFilter _ecsFilter =
    Build.For<EntitiesPresenter>()
         .Filter()
         .WhereProvider<PlayerProvider>()
         .Include<GravityComponent>();

    public void FixedRun()
    {
        foreach (var entity in _ecsFilter)
        {
            var provider = entity.GetProvider<PlayerProvider>();
            ref var gravityComp = ref entity.Get<GravityComponent>();
            var cc = provider.characterController;

            gravityComp.verticalVelocity -= gravityComp.gravity * Time.fixedDeltaTime;

            var pos = provider.transform.position;
            var groundCheckOffset = gravityComp.groundCheckOffset;
            if (pos.y <= groundCheckOffset)
            {
                pos.y = groundCheckOffset;

                gravityComp.verticalVelocity = 0f;
                gravityComp.isGrounded = false;
            }
            else
            {
                cc.Move(gravityComp.verticalVelocity * Time.fixedDeltaTime * Vector3.up);
                gravityComp.isGrounded = cc.isGrounded;
                if (gravityComp.isGrounded && gravityComp.verticalVelocity < 0)
                {
                    gravityComp.verticalVelocity = -2f;
                }
            }
        }
    }
}

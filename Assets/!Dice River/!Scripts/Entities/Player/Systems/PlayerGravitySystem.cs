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

            gravityComp.isGrounded = (cc.collisionFlags & CollisionFlags.Below) != 0;

            if (gravityComp.isGrounded && gravityComp.verticalVelocity < 0)
            {
                gravityComp.verticalVelocity = 0f;
            }

            if (provider.transform.position.y > gravityComp.groundCheckOffset)
            {
                gravityComp.verticalVelocity -= gravityComp.gravity * Time.fixedDeltaTime;
            }
            else
            {
                gravityComp.verticalVelocity = 0f;

                var pos = provider.transform.position;
                if (pos.y < gravityComp.groundCheckOffset)
                {
                    provider.transform.position = new Vector3(pos.x, gravityComp.groundCheckOffset, pos.z);
                }
            }

            var verticalMove = gravityComp.verticalVelocity * Time.fixedDeltaTime * Vector3.up;
            cc.Move(verticalMove);
        }
    }
}

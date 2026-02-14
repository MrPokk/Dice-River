using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

[RequireComponent(typeof(SpriteRipplesComponent))]
public class EnvironmentProvider : ProviderEcs<EnvironmentPresenter>
{
    [ReadOnly] public SpriteRipplesComponent spriteRipple;
    private void Start()
    {
        spriteRipple = GetComponent<SpriteRipplesComponent>();
    }
}

public class EnvironmentPresenter : EcsPresenter
{ }

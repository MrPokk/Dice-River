using BitterECS.Integration;
using UnityEngine;

[RequireComponent(typeof(SpriteRipplesComponent))]
public class HazardProvider : EntitiesProvider
{
    [ReadOnly] public SpriteRipplesComponent spriteRipple;
    private void Start()
    {
        spriteRipple = GetComponent<SpriteRipplesComponent>();
    }
}

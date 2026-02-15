using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public class SpriteRipplesComponent
{
    public SpriteRenderer ripplesObject;
}

public class SpriteRipplesComponentProvider : ProviderEcs<SpriteRipplesComponent>
{
}

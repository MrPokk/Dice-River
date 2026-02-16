using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public class SpriteRipplesComponent
{
    public SpriteRenderer ripplesObject;
    [ReadOnly] public Vector3 baseScale;
}

public class SpriteRipplesComponentProvider : ProviderEcs<SpriteRipplesComponent>
{
    protected override void Awake()
    {
        _value.baseScale = _value.ripplesObject.transform.localScale;
        base.Awake();
    }
}

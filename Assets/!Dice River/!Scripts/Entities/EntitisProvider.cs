using System;
using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

public class EntitiesProvider : ProviderEcs<EntitiesPresenter>
{
    public SpriteRenderer spriteRenderer;
    public new Rigidbody2D rigidbody;

    protected override void Awake()
    {
        base.Awake();
        rigidbody = GetComponentInChildren<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        Entity.Add<IsCollisionEnter>(new(collision2D));
    }

    void OnCollisionExit2D(Collision2D collision2D)
    {
        Entity.Remove<IsCollisionEnter>();
    }
}

public class EntitiesPresenter : EcsPresenter
{
    protected override void Registration()
    {
        AddCheckEvent<IsCollisionEnter>();
    }
}


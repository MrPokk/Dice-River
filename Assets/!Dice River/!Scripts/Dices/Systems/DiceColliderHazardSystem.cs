using System;
using BitterECS.Core;
using UnityEngine;

public class DiceColliderHazardSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private EcsEvent _ecsEvent =
    new EcsEvent<DicePresenter>()
        .Subscribe<IsTriggerColliderEnter>(added: OnDiceCollider);

    private static void OnDiceCollider(EcsEntity entity)
    {
        Debug.Log("ds");
    }
}

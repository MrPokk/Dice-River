using System;
using BitterECS.Core;
using UnityEngine;
using DG.Tweening;

public class TagDiceMirrorSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private readonly EcsEvent _ecsEventAdding =
    new EcsEvent<DicePresenter>()
    .SubscribeWhereEntity<IsActivatingEvent>(e =>
        EcsConditions.Has<TagMirrorCopy>(e), removed: OnAdding);

    private static void OnAdding(EcsEntity entity)
    {
        var gridDice = entity.Get<GridComponent>();
        var targetingCopy = entity.Get<TagMirrorCopy>();

        var toCopyIndex = targetingCopy.indexToCopy + gridDice.currentPosition;
        var isCopy = gridDice.gridPresenter.TryGetValue(toCopyIndex, out var copyProvider);

        if (!isCopy || copyProvider == null)
        {
            return;
        }

        var diceCopy = (DiceProvider)copyProvider;

        Vector2Int[] offsetsToTry =
        {
            targetingCopy.indexToPast,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var offset in offsetsToTry)
        {
            var targetIndex = gridDice.currentPosition + offset;

            var isIndexValid = gridDice.gridPresenter.TryGetValue(targetIndex, out var existingProvider);

            if (isIndexValid && existingProvider == null)
            {
                DiceInteractionSystem.InstantiateObject(targetIndex, diceCopy, out var instantiatedObject);

                if (instantiatedObject != null)
                {
                    var defaultScale = instantiatedObject.transform.localScale;
                    instantiatedObject.transform.localScale = Vector3.zero;
                    instantiatedObject.transform.DOScale(defaultScale, 0.3f).SetEase(Ease.OutBack).Play();
                }

                return;
            }
        }
    }
}

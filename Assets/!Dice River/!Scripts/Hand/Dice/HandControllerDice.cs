using System.Collections.Generic;
using BitterECS.Core;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using UINotDependence.Core;

public class HandControllerDice : HandController<KeyValuePair<EcsEntity, DiceProvider>, UIProvider>
{
    [Header("Setting Controller Dice")]
    public float timeRefreshSecond;
    public uint maxCountDice;

    [Header("Animation")]
    public float animDuration = 0.5f;
    public float jumpPower = 100f;

    [Header("Arrow Selector Settings")]
    public Vector3 arrowOffset = new(0, 100f, 0);

    public override void Initialize(HandStackController<KeyValuePair<EcsEntity, DiceProvider>, UIProvider> handStackController)
    {
        base.Initialize(handStackController);

        for (var i = 0; i < maxCountDice; i++)
        {
            handStackController.DrawToHand();
        }
    }

    protected override void OnChangedInternal()
    {
        UpdateArrowPosition();
    }

    private void UpdateArrowPosition()
    {
        var firstView = GetViews().FirstOrDefault();
        if (firstView == null)
        {
            UIController.ClosePopup<UIArrowSelectorPopup>();
            return;
        }

        if (!UIController.TryGetOpenedPopup<UIArrowSelectorPopup>(out var popup))
        {
            popup = UIController.OpenPopup<UIArrowSelectorPopup>();
        }

        if (popup != null)
        {
            popup.transform.position = firstView.transform.position + arrowOffset;
        }
    }

    private void LateUpdate()
    {
        if (Items.Count <= 0)
        {
            return;
        }
        UpdateArrowPosition();
    }

    protected override void AnimateEntry(UIProvider view)
    {
        var rt = view.GetComponent<RectTransform>();
        var layout = view.GetComponent<LayoutElement>();

        rt.DOKill();
        if (layout != null) layout.ignoreLayout = true;

        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        rt.localScale = Vector3.one * 0.5f;

        DOTween.Sequence()
            .Join(rt.DOLocalJump(Vector3.zero, jumpPower, 1, animDuration).SetEase(Ease.OutQuad))
            .Join(rt.DOPunchRotation(new Vector3(0, 0, 20f), animDuration))
            .Join(rt.DOScale(Vector3.one, animDuration).SetEase(Ease.OutBack))
            .OnUpdate(() =>
            {
                if (Items.Count > 0 && GetViews().First() == view)
                {
                    UpdateArrowPosition();
                }
            })
            .OnComplete(() =>
            {
                if (layout != null) layout.ignoreLayout = false;
                UpdateArrowPosition();
            })
            .SetTarget(rt)
            .Play();
    }

    public override bool ExtractToFirst(out KeyValuePair<EcsEntity, DiceProvider> val)
    {
        var result = base.ExtractToFirst(out val);
        if (result)
        {
            EcsSystems.Run<IHandSucceedExtraction>(s => s.ResultSucceedExtraction(this));
            if (Items.Count == 0) EcsSystems.Run<IHandResultInExtractEnded>(s => s.ResultInExtractEnded(this));
        }
        else EcsSystems.Run<IHandFailExtraction>(s => s.ResultFailExtraction(this));

        return result;
    }

    public override bool Add(KeyValuePair<EcsEntity, DiceProvider> data, UIProvider view)
    {
        if (Items.Count >= maxCountDice)
        {
            EcsSystems.Run<IHandFailAdd>(s => s.ResultFailAdd(this));
            return false;
        }

        var result = base.Add(data, view);
        if (result) EcsSystems.Run<IHandSucceedAdd>(s => s.ResultSucceedAdd(this, data, view));
        else EcsSystems.Run<IHandFailAdd>(s => s.ResultFailAdd(this));
        return result;
    }

    public override bool Remove(KeyValuePair<EcsEntity, DiceProvider> data)
    {
        var result = base.Remove(data);
        if (result)
        {
            EcsSystems.Run<IHandSucceedRemove>(s => s.ResultSucceedRemove(this));
            if (Items.Count == 0) EcsSystems.Run<IHandResultInRemoveEnded>(s => s.ResultInRemoveEnded(this));
        }
        else EcsSystems.Run<IHandFailRemove>(s => s.ResultFailRemove(this));

        return result;
    }
}

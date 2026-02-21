using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UINotDependence.Core;
using InGame.Script.Component_Sound;

public class UISwipingPopup : UIPopup, IPointerClickHandler
{
    [Header("UI Elements")]
    [SerializeField] private List<Image> _imagesLeft;
    [SerializeField] private List<Image> _imagesRight;

    [Header("Animation Settings")]
    [SerializeField] private float _swipeDuration = 0.5f;
    [SerializeField] private float _swipeDistance = 800f;

    [Header("Shake Settings")]
    [SerializeField] private float _shakeStrength = 15f;
    [SerializeField] private int _shakeVibrato = 10;
    [SerializeField] private float _shakeScaleStrength = 0.1f;

    [Header("Auto Swipe Settings")]
    [SerializeField] private float _autoSwipeDelay = 3f;

    [Header("Parallax Settings")]
    [SerializeField] private float _parallaxStrength = 25f;
    [SerializeField] private float _parallaxSmoothness = 5f;

    private int _swipeStage = 0;
    private float _currentAutoTimer;

    private Vector2[] _leftStartPos;
    private Vector2[] _rightStartPos;

    private Vector2[] _leftParallaxFactors;
    private Vector2[] _rightParallaxFactors;

    private static bool s_isOpenSwipingPopups;

    private void Awake()
    {
        if (s_isOpenSwipingPopups)
        {
            UIController.ClosePopup<UISwipingPopup>();
            return;
        }

        s_isOpenSwipingPopups = true;

        _leftStartPos = new Vector2[_imagesLeft.Count];
        _leftParallaxFactors = new Vector2[_imagesLeft.Count];
        for (var i = 0; i < _imagesLeft.Count; i++)
        {
            _leftStartPos[i] = _imagesLeft[i].rectTransform.anchoredPosition;
            var depth = 1f + (i * 0.3f);
            _leftParallaxFactors[i] = new Vector2(Random.Range(0.5f, 1.5f) * depth, Random.Range(0.5f, 1.5f) * depth);
        }

        _rightStartPos = new Vector2[_imagesRight.Count];
        _rightParallaxFactors = new Vector2[_imagesRight.Count];
        for (var i = 0; i < _imagesRight.Count; i++)
        {
            _rightStartPos[i] = _imagesRight[i].rectTransform.anchoredPosition;
            var depth = 1f + (i * 0.3f);
            _rightParallaxFactors[i] = new Vector2(Random.Range(0.5f, 1.5f) * depth, Random.Range(0.5f, 1.5f) * depth);
        }
    }

    private void OnEnable()
    {
        ResetPopup();
    }

    private void OnDisable()
    {
        foreach (var img in _imagesLeft) img.rectTransform.DOKill();
        foreach (var img in _imagesRight) img.rectTransform.DOKill();
    }

    private void Update()
    {
        if (_swipeStage >= 2) return;

        _currentAutoTimer -= Time.unscaledDeltaTime;
        if (_currentAutoTimer <= 0f)
        {
            ExecuteSwipe();
        }

        var mousePos = ControllableSystem.PointerPosition;
        var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        var offset = (mousePos - screenCenter) / screenCenter;

        if (_swipeStage < 1) ApplyParallax(_imagesLeft, _leftStartPos, _leftParallaxFactors, offset);
        if (_swipeStage < 2) ApplyParallax(_imagesRight, _rightStartPos, _rightParallaxFactors, offset);
    }

    private void ApplyParallax(List<Image> images, Vector2[] startPos, Vector2[] factors, Vector2 offset)
    {
        for (var i = 0; i < images.Count; i++)
        {
            var uniqueOffset = new Vector2(offset.x * factors[i].x, offset.y * factors[i].y);

            var floatOffset = new Vector2(
                Mathf.Sin(Time.unscaledTime * 1.5f + i) * 3f,
                Mathf.Cos(Time.unscaledTime * 1.2f + i) * 3f
            );

            var targetPos = startPos[i] + (uniqueOffset * _parallaxStrength) + floatOffset;

            images[i].rectTransform.anchoredPosition = Vector2.Lerp(
                images[i].rectTransform.anchoredPosition,
                targetPos,
                Time.unscaledDeltaTime * _parallaxSmoothness);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ExecuteSwipe();
    }

    private void SoundEffect()
    {
        SoundController.PlaySoundRandomPitch(SoundType.MenuClick);
    }

    private void ExecuteSwipe()
    {
        if (_swipeStage >= 2) return;

        SoundEffect();

        if (_swipeStage == 0)
        {
            SwipeLeftImages();
            _swipeStage++;
            _currentAutoTimer = _autoSwipeDelay;
        }
        else if (_swipeStage == 1)
        {
            SwipeRightImages();
            _swipeStage++;

            DOVirtual.DelayedCall(_swipeDuration, Close).SetUpdate(true);
        }
    }

    private void SwipeLeftImages()
    {
        foreach (var img in _imagesLeft)
        {
            var seq = DOTween.Sequence();
            seq.SetUpdate(true);
            seq.Join(img.rectTransform.DOAnchorPosX(_swipeDistance, _swipeDuration).SetRelative(true).SetEase(Ease.InOutQuad));
            seq.Join(img.rectTransform.DOShakeRotation(_swipeDuration, new Vector3(0, 0, _shakeStrength), _shakeVibrato, 90f));
            seq.Join(img.rectTransform.DOShakeScale(_swipeDuration, _shakeScaleStrength, _shakeVibrato, 90f));
            seq.Play();
        }
    }

    private void SwipeRightImages()
    {
        foreach (var img in _imagesRight)
        {
            var seq = DOTween.Sequence();
            seq.SetUpdate(true);
            seq.Join(img.rectTransform.DOAnchorPosX(-_swipeDistance, _swipeDuration).SetRelative(true).SetEase(Ease.InOutQuad));
            seq.Join(img.rectTransform.DOShakeRotation(_swipeDuration, new Vector3(0, 0, _shakeStrength), _shakeVibrato, 90f));
            seq.Join(img.rectTransform.DOShakeScale(_swipeDuration, _shakeScaleStrength, _shakeVibrato, 90f));
            seq.Play();
        }
    }

    public void ResetPopup()
    {
        _swipeStage = 0;
        _currentAutoTimer = _autoSwipeDelay;

        foreach (var img in _imagesLeft) img.rectTransform.DOKill();
        foreach (var img in _imagesRight) img.rectTransform.DOKill();

        if (_leftStartPos != null)
        {
            for (var i = 0; i < _imagesLeft.Count; i++)
            {
                _imagesLeft[i].rectTransform.anchoredPosition = _leftStartPos[i];
                _imagesLeft[i].rectTransform.localRotation = Quaternion.identity;
                _imagesLeft[i].rectTransform.localScale = Vector3.one;
            }
        }

        if (_rightStartPos != null)
        {
            for (var i = 0; i < _imagesRight.Count; i++)
            {
                _imagesRight[i].rectTransform.anchoredPosition = _rightStartPos[i];
                _imagesRight[i].rectTransform.localRotation = Quaternion.identity;
                _imagesRight[i].rectTransform.localScale = Vector3.one;
            }
        }
    }
}

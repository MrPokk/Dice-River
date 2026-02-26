using System.Linq;
using UINotDependence.Core;
using UnityEngine;
using DG.Tweening;
using BitterECS.Core;
using BitterECS.Integration.Unity;
using TMPro;
using UnityEngine.UI;

public class UIDefeatScreen : UIScreen
{
    [SerializeField] private GameObject scoreElement;
    [SerializeField] private TMP_Text textScore;
    [SerializeField] private GridLayoutGroup containerIcon;
    [SerializeField] private GameObject mainElement;
    [SerializeField] private UIRestartButtonComponent restartButton;
    [SerializeField] private Image slideBackground;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float scoreAppearDuration = 0.5f;
    [SerializeField] private float scoreCountDuration = 1.5f;

    public override void Open()
    {
        UIController.CloseAllPopups();
        mainElement.SetActive(false);
        restartButton.AddListener(Restart);

        textScore.text = "0";
        scoreElement.transform.localScale = Vector3.zero;

        slideBackground.DOFade(0f, 0f).Play();
        slideBackground.gameObject.SetActive(true);

        InstantiateAllUI();
        base.Open();

        slideBackground.DOFade(1f, fadeDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                mainElement.SetActive(true);
                slideBackground.gameObject.SetActive(false);
                AnimateScore();
            }).Play();
    }

    private void AnimateScore()
    {
        scoreElement.transform.DOScale(1f, scoreAppearDuration)
            .SetEase(Ease.OutBack).Play();

        int targetScore = GFlow.GState.totalScrollDistance;
        var currentDisplayedScore = 0;

        DOTween.To(() => currentDisplayedScore, x =>
        {
            currentDisplayedScore = x;
            textScore.text = currentDisplayedScore.ToString();
        }, targetScore, scoreCountDuration)
        .SetEase(Ease.OutQuad).Play();
    }

    public override void Close()
    {
        slideBackground.DOKill();
        scoreElement.transform.DOKill();
        DOTween.Kill(textScore);
        restartButton.RemoveListener(Restart);
        base.Close();
    }

    private void Restart()
    {
        restartButton.RemoveListener(Restart);

        slideBackground.gameObject.SetActive(true);
        slideBackground.DOFade(0f, 0f);

        slideBackground.DOFade(1f, fadeDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                Startup.Restart();
            }).Play();
    }

    private void InstantiateAllUI()
    {
        var collected = GFlow.GState.collectedDiceTypes;
        var allPrefabs = UiIconPaths.AllPaths.Select(path => new Loader<UIProvider>(path).Prefab());
        var missing = allPrefabs.Except(collected);

        foreach (var dice in collected)
        {
            Instantiate(dice, containerIcon.transform);
        }

        foreach (var dice in missing)
        {
            var instance = Instantiate(dice, containerIcon.transform);
            instance.GetComponent<Image>().color = Color.black;

            new EcsBuilder<UIPresenter>()
                .WithLink(instance)
                .With<IsNotRaycast>()
                .Create();
        }
    }
}

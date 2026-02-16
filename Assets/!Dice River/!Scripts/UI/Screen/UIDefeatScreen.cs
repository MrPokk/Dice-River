using System.Linq; // Добавлено для LINQ
using UINotDependence.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using BitterECS.Core;

public class UIDefeatScreen : UIScreen
{
    [SerializeField] private GridLayoutGroup containerIcon;
    [SerializeField] private GameObject mainElement;
    [SerializeField] private UIRestartButtonComponent restartButton;
    [SerializeField] private Image slideBackground;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1.0f;

    public override void Open()
    {
        UIController.CloseAllPopups();
        mainElement.SetActive(false);
        restartButton.AddListener(Restart);

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
            }).Play();
    }

    public override void Close()
    {
        slideBackground.DOKill();
        restartButton.RemoveListener(Restart);
        base.Close();
    }
    private void Restart()
    {
        restartButton.RemoveListener(Restart);

        slideBackground.gameObject.SetActive(true);
        slideBackground.DOFade(0f, 0f).Play();

        slideBackground.DOFade(1f, fadeDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }).Play();
    }

    private void InstantiateAllUI()
    {
        var collected = StartupGameplay.GState.collectedDiceTypes;

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

using System.Collections.Generic;
using UINotDependence.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIDefeatScreen : UIScreen
{
    public GridLayoutGroup containerIcon;
    public GameObject mainElement;
    public UIRestartButtonComponent restartButton;
    public Image slideBackground;

    public override void Open()
    {
        mainElement.SetActive(false);
        restartButton.AddListener(Restart);
        InstantiateAllUI();
        base.Open();
    }

    public override void Close()
    {
        restartButton.RemoveListener(Restart);
        base.Close();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void InstantiateAllUI()
    {
        var collectedDiceTypes = StartupGameplay.GState.collectedDiceTypes;
        var allUiPaths = UiIconPaths.AllPaths;
        var listMissing = new List<UIProvider>();
        foreach (var ui in allUiPaths)
        {
            var diceTypeUI = new Loader<UIProvider>(ui).Prefab();
            if (!collectedDiceTypes.Contains(diceTypeUI))
            {
                listMissing.Add(diceTypeUI);
            }
        }

        foreach (var diceType in collectedDiceTypes)
        {
            Instantiate(diceType, containerIcon.transform);
        }
        foreach (var diceType in listMissing)
        {
            var notUnlock = Instantiate(diceType, containerIcon.transform);
            notUnlock.GetComponent<Image>().color = Color.black;
            notUnlock.Entity.Add<IsNotRaycast>();

        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum DiceVisualState
{
    Default,
    Lifted
}

public class SpriteSideComponent : MonoBehaviour
{
    [Header("Renderers")]
    public SpriteRenderer frontSidesObject;
    public SpriteRenderer leftSidesObject;
    public SpriteRenderer rightSidesObject;
    public SpriteRenderer ripplesObject;

    [System.Serializable]
    public struct StateSprites
    {
        public DiceVisualState state;
        public Sprite frontSprite;
        public Sprite leftSprite;
        public Sprite rightSprite;
    }

    [Header("Configuration")]
    public List<StateSprites> stateConfigs;

    private StateSprites _currentState;

    public void SetState(DiceVisualState newState)
    {
        var config = stateConfigs.FirstOrDefault(x => x.state == newState);
        _currentState = config;

        if (frontSidesObject != null) frontSidesObject.sprite = config.frontSprite;
        if (leftSidesObject != null) leftSidesObject.sprite = config.leftSprite;
        if (rightSidesObject != null) rightSidesObject.sprite = config.rightSprite;
    }

    public void SetFrontActive(bool isActive)
    {
        if (frontSidesObject.gameObject.activeSelf != isActive)
            frontSidesObject.gameObject.SetActive(isActive);
    }

    public void SetLeftActive(bool isActive)
    {
        if (leftSidesObject.gameObject.activeSelf != isActive)
            leftSidesObject.gameObject.SetActive(isActive);
    }

    public void SetRightActive(bool isActive)
    {
        if (rightSidesObject.gameObject.activeSelf != isActive)
            rightSidesObject.gameObject.SetActive(isActive);
    }

    public void SetRippleActive(bool isActive)
    {
        if (ripplesObject.gameObject.activeSelf != isActive)
            ripplesObject.gameObject.SetActive(isActive);
    }

    public void ResetSides()
    {
        SetFrontActive(true);
        SetLeftActive(true);
        SetRightActive(true);
    }

    public void ToggleFront() => SetFrontActive(!frontSidesObject.gameObject.activeSelf);
    public void ToggleLeft() => SetLeftActive(!leftSidesObject.gameObject.activeSelf);
    public void ToggleRight() => SetRightActive(!rightSidesObject.gameObject.activeSelf);
    public void ToggleRipple() => SetRippleActive(!ripplesObject.gameObject.activeSelf);
}

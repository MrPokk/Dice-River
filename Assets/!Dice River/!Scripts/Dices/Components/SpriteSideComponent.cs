using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public SpriteRenderer topSidesObject;
    public SpriteRenderer ripplesObject;

    [System.Serializable]
    public struct StateSprites
    {
        public DiceVisualState state;

        public bool useSingleSprite;
        public Sprite allSidesSprite;

        public Sprite frontSprite;
        public Sprite leftSprite;
        public Sprite rightSprite;

        public Sprite topSprite;
    }

    [Header("Configuration")]
    [Tooltip("Select a state to preview the dice appearance.")]
    public DiceVisualState previewState;
    public List<StateSprites> stateConfigs;
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (stateConfigs != null && stateConfigs.Count > 0)
        {
            EditorApplication.delayCall += () =>
            {
                if (this != null) SetState(previewState);
            };
        }
    }
#endif

    public void SetState(DiceVisualState newState)
    {
        var config = stateConfigs.FirstOrDefault(x => x.state == newState);

        if (string.IsNullOrEmpty(config.state.ToString()) && config.topSprite == null) return;

        Sprite f, l, r;
        if (config.useSingleSprite)
        {
            f = l = r = config.allSidesSprite;
        }
        else
        {
            f = config.frontSprite;
            l = config.leftSprite;
            r = config.rightSprite;
        }

        if (frontSidesObject != null) frontSidesObject.sprite = f;
        if (leftSidesObject != null) leftSidesObject.sprite = l;
        if (rightSidesObject != null) rightSidesObject.sprite = r;
        if (topSidesObject != null) topSidesObject.sprite = config.topSprite;
    }

    public void SetFrontActive(bool isActive) => SetObjectActive(frontSidesObject, isActive);
    public void SetLeftActive(bool isActive) => SetObjectActive(leftSidesObject, isActive);
    public void SetRightActive(bool isActive) => SetObjectActive(rightSidesObject, isActive);
    public void SetTopActive(bool isActive) => SetObjectActive(topSidesObject, isActive);
    public void SetRippleActive(bool isActive) => SetObjectActive(ripplesObject, isActive);

    private void SetObjectActive(SpriteRenderer renderer, bool isActive)
    {
        if (renderer != null && renderer.gameObject.activeSelf != isActive)
            renderer.gameObject.SetActive(isActive);
    }

    public void ResetSides()
    {
        SetFrontActive(true);
        SetLeftActive(true);
        SetRightActive(true);
        SetTopActive(true);
    }

    public void ToggleFront() => SetFrontActive(!frontSidesObject.gameObject.activeSelf);
    public void ToggleLeft() => SetLeftActive(!leftSidesObject.gameObject.activeSelf);
    public void ToggleRight() => SetRightActive(!rightSidesObject.gameObject.activeSelf);
    public void ToggleTop() => SetTopActive(!topSidesObject.gameObject.activeSelf);
    public void ToggleRipple() => SetRippleActive(!ripplesObject.gameObject.activeSelf);
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SpriteSideComponent.StateSprites))]
public class StateSpritesDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty state = property.FindPropertyRelative("state");
        SerializedProperty useSingle = property.FindPropertyRelative("useSingleSprite");
        SerializedProperty allSides = property.FindPropertyRelative("allSidesSprite");
        SerializedProperty front = property.FindPropertyRelative("frontSprite");
        SerializedProperty left = property.FindPropertyRelative("leftSprite");
        SerializedProperty right = property.FindPropertyRelative("rightSprite");
        SerializedProperty top = property.FindPropertyRelative("topSprite");

        Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        string stateName = state.enumDisplayNames[state.enumValueIndex];
        property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, $"Config: {stateName}", true);
        rect.y += EditorGUIUtility.singleLineHeight + 2;

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            EditorGUI.PropertyField(rect, state);
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(rect, useSingle, new GUIContent("Use Single Sprite (F,L,R)"));
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            if (useSingle.boolValue)
            {
                EditorGUI.PropertyField(rect, allSides, new GUIContent("All Sides Sprite"));
                rect.y += EditorGUIUtility.singleLineHeight + 2;
            }
            else
            {
                EditorGUI.PropertyField(rect, front);
                rect.y += EditorGUIUtility.singleLineHeight + 2;
                EditorGUI.PropertyField(rect, left);
                rect.y += EditorGUIUtility.singleLineHeight + 2;
                EditorGUI.PropertyField(rect, right);
                rect.y += EditorGUIUtility.singleLineHeight + 2;
            }

            EditorGUI.PropertyField(rect, top);

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;

        SerializedProperty useSingle = property.FindPropertyRelative("useSingleSprite");

        int lineCount = 4;

        if (useSingle.boolValue)
            lineCount += 1;
        else
            lineCount += 3;

        return lineCount * (EditorGUIUtility.singleLineHeight + 2) + 5;
    }
}
#endif

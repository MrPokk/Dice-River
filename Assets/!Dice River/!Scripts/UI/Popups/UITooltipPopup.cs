using TMPro;
using UINotDependence.Core;
using UnityEngine;

public class UITooltipPopup : UIPopup
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _abilityText;

    [Header("Positioning")]
    [SerializeField] private Vector2 _offset = new(10f, -10f);

    private Canvas _rootCanvas;

    private void Awake()
    {
        _rootCanvas = GetComponentInParent<Canvas>();
    }

    public void Bind(
        NameComponent nameComponent,
        DescriptionComponent descriptorComponent,
        AbilityDescriptorComponent abilityDescriptorComponent)
    {
        if (_nameText) _nameText.text = nameComponent.value;
        if (_descriptionText) _descriptionText.text = descriptorComponent.description;

        if (_abilityText)
        {
            _abilityText.text = abilityDescriptorComponent.description;
            _abilityText.gameObject.SetActive(!string.IsNullOrEmpty(abilityDescriptorComponent.description));
        }
    }

    public override void Open()
    {
        UIAnimationComponent
        .UsingAnimation(gameObject)
        .ApplyPresetOpen(UIAnimationPresets.CreateSlideFromRightPreset())
        .PlayOpenAnimation();

        base.Open();
    }

    private void Update()
    {
        if (!gameObject || _rootCanvas == null)
            return;

        Vector3 pointerPos = ControllableSystem.PointerPosition;

        var scaleFactor = _rootCanvas.scaleFactor;
        var scaledOffset = (Vector3)_offset * scaleFactor;

        transform.position = pointerPos + scaledOffset;
    }

    public override void Close()
    {
        base.Close();
    }
}

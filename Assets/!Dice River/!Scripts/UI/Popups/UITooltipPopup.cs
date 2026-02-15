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
        if (!gameObject)
            return;

        Vector3 pointerPos = ControllableSystem.PointerPosition;

        transform.position = new Vector3(
            pointerPos.x + _offset.x,
            pointerPos.y + _offset.y,
            0f
        );
    }

    public override void Close()
    {
        base.Close();
    }
}

using System.Linq;
using BitterECS.Core;
using BitterECS.Integration.Unity;
using UnityEngine;

public class PlayerSelectorMoveSystem : IStartToGameplay, IEcsFixedRunSystem
{
    public Priority Priority => Priority.Medium;

    private readonly EcsFilter _ecsFilter = new EcsFilter<EntitiesPresenter>()
         .WhereProvider<EntitiesProvider>()
         .Include<InputComponent>()
         .Include<FacingComponent>();

    private UISelectorElement _selector;
    private Transform _selectorTransform;

    private readonly Vector3 _visualOffset = new(0, 0.55f, 0);
    private readonly float _moveSpeed = 20f;
    private const float LookDistance = 0.75f;

    public void ToStart()
    {
        _selector = new Loader<UISelectorElement>(UiPrefabsPaths.UISELECTOR).New();
        _selectorTransform = _selector.transform;
        _selector.SetVisible(false);
    }

    public void FixedRun()
    {
        foreach (var entity in _ecsFilter)
        {
            UpdateSelector(entity);
        }
    }

    private void UpdateSelector(EcsEntity entity)
    {
        if (_selector == null)
            return;

        var monoGrid = Startup.GridRaft.monoGrid;
        var transform = entity.GetProvider<EntitiesProvider>().transform;
        var direction = entity.Get<FacingComponent>().direction.normalized;

        var checkPosition = transform.position + (direction * LookDistance);
        var targetGridPos = monoGrid.ConvertingPosition(checkPosition);

        if (!monoGrid.IsWithinGrid(targetGridPos))
        {
            _selector.SetVisible(false);
            return;
        }

        var targetObject = monoGrid.GetGameObject(targetGridPos);
        var isSlotEmpty = targetObject == null;
        var isCarrying = entity.Has<IsGrabbingComponent>();
        var isHandEmpty = Startup.HandControllerDice != null && !Startup.HandControllerDice.Items.Any();

        var shouldShow = !isCarrying || isSlotEmpty;

        var isMouseMode = !isCarrying && isSlotEmpty;

        if (!shouldShow || (isMouseMode && isHandEmpty))
        {
            _selector.SetVisualAllIcon(false);
        }
        else
        {
            var showHandIcon = isCarrying || (targetObject is DiceProvider);

            _selector.SetVisible(true);
            _selector.SetVisualMode(showHandIcon);
        }

        var targetWorldPos = monoGrid.ConvertingPosition(targetGridPos) + _visualOffset;
        _selectorTransform.position = Vector3.Lerp(_selectorTransform.position, targetWorldPos, _moveSpeed * Time.fixedDeltaTime);
    }
}

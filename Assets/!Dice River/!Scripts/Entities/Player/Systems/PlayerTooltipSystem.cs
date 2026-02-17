using BitterECS.Core;
using UnityEngine;

public class PlayerTooltipSystem : IEcsRunSystem
{
    public Priority Priority => Priority.Low;

    private readonly EcsFilter _playerFilter = Build.For<EntitiesPresenter>()
        .Filter()
        .WhereProvider<PlayerProvider>()
        .Include<FacingComponent>();

    private const float IdleThreshold = 2.0f;
    private UITooltipCanvas _uiTooltipCanvas;

    private UIAnimationComponent _animationComponent => UIAnimationComponent
            .Using(_uiTooltipCanvas.tooltipPopup.gameObject)
            .SetPresets(UIAnimationPresets.PopupOpen,
                        UIAnimationPresets.PopupClose);

    public void Run()
    {
        foreach (var entity in _playerFilter)
        {
            ProcessPlayer(entity);
        }
    }

    private void ProcessPlayer(EcsEntity entity)
    {
        ref var state = ref entity.GetOrAdd<TooltipStateComponent>();

        if (entity.Has<IsMovingComponent>())
        {
            ResetTooltip(ref state);
            return;
        }

        if (state.activeTooltip == null && (state.idleTime += Time.deltaTime) >= IdleThreshold)
        {
            TrySpawnTooltip(entity, ref state);
        }
    }

    private void TrySpawnTooltip(EcsEntity entity, ref TooltipStateComponent state)
    {
        var grid = Startup.GridRaft.monoGrid;
        var targetGridPos = GetLookAtGridPosition(entity, grid);

        if (grid.IsWithinGrid(targetGridPos) && grid.TryGetValue(targetGridPos, out var provider) && provider is DiceProvider dice)
        {
            var spawnPos = grid.ConvertingPosition(targetGridPos) + Vector3.up * 2f;
            state.activeTooltip = CreateTooltip(dice, spawnPos);
        }
    }

    private Vector2Int GetLookAtGridPosition(EcsEntity entity, MonoGridPresenter grid)
    {
        var facing = entity.Get<FacingComponent>().direction;
        var pos = entity.GetProvider<PlayerProvider>().transform.position;
        return grid.ConvertingPosition(pos + facing.normalized * 0.75f);
    }

    private UITooltipCanvas CreateTooltip(DiceProvider dice, Vector3 position)
    {
        var info = dice.spriteIcon.Prefab().ToEntity();

        if (_uiTooltipCanvas == null)
        {
            _uiTooltipCanvas = new Loader<UITooltipCanvas>(UiPopupsPaths.UITOOLTIP_CANVAS).New(position, Quaternion.identity);
        }
        else
        {
            _uiTooltipCanvas.transform.position = position;
            _uiTooltipCanvas.gameObject.SetActive(true);
        }

        _uiTooltipCanvas.tooltipPopup.Bind(
            info.Get<NameComponent>(),
            info.Get<DescriptionComponent>(),
            info.Get<AbilityDescriptorComponent>()
        );

        _animationComponent.PlayOpen();

        info.Destroy();
        return _uiTooltipCanvas;
    }

    private void ResetTooltip(ref TooltipStateComponent state)
    {
        state.idleTime = 0;
        if (_uiTooltipCanvas != null)
        {
            _animationComponent.PlayClose();
            _uiTooltipCanvas.gameObject.SetActive(false);
        }
        state.activeTooltip = null;
    }
}


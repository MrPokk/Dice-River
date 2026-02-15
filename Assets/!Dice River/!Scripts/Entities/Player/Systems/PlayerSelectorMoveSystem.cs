using BitterECS.Core;
using UnityEngine;

public class PlayerSelectorMoveSystem : IEcsInitSystem, IEcsFixedRunSystem
{
    public Priority Priority => Priority.Medium;

    private EcsFilter _ecsFilter = Build.For<EntitiesPresenter>()
         .Filter()
         .WhereProvider<EntitiesProvider>()
         .Include<InputComponent>()
         .Include<FacingComponent>();

    private Transform _selector;
    private readonly Vector3 _offset = new(0, 0.55f, 0);
    private readonly Vector2Int[] _neighborOffsets = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    public void Init()
    {
        var selectorPrefab = new Loader<GameObject>(UiPrefabsPaths.UISELECTOR).New();
        _selector = selectorPrefab.transform;
        _selector.gameObject.SetActive(false);
    }

    public void FixedRun()
    {
        foreach (var entity in _ecsFilter)
        {
            var transform = entity.GetProvider<EntitiesProvider>().transform;
            var facingDir = entity.Get<FacingComponent>().direction;
            var monoGrid = Startup.GridRaft.monoGrid;

            var checkPosition = transform.position + (facingDir.normalized * 0.75f);
            var targetGridPos = monoGrid.ConvertingPosition(checkPosition);

            if (!monoGrid.IsWithinGrid(targetGridPos))
            {
                if (_selector.gameObject.activeSelf) _selector.gameObject.SetActive(false);
                continue;
            }

            if (monoGrid.IsEmpty)
            {
                _selector.gameObject.SetActive(true);
                continue;
            }

            var isValidPosition = false;

            if (entity.Has<IsGrabbingComponent>())
            {
                isValidPosition = true;
            }
            else
            {


                if (monoGrid.GetGameObject(targetGridPos) is DiceProvider)
                {
                    isValidPosition = true;
                }
                else
                {
                    foreach (var offset in _neighborOffsets)
                    {
                        var neighborPos = targetGridPos + offset;
                        if (monoGrid.GetGameObject(neighborPos) is not DiceProvider)
                        {
                            continue;
                        }

                        isValidPosition = true;
                        break;
                    }
                }
            }

            if (isValidPosition)
            {
                if (!_selector.gameObject.activeSelf) _selector.gameObject.SetActive(true);
                _selector.position = monoGrid.ConvertingPosition(targetGridPos) + _offset;
            }
            else
            {
                if (_selector.gameObject.activeSelf) _selector.gameObject.SetActive(false);
            }
        }
    }
}

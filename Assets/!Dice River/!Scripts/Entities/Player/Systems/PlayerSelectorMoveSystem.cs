using BitterECS.Core;
using UnityEngine;

public class PlayerSelectorMoveSystem : IEcsInitSystem, IEcsFixedRunSystem
{
    public Priority Priority => Priority.Medium;
    private EcsFilter _ecsFilter =
    Build.For<EntitiesPresenter>()
         .Filter()
         .WhereProvider<EntitiesProvider>()
         .Include<InputComponent>()
         .Include<FacingComponent>();

    private Transform _selector;
    private Vector3 _offset = new Vector3(0, 0.55f, 0);

    public void Init()
    {
        var selectorPrefab = new Loader<GameObject>(UiPrefabsPaths.SELECTOR).GetInstance();
        _selector = selectorPrefab.transform;
    }

    public void FixedRun()
    {
        foreach (var entity in _ecsFilter)
        {
            var transform = entity.GetProvider<EntitiesProvider>().transform;

            var facingDir = entity.Get<FacingComponent>().direction;

            var targetPosition = transform.position + facingDir;

            var monoGrid = Startup.GridRaft.monoGrid;
            var gridPosition = monoGrid.ConvertingPosition(targetPosition);
            _selector.position = monoGrid.ConvertingPosition(gridPosition) + _offset;
        }
    }
}

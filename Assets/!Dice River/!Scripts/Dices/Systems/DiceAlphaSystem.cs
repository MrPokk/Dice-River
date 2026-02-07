using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

public class DiceAlphaSystem : IEcsRunSystem
{
    public Priority Priority => Priority.Low;

    private EcsFilter _playerFilter = Build.For<EntitiesPresenter>()
         .Filter()
         .WhereProvider<PlayerProvider>();

    private EcsFilter _diceFilter = Build.For<DicePresenter>()
         .Filter()
         .WhereProvider<DiceProvider>();

    private float _maxDistance = 8f;
    private float _minDistance = 2f;
    private float _falloffPower = 6f;

    public void Run()
    {
        foreach (var diceEntity in _diceFilter)
        {
            var diceProvider = diceEntity.GetProvider<DiceProvider>();
            var spriteRoll = diceProvider.spriteRoll;

            if (spriteRoll == null) continue;

            var closestDistance = float.MaxValue;

            foreach (var playerEntity in _playerFilter)
            {
                var playerTransform = playerEntity.GetProvider<PlayerProvider>().transform;
                var distance = Vector3.Distance(diceProvider.transform.position, playerTransform.position);
                if (distance < closestDistance) closestDistance = distance;
            }

            var t = Mathf.Clamp01((closestDistance - _minDistance) / (_maxDistance - _minDistance));
            var alpha = Mathf.Pow(1f - t, _falloffPower);

            var isActive = alpha > 0.0001f;

            if (spriteRoll.gameObject.activeSelf != isActive)
            {
                spriteRoll.gameObject.SetActive(isActive);
            }

            if (!isActive)
            {
                continue;
            }

            var currentRoll = spriteRoll.GetCurrentRollPrefab();
            if (currentRoll != null && currentRoll.prefab != null)
            {
                spriteRoll.SetAlpha(alpha);
            }
        }
    }
}

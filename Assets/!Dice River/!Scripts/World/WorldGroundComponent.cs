using UnityEngine;

public class WorldGroundComponent : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _spriteVariate;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }
}

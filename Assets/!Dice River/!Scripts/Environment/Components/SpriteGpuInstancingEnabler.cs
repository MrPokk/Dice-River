using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteGpuInstancingEnabler : MonoBehaviour
{
    private void Awake()
    {
        var materialPropertyBlock = new MaterialPropertyBlock();
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}

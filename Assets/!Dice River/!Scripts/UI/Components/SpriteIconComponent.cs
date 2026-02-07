using UnityEngine;

public class SpriteIconComponent : MonoBehaviour
{
    public UIProvider Provider => GetComponent<UIProvider>();

    public UIProvider New()
    {
        return Instantiate(Provider);
    }

    public UIProvider Prefab()
    {
        return Provider;
    }
}

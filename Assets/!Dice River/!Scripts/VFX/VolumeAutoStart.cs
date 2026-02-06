using UnityEngine;
using UnityEngine.Rendering;

public class VolumeAutoStart : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Volume>().weight = 1f;
    }
}

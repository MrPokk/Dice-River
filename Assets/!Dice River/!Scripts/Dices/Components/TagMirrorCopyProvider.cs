using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct TagMirrorCopy
{
    public Vector2Int indexToCopy;
    public Vector2Int indexToPast;
}

public class TagMirrorCopyProvider : ProviderEcs<TagMirrorCopy> { }

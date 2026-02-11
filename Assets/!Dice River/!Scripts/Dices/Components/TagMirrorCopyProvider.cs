using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct TagMirrorCopy { }

[RequireComponent(typeof(NeighborsComponentProvider))]
public class TagMirrorCopyProvider : ProviderEcs<TagMirrorCopy> { }

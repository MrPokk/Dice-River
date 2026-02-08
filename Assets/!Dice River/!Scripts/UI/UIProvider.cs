using System;
using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

[RequireComponent(typeof(UILinkingPrefabComponentProvider))]
public class UIProvider : ProviderEcs<UIPresenter>
{

}

public class UIPresenter : EcsPresenter
{ }

using System;
using BitterECS.Integration.Unity;
using UnityEngine;

[Serializable]
public struct TagRerollDice
{

}

[RequireComponent(typeof(NeighborsComponentProvider))]
public class TagRerollDiceProvider : ProviderEcs<TagRerollDice>
{ }

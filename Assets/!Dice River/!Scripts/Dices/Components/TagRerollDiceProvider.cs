using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct TagRerollDice
{

}

[RequireComponent(typeof(NeighborsComponentProvider))]
public class TagRerollDiceProvider : ProviderEcs<TagRerollDice>
{ }

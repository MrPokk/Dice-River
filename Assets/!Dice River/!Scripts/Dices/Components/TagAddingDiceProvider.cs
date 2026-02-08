using System;
using BitterECS.Integration;

[Serializable]
public struct TagAddingDice
{
    public int addingModification;
}

public class TagAddingDiceProvider : ProviderEcs<TagAddingDice>
{

}

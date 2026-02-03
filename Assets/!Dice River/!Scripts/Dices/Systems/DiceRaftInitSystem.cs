using System.Linq;
using UnityEngine;

public class DiceRaftInitSystem
{
    public static GameObject GridRaftParent;

    public static void Initialize(MonoGridPresenter gridRaft)
    {
        GridRaftParent = new GameObject("GridRaftParent");

        var dicePrefab = new Loader<DiceProvider>(DicesPaths.TEST_DICE).GetPrefab();
        var nodes = gridRaft.GetGridDictionary().Keys.ToArray();
        foreach (var node in nodes)
        {
            DiceSetterSystem.SpawnDiceRaft(node, dicePrefab, out _);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TypeII/Room Definition")]
public class RoomDefinition : ScriptableObject
{
    [Header("Identity")]
    public string id;                 // "arrival", "oxygen", ...
    public string displayName;        // "Oxygen Room"
    public Color roomColor = Color.white;

    [Header("Economy Per Tick")]
    [Tooltip("Resources this room PRODUCES each tick (positive amounts).")]
    public List<ResourceAmount> produces = new();

    [Tooltip("Resources this room CONSUMES each tick (positive amounts).")]
    public List<ResourceAmount> consumes = new();

    // Convert lists to dictionaries for faster runtime use
    public Dictionary<ResourceType, int> GetProductionDict()
    {
        var dict = new Dictionary<ResourceType, int>();
        foreach (var p in produces)
        {
            if (!dict.ContainsKey(p.type)) dict[p.type] = 0;
            dict[p.type] += Mathf.Max(0, p.amount);
        }
        return dict;
    }

    public Dictionary<ResourceType, int> GetConsumptionDict()
    {
        var dict = new Dictionary<ResourceType, int>();
        foreach (var c in consumes)
        {
            if (!dict.ContainsKey(c.type)) dict[c.type] = 0;
            dict[c.type] += Mathf.Max(0, c.amount);
        }
        return dict;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Initial Resources")]
    [SerializeField] private List<ResourceAmount> initialResources = new();

    private Dictionary<ResourceType, int> _amounts = new();

    public System.Action OnResourcesChanged;

    private void Awake()
    {
        _amounts = new Dictionary<ResourceType, int>();

        foreach (ResourceType t in System.Enum.GetValues(typeof(ResourceType)))
            _amounts[t] = 0;

        foreach (var entry in initialResources)
            _amounts[entry.type] = Mathf.Max(0, entry.amount);

        OnResourcesChanged?.Invoke();
    }

    public int Get(ResourceType type)
    {
        if (_amounts.TryGetValue(type, out int value))
            return value;

        // If missing, auto-create it (so game never crashes)
        _amounts[type] = 0;
        return 0;
    }


    public bool CanAfford(Dictionary<ResourceType, int> cost)
    {
        foreach (var kvp in cost)
        {
            if (kvp.Value <= 0) continue;
            if (_amounts[kvp.Key] < kvp.Value) return false;
        }
        return true;
    }

    public void ApplyDelta(Dictionary<ResourceType, int> delta)
    {
        // delta can be positive or negative
        foreach (var kvp in delta)
        {
            var t = kvp.Key;
            var newValue = _amounts[t] + kvp.Value;
            _amounts[t] = Mathf.Max(0, newValue); // never negative
        }

        OnResourcesChanged?.Invoke();
    }

    public static Dictionary<ResourceType, int> MakeCost(params (ResourceType type, int amount)[] items)
    {
        var dict = new Dictionary<ResourceType, int>();
        foreach (var it in items)
        {
            if (!dict.ContainsKey(it.type)) dict[it.type] = 0;
            dict[it.type] += it.amount;
        }
        return dict;
    }
}

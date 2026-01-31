using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomSimulationSystem : MonoBehaviour
{
    [SerializeField] private TickSystem tickSystem;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private Transform roomsRoot;

    private void OnEnable()
    {
        tickSystem.OnTick += SimulateTick;
    }

    private void OnDisable()
    {
        tickSystem.OnTick -= SimulateTick;
    }

    private void SimulateTick()
    {
        var roomTiles = roomsRoot.GetComponentsInChildren<RoomTile>();
        var rooms = new List<(RoomTile tile, RoomInstance inst)>();

        foreach (var tile in roomTiles)
        {
            var inst = tile.GetComponent<RoomInstance>();
            if (inst == null || inst.definition == null) continue;
            rooms.Add((tile, inst));
        }

        // Deterministic order: by y then x
        var ordered = rooms
            .OrderBy(r => r.tile.cell.y)
            .ThenBy(r => r.tile.cell.x)
            .ToList();

        foreach (var r in ordered)
        {
            var def = r.inst.definition;
            var cost = def.GetConsumptionDict();
            var gain = def.GetProductionDict();

            // If can't afford consumption, room is offline this tick
            if (!resourceManager.CanAfford(cost))
            {
                r.inst.SetOffline(true);
                continue;
            }

            // Pay consumption
            var delta = new Dictionary<ResourceType, int>();
            foreach (var c in cost) delta[c.Key] = delta.GetValueOrDefault(c.Key, 0) - c.Value;

            // Add production
            foreach (var p in gain) delta[p.Key] = delta.GetValueOrDefault(p.Key, 0) + p.Value;

            resourceManager.ApplyDelta(delta);
            r.inst.SetOffline(false);
        }
    }
}

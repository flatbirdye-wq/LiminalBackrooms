using System.Collections.Generic;
using UnityEngine;

public class DiscoveryTracker : MonoBehaviour
{
    public HashSet<string> visitedLevels = new HashSet<string>();
    public HashSet<string> discoveredEntities = new HashSet<string>();
    public HashSet<string> discoveredItems = new HashSet<string>();
    public HashSet<string> discoveredExits = new HashSet<string>();

    public void ReportVisitedLevel(string levelId)
    {
        visitedLevels.Add(levelId);
    }

    public void ReportEncounteredEntity(string entityId)
    {
        discoveredEntities.Add(entityId);
    }

    public void ReportCollectedItem(string itemId)
    {
        discoveredItems.Add(itemId);
    }

    public void ReportDiscoveredExit(string exitId)
    {
        discoveredExits.Add(exitId);
    }

    public DiscoverySaveData GetSaveData()
    {
        return new DiscoverySaveData
        {
            visited = new List<string>(visitedLevels),
            entities = new List<string>(discoveredEntities),
            items = new List<string>(discoveredItems),
            exits = new List<string>(discoveredExits)
        };
    }

    public void ApplySaveData(DiscoverySaveData data)
    {
        visitedLevels = new HashSet<string>(data.visited ?? new List<string>());
        discoveredEntities = new HashSet<string>(data.entities ?? new List<string>());
        discoveredItems = new HashSet<string>(data.items ?? new List<string>());
        discoveredExits = new HashSet<string>(data.exits ?? new List<string>());
    }
}

[System.Serializable]
public class DiscoverySaveData
{
    public List<string> visited;
    public List<string> entities;
    public List<string> items;
    public List<string> exits;
}

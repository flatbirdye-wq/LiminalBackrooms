using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public int maxSlots = 6;
    public string savePrefix = "save_";

    private string SavePathFor(int slot)
    {
        return Path.Combine(Application.persistentDataPath, $"{savePrefix}{slot}.json");
    }

    public bool SaveGame(int slot, GameState state)
    {
        if (slot < 0 || slot >= maxSlots) return false;
        try
        {
            string json = JsonUtility.ToJson(state, true);
            File.WriteAllText(SavePathFor(slot), json);
            Debug.Log($"Saved slot {slot} to {SavePathFor(slot)}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Save failed: " + ex);
            return false;
        }
    }

    public GameState LoadGame(int slot)
    {
        if (slot < 0 || slot >= maxSlots) return null;
        string path = SavePathFor(slot);
        if (!File.Exists(path)) return null;
        try
        {
            string json = File.ReadAllText(path);
            var state = JsonUtility.FromJson<GameState>(json);
            Debug.Log($"Loaded slot {slot} from {path}");
            return state;
        }
        catch (Exception ex)
        {
            Debug.LogError("Load failed: " + ex);
            return null;
        }
    }

    public bool DeleteSlot(int slot)
    {
        string path = SavePathFor(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
            return true;
        }
        return false;
    }

    public List<bool> EnumerateSlots()
    {
        var list = new List<bool>();
        for (int i = 0; i < maxSlots; i++)
        {
            list.Add(File.Exists(SavePathFor(i)));
        }
        return list;
    }
}

[Serializable]
public class GameState
{
    public string levelId;
    public int levelSeed;
    public SerializableVector3 playerPosition;
    public SerializableQuaternion playerRotation;
    public int almondWater;
    public int royalRations;
    public int batteries;
    public int medkits;
    public float sanity;
    public float stamina;
    public DiscoverySaveData discovery;
    public List<EntitySaveData> entities = new List<EntitySaveData>();
}

[Serializable]
public struct SerializableVector3 { public float x,y,z; public SerializableVector3(Vector3 v) { x=v.x;y=v.y;z=v.z;} public Vector3 ToVector3() { return new Vector3(x,y,z);} }
[Serializable]
public struct SerializableQuaternion { public float x,y,z,w; public SerializableQuaternion(Quaternion q) { x=q.x;y=q.y;z=q.z;w=q.w;} public Quaternion ToQuaternion() { return new Quaternion(x,y,z,w;} }

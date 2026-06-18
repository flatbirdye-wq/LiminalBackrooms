using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject roomPrefab;
    public GameObject hallwayPrefab;
    public Light defaultFluorescent;

    [Header("Runtime settings")]
    public int maxRooms = 200;
    public float gridSize = 4f;
    public Material floorMaterial;
    public Material wallMaterial;
    public Material ceilingMaterial;

    private List<Room> rooms = new List<Room>();
    private System.Random prng;

    public int usedSeed { get; private set; }

    [Serializable]
    public class GenerationContext
    {
        public LevelConfig level;
        public int seed;
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        rooms.Clear();
    }

    public IEnumerator GenerateLevel(LevelConfig config, int seed)
    {
        Clear();
        usedSeed = seed;
        prng = new System.Random(seed);

        int roomCount = Mathf.Clamp(prng.Next(30, 80), 20, maxRooms);
        Vector3 origin = Vector3.zero;
        Room start = CreateRoom(origin, prng.Next(config.roomMinSize, config.roomMaxSize + 1));
        rooms.Add(start);

        int attempts = 0;
        int index = 0;
        while (rooms.Count < roomCount && attempts < roomCount * 10)
        {
            Room baseRoom = rooms[index % rooms.Count];
            Vector3 dir = RandomDirection(prng);
            int roomSize = prng.Next(config.roomMinSize, config.roomMaxSize + 1);
            Vector3 pos = baseRoom.position + dir * (baseRoom.radius + roomSize + Random.Range(1f, 3f) * gridSize);

            if (!IsOverlapping(pos, roomSize))
            {
                Room newRoom = CreateRoom(pos, roomSize);
                rooms.Add(newRoom);
                CreateHallway(baseRoom, newRoom);
                if (prng.NextDouble() < config.deadEndChance)
                {
                }
                else
                {
                    if (prng.NextDouble() < 0.25)
                    {
                        Vector3 mid = Vector3.Lerp(baseRoom.position, newRoom.position, 0.5f);
                        if (!IsOverlapping(mid, Mathf.Max(baseRoom.radius, newRoom.radius)))
                        {
                            var midRoom = CreateRoom(mid, Mathf.Max(3, (int)(Mathf.Lerp(baseRoom.radius, newRoom.radius, 0.5f))));
                            rooms.Add(midRoom);
                            CreateHallway(baseRoom, midRoom);
                            CreateHallway(midRoom, newRoom);
                        }
                    }
                }
            }

            index++;
            attempts++;
            if (attempts % 8 == 0)
                yield return null;
        }

        for (int i = 0; i < Mathf.Min(5, rooms.Count/10); i++)
        {
            if (prng.NextDouble() < config.anomalyRarity * 10)
            {
                Room a = rooms[prng.Next(0, rooms.Count)];
                Room b = rooms[prng.Next(0, rooms.Count)];
                if (a != b)
                    CreateTeleportLink(a, b);
            }
        }

        foreach (var r in rooms)
        {
            YieldDecorate(r, config);
            yield return null;
        }

        yield return null;
    }

    private void YieldDecorate(Room r, LevelConfig config)
    {
        GameObject go = Instantiate(roomPrefab, r.position, Quaternion.identity, transform);
        go.transform.localScale = new Vector3(r.radius, 3f, r.radius);
        var rend = go.GetComponentInChildren<MeshRenderer>();
        if (rend != null)
        {
            rend.sharedMaterial = floorMaterial;
        }

        GameObject lightObj = new GameObject("Fluorescent");
        lightObj.transform.parent = go.transform;
        lightObj.transform.localPosition = new Vector3(0, 2.6f, 0);
        Light l = lightObj.AddComponent<Light>();
        l.type = LightType.Point;
        l.range = r.radius * 2f;
        l.intensity = 1.6f;
        l.color = new Color(1f, 0.95f, 0.75f);
        var flick = lightObj.AddComponent<FlickerLight>();
        flick.Setup(0.02f, 0.25f, Random.Range(0.01f, 1.0f));
    }

    private void CreateHallway(Room a, Room b)
    {
        Vector3 midpoint = (a.position + b.position) * 0.5f;
        Vector3 dir = (b.position - a.position).normalized;
        float dist = Vector3.Distance(a.position, b.position);

        GameObject hall = Instantiate(hallwayPrefab, midpoint, Quaternion.LookRotation(dir), transform);
        hall.transform.localScale = new Vector3(2f, 2.5f, dist);
    }

    private void CreateTeleportLink(Room a, Room b)
    {
        GameObject portal = new GameObject("TeleportLink");
        portal.transform.parent = transform;
        portal.transform.position = (a.position + b.position) / 2f;
        var t = portal.AddComponent<BoxCollider>();
        t.isTrigger = true;
        t.size = new Vector3(2f, 2f, 2f);
        var link = portal.AddComponent<TeleportLink>();
        link.targetRoomPosition = b.position + Vector3.up*1.5f;
        link.sourceRoomPosition = a.position + Vector3.up*1.5f;
        link.debugLabel = $"{a.id}->{b.id}";
    }

    private Room CreateRoom(Vector3 pos, int size)
    {
        Room r = new Room();
        r.id = Guid.NewGuid().ToString();
        r.position = pos;
        r.radius = size;
        return r;
    }

    private bool IsOverlapping(Vector3 pos, float size)
    {
        foreach (var r in rooms)
        {
            if (Vector3.Distance(pos, r.position) < (r.radius + size) * 0.8f)
                return true;
        }
        return false;
    }

    private Vector3 RandomDirection(System.Random rng)
    {
        int dir = rng.Next(0, 4);
        switch (dir)
        {
            case 0: return Vector3.forward * gridSize;
            case 1: return Vector3.back * gridSize;
            case 2: return Vector3.left * gridSize;
            default: return Vector3.right * gridSize;
        }
    }
}

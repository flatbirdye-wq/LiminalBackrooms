using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EntityBase : MonoBehaviour
{
    public bool IsHostile = true;
    public float visionAngle = 90f;
    public float visionRange = 18f;
    public float hearingRange = 12f;
    public Transform[] patrolPoints;
    public float memoryTime = 6f;

    protected NavMeshAgent agent;
    protected Transform player;
    protected float lastSeenTime = -999f;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            agent.destination = patrolPoints[0].position;
        }
    }

    protected virtual void Update()
    {
        SenseEnvironment();
        Behavior();
    }

    protected virtual void SenseEnvironment()
    {
        if (player == null) return;
        Vector3 dir = (player.position - transform.position);
        float dist = dir.magnitude;
        if (dist <= hearingRange)
        {
            if (dist <= hearingRange)
            {
                lastSeenTime = Time.time;
                OnHearPlayer();
            }
        }

        if (dist <= visionRange)
        {
            float angle = Vector3.Angle(transform.forward, dir.normalized);
            if (angle <= visionAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir.normalized, out hit, visionRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        lastSeenTime = Time.time;
                        OnSeePlayer();
                    }
                }
            }
        }
    }

    protected virtual void Behavior()
    {
        if (Time.time - lastSeenTime < memoryTime)
        {
            if (player != null)
            {
                agent.SetDestination(player.position);
            }
        }
        else
        {
            if (patrolPoints != null && patrolPoints.Length > 0)
            {
                if (agent.remainingDistance < 1f)
                {
                    int idx = Random.Range(0, patrolPoints.Length);
                    agent.SetDestination(patrolPoints[idx].position);
                }
            }
        }
    }

    protected virtual void OnSeePlayer()
    {
        IsHostile = true;
    }

    protected virtual void OnHearPlayer()
    {
    }

    public virtual EntitySaveData GetSaveData()
    {
        return new EntitySaveData
        {
            instanceId = gameObject.GetInstanceID(),
            position = transform.position,
            rotation = transform.rotation,
            isActive = gameObject.activeSelf
        };
    }

    public virtual void ApplySaveData(EntitySaveData d)
    {
        transform.position = d.position;
        transform.rotation = d.rotation;
        gameObject.SetActive(d.isActive);
    }
}

[System.Serializable]
public class EntitySaveData
{
    public int instanceId;
    public Vector3 position;
    public Quaternion rotation;
    public bool isActive;
}

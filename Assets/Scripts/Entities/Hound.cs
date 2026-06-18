using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EntityBase), typeof(NavMeshAgent))]
public class Hound : MonoBehaviour
{
    private EntityBase baseAI;
    private NavMeshAgent agent;
    public float sprintMultiplier = 1.6f;

    void Awake()
    {
        baseAI = GetComponent<EntityBase>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (baseAI.IsHostile && agent.hasPath)
        {
            agent.speed = 4.5f * sprintMultiplier;
        }
        else
        {
            agent.speed = 3.0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var sanity = collision.collider.GetComponent<SanitySystem>();
            if (sanity != null) sanity.ChangeSanity(-25f, "HoundAttack");
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(EntityBase))]
public class Smiler : MonoBehaviour
{
    public Light eyeGlow;
    private EntityBase baseAI;

    void Awake()
    {
        baseAI = GetComponent<EntityBase>();
    }

    void Update()
    {
        float ambient = RenderSettings.ambientIntensity;
        if (ambient > 0.4f)
        {
            baseAI.IsHostile = false;
            if (eyeGlow) eyeGlow.intensity = Mathf.Lerp(eyeGlow.intensity, 0.1f, Time.deltaTime * 2f);
        }
        else
        {
            baseAI.IsHostile = true;
            if (eyeGlow) eyeGlow.intensity = Mathf.Lerp(eyeGlow.intensity, 3f, Time.deltaTime * 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var sanity = other.GetComponent<SanitySystem>();
            if (sanity != null) sanity.ChangeSanity(-40f, "SmilerEncounter");
        }
    }
}

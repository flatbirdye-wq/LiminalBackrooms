using UnityEngine;
using System;

public class SanitySystem : MonoBehaviour
{
    public float MaxSanity = 100f;
    public float Current { get; private set; }

    public event Action<float, float> OnSanityChanged;
    public event System.Action OnLowSanity;

    public float lowSanityThreshold = 30f;
    private bool lowTriggered = false;

    private void Awake()
    {
        Current = MaxSanity;
    }

    public void ChangeSanity(float delta, string reason = "")
    {
        Current = Mathf.Clamp(Current + delta, 0f, MaxSanity);
        OnSanityChanged?.Invoke(Current, MaxSanity);
        if (!lowTriggered && Current <= lowSanityThreshold)
        {
            lowTriggered = true;
            OnLowSanity?.Invoke();
        }
        else if (lowTriggered && Current > lowSanityThreshold)
        {
            lowTriggered = false;
        }
    }
}

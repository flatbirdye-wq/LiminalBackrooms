using UnityEngine;
using System;

[System.Serializable]
public class StaminaSystem : MonoBehaviour
{
    public float Max = 100f;
    public float Current { get; private set; }

    public event Action<float, float> OnStaminaChanged;

    private void Awake()
    {
        Current = Max;
    }

    public void Drain(float amount)
    {
        Current = Mathf.Max(0f, Current - amount);
        OnStaminaChanged?.Invoke(Current, Max);
    }

    public void Recover(float amount)
    {
        Current = Mathf.Min(Max, Current + amount);
        OnStaminaChanged?.Invoke(Current, Max);
    }
}

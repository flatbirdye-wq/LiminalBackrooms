using UnityEngine;

public class PostProcessController : MonoBehaviour
{
    public Material vhsMaterial;

    public void SetDistortion(float amount)
    {
        if (vhsMaterial) vhsMaterial.SetFloat("_DistortionAmount", amount);
    }

    public void SetGrain(float amount)
    {
        if (vhsMaterial) vhsMaterial.SetFloat("_GrainAmount", amount);
    }

    public void SetChromatic(float amount)
    {
        if (vhsMaterial) vhsMaterial.SetFloat("_ChromaticAmount", amount);
    }
}

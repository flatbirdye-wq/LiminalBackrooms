using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int almondWater = 0;
    public int royalRations = 0;
    public int batteries = 0;
    public int medkits = 0;

    public void UseAlmondWater()
    {
        if (almondWater > 0)
        {
            almondWater--;
            var sanity = GetComponent<SanitySystem>();
            var player = GetComponent<PlayerController>();
            if (sanity != null) sanity.ChangeSanity(+12f, "AlmondWater");
        }
    }

    public void UseRation()
    {
        if (royalRations > 0)
        {
            royalRations--;
            var stamina = GetComponent<StaminaSystem>();
            if (stamina != null) stamina.Recover(18f);
        }
    }

    public void UseBattery()
    {
        if (batteries > 0)
        {
            batteries--;
        }
    }

    public void UseMedkit()
    {
        if (medkits > 0)
        {
            medkits--;
            var sanity = GetComponent<SanitySystem>();
            if (sanity != null) sanity.ChangeSanity(+8f, "Medkit");
        }
    }
}

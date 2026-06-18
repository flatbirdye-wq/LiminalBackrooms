using UnityEngine;

public class TeleportLink : MonoBehaviour
{
    public Vector3 targetRoomPosition;
    public Vector3 sourceRoomPosition;
    public string debugLabel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var noclip = FindObjectOfType<NoclipTransition>();
            if (noclip != null)
            {
                noclip.StartTransition(other.gameObject.transform, targetRoomPosition);
            }
            else
            {
                other.transform.position = targetRoomPosition;
            }

            var tracker = FindObjectOfType<DiscoveryTracker>();
            if (tracker != null)
            {
                tracker.ReportDiscoveredExit(debugLabel);
            }
        }
    }
}

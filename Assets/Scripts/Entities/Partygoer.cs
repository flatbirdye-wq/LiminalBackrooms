using UnityEngine;

[RequireComponent(typeof(EntityBase))]
public class Partygoer : MonoBehaviour
{
    public AudioClip friendlyChatter;
    private bool lured = false;

    void Update()
    {
        if (!lured)
        {
            if (Random.value < 0.001f)
            {
                AudioSource.PlayClipAtPoint(friendlyChatter, transform.position, 0.6f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !lured)
        {
            StartCoroutine(ConsumePlayer(other.gameObject));
        }
    }

    private System.Collections.IEnumerator ConsumePlayer(GameObject player)
    {
        lured = true;
        var sanity = player.GetComponent<SanitySystem>();
        if (sanity != null) sanity.ChangeSanity(-60f, "PartygoerLure");
        yield return new WaitForSeconds(2.5f);
        var gm = FindObjectOfType<GameManager>();
        if (gm != null) gm.OnPlayerConsumed();
    }
}

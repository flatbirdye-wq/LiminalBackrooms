using System.Collections;
using UnityEngine;

public class NoclipTransition : MonoBehaviour
{
    public float distortionDuration = 2.4f;
    public AnimationCurve distortionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AudioSource sfxSource;
    public AudioClip realityBreakClip;
    public CanvasGroup screenVignette;
    public PostProcessController postController;

    public void StartTransition(Transform player, Vector3 dest)
    {
        StartCoroutine(DoTransition(player, dest));
    }

    private IEnumerator DoTransition(Transform player, Vector3 dest)
    {
        if (sfxSource && realityBreakClip) sfxSource.PlayOneShot(realityBreakClip);

        float elapsed = 0f;
        while (elapsed < distortionDuration)
        {
            float t = distortionCurve.Evaluate(elapsed / distortionDuration);
            if (screenVignette) screenVignette.alpha = Mathf.Lerp(0f, 1f, t);
            if (postController != null) postController.SetDistortion(t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.position = dest;

        yield return new WaitForSeconds(0.15f);

        elapsed = 0f;
        while (elapsed < distortionDuration)
        {
            float t = 1f - distortionCurve.Evaluate(elapsed / distortionDuration);
            if (screenVignette) screenVignette.alpha = Mathf.Lerp(0f, 1f, t);
            if (postController != null) postController.SetDistortion(t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (screenVignette) screenVignette.alpha = 0f;
        if (postController != null) postController.SetDistortion(0f);
    }
}

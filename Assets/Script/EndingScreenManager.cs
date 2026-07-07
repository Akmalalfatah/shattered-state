using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingScreenManager : MonoBehaviour
{
    [Header("UI Canvas Groups")]
    public CanvasGroup lightGroup;
    public CanvasGroup thankYouTextGroup; // "Terima kasih..."
    public CanvasGroup normalTitleGroup;  // Judul mulus
    public CanvasGroup crackedTitleGroup; // Judul retak
    public CanvasGroup buttonGroup;       // Tombol Return

    [Header("Transforms")]
    public Transform titleTransform;      // Untuk efek getar

    [Header("Pengaturan")]
    public string sceneName = "Shattered State";

    void Start()
    {
        // 1. Memunculkan kursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 2. Inisialisasi CanvasGroup
        CanvasGroup[] groups = { lightGroup, normalTitleGroup, crackedTitleGroup, thankYouTextGroup, buttonGroup };
        foreach (var cg in groups)
        {
            if (cg != null)
            {
                cg.alpha = 0f;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
        }

        // 3. Menjalankan sequence
        StartCoroutine(EndingSequence());
    }

    IEnumerator EndingSequence()
    {
        yield return new WaitForSeconds(1f);

        if (lightGroup != null) yield return StartCoroutine(FadeCanvasGroup(lightGroup, 0f, 1f, 2f));

        if (thankYouTextGroup != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(thankYouTextGroup, 0f, 1f, 1f));
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(FadeCanvasGroup(thankYouTextGroup, 1f, 0f, 1f));
        }

        if (normalTitleGroup != null) yield return StartCoroutine(FadeCanvasGroup(normalTitleGroup, 0f, 1f, 1.5f));
        yield return new WaitForSeconds(1f);

        if (titleTransform != null) yield return StartCoroutine(CrackEffect(titleTransform, 0.5f));

        if (normalTitleGroup != null && crackedTitleGroup != null)
        {
            StartCoroutine(FadeCanvasGroup(normalTitleGroup, 1f, 0f, 1f));
            yield return StartCoroutine(FadeCanvasGroup(crackedTitleGroup, 0f, 1f, 1f));
        }

        if (buttonGroup != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(buttonGroup, 0f, 1f, 1f));
            buttonGroup.interactable = true;
            buttonGroup.blocksRaycasts = true;
        }
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cg.alpha = end;
    }

    IEnumerator CrackEffect(Transform tf, float duration)
    {
        Vector3 orig = tf.localPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            tf.localPosition = orig + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        tf.localPosition = orig;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(sceneName);
    }
}
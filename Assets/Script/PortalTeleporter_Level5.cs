using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// PortalTeleporter_Level5.cs
/// Modifikasi PortalTeleporter khusus Level 5:
/// - Saat player masuk portal → dialog cutscene muncul dulu
/// - Setelah dialog selesai → pindah ke scene berikutnya
///
/// SETUP INSPECTOR:
/// - targetSceneName  : nama scene tujuan (misal "Credits" atau "MainMenu")
/// - dialogueManager  : drag DialogueManager dari scene
/// - dialogueLines    : isi dialog cutscene akhir
/// - characterName    : nama karakter yang berbicara
/// </summary>
public class PortalTeleporter_Level5 : MonoBehaviour
{
    [Header("Settings")]
    public string targetSceneName = "MainMenu";

    [Header("Dialogue")]
    [Tooltip("Drag DialogueManager dari scene ke sini")]
    public DialogueManager dialogueManager;

    [Tooltip("Isi dialog cutscene akhir Level 5")]
    [TextArea(3, 5)]
    public string[] endingDialogueLines;

    [Tooltip("Nama karakter yang berbicara")]
    public string characterName = "Lummen";

    [Tooltip("Jeda (detik) setelah dialog selesai sebelum pindah scene")]
    public float delayAfterDialogue = 1f;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(EndSequence());
        }
    }

    private IEnumerator EndSequence()
    {
        // 1. Hentikan pergerakan waktu (pause game)
        Time.timeScale = 0f;

        // 2. Setup dan mulai dialog
        if (dialogueManager != null && endingDialogueLines.Length > 0)
        {
            // Override dialogue lines dan character name
            dialogueManager.characterName = characterName;
            dialogueManager.dialogueLines = endingDialogueLines;
            dialogueManager.StartDialogue();

            // 3. Tunggu sampai dialog selesai (panel tertutup)
            yield return new WaitUntil(() =>
                dialogueManager.gameObject.activeSelf &&
                dialogueManager.dialoguePanel != null &&
                !dialogueManager.dialoguePanel.activeSelf
            );
        }

        // 4. Resume waktu
        Time.timeScale = 1f;

        // 5. Jeda sebentar lalu pindah scene
        yield return new WaitForSeconds(delayAfterDialogue);

        Debug.Log("[Portal Level 5] Dialog selesai. Pindah ke: " + targetSceneName);
        SceneManager.LoadScene(targetSceneName);
    }
}
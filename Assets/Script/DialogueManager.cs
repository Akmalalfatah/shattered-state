using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;

    [Header("Dialogue Content")]
    public string characterName = "Lummen";
    [TextArea(3, 5)]
    public string[] dialogueLines;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private float lastClickTime = 0f; // Tambahkan variabel ini di atas
    private Coroutine typingCoroutine; // Simpan referensi coroutine
    public float typeSpeed = 0.03f;

    void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        nameText.text = characterName;
        currentLineIndex = 0;

        // Mulai dan simpan referensi coroutine
        typingCoroutine = StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }

        isTyping = false;
    }

    public void ClickNextButton()
    {
        // Tambahkan pengaman: Abaikan klik jika dilakukan kurang dari 0.2 detik setelah klik terakhir
        if (Time.unscaledTime - lastClickTime < 0.2f) return;
        lastClickTime = Time.unscaledTime;

        if (isTyping)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            dialogueText.text = dialogueLines[currentLineIndex];
            isTyping = false;
        }
        else
        {
            currentLineIndex++;
            if (currentLineIndex < dialogueLines.Length)
            {
                typingCoroutine = StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
            }
            else
            {
                EndDialogue();
            }
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Dialog Selesai!");
    }
}
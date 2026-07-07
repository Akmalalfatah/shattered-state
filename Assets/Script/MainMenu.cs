using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour // Kelas dimulai di sini
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject loadingPanel;

    [Header("Loading Settings")]
    public Slider loadingSlider;
    public float loadingDuration = 3f;

    [Header("Dependencies")]
    public DialogueManager dialogueManager; // Pindahkan ke dalam kelas

    void Start()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (loadingPanel != null) loadingPanel.SetActive(false);

        Time.timeScale = 0f; // Menahan game agar tidak berjalan di background
    }

    public void PlayGame()
    {
        StartCoroutine(StartLoadingTransition());
    }

    IEnumerator StartLoadingTransition()
    {
        if (loadingSlider != null) loadingSlider.value = 0f;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (loadingPanel != null) loadingPanel.SetActive(true);

        float elapsedTime = 0f;
        while (elapsedTime < loadingDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            if (loadingSlider != null)
            {
                loadingSlider.value = Mathf.Clamp01(elapsedTime / loadingDuration);
            }

            yield return null;
        }

        if (loadingSlider != null) loadingSlider.value = 1f;
        yield return new WaitForSecondsRealtime(0.2f);

        if (loadingPanel != null) loadingPanel.SetActive(false);

        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue();
        }
        else
        {
            Time.timeScale = 1f;
        }
    } // Penutup IEnumerator

    public void QuitGame()
    {
        Debug.Log("Keluar dari Game...");
        Application.Quit();
    }
} // Penutup class MainMenu
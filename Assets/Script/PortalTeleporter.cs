using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ada untuk pindah level

public class PortalTeleporter : MonoBehaviour
{
    [Header("Settings")]
    public string targetSceneName = "Level1"; // Pastikan nama ini sama dengan nama scene Level 1 kamu

    // PERBAIKAN: Menggunakan Collider (3D), bukan Collider2D
    private void OnTriggerEnter(Collider collision) 
    {
        // Mengecek apakah yang menyentuh portal adalah objek dengan Tag "Player"
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player masuk ke portal! Pindah ke: " + targetSceneName);
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
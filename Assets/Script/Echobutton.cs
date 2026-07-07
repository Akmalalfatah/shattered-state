using UnityEngine;
using System.Collections;

/// <summary>
/// EchoButton.cs — Level 5
/// Dipukul pemain (tombol E) → platform Echo muncul selama [duration] detik lalu menghilang.
/// Pukul lagi saat aktif → timer direset (perpanjang durasi).
///
/// SETUP INSPECTOR:
/// - echoPlatforms  : array platform yang muncul/hilang
/// - duration       : berapa detik platform bertahan (default 5)
/// - cooldown       : jeda setelah hilang sebelum bisa aktif lagi (default 2)
/// - activeVisual   : (opsional) visual tombol saat aktif
/// - inactiveVisual : (opsional) visual tombol saat tidak aktif / cooldown
/// - Layer objek ini harus sama dengan puzzleLayer di PlayerMovement
/// </summary>
public class EchoButton : MonoBehaviour
{
    [Header("Echo Platform")]
    [Tooltip("Platform yang muncul saat tombol ini dipukul")]
    public GameObject[] echoPlatforms;

    [Tooltip("Berapa detik platform bertahan")]
    public float duration = 5f;

    [Tooltip("Jeda setelah platform hilang sebelum bisa diaktifkan lagi")]
    public float cooldown = 2f;

    [Header("Visual Tombol (Opsional)")]
    public GameObject activeVisual;
    public GameObject inactiveVisual;

    [Header("Audio (Opsional)")]
    public AudioClip activateSound;
    public AudioClip deactivateSound;
    private AudioSource audioSource;

    private bool isActive = false;
    private bool isOnCooldown = false;
    private Coroutine timerCoroutine;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetPlatforms(false);
        SetVisual(false);
    }

    // Dipanggil oleh PlayerMovement saat pemain menekan E ke arah tombol ini
    public void Interact()
    {
        if (isOnCooldown)
        {
            Debug.Log("[EchoButton] " + gameObject.name + " sedang cooldown.");
            return;
        }

        // Jika sudah aktif, reset timer (perpanjang waktu)
        if (isActive)
        {
            Debug.Log("[EchoButton] " + gameObject.name + " timer direset!");
            if (timerCoroutine != null) StopCoroutine(timerCoroutine);
            timerCoroutine = StartCoroutine(EchoTimer());
            return;
        }

        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(EchoTimer());
    }

    private IEnumerator EchoTimer()
    {
        // Aktifkan
        isActive = true;
        SetPlatforms(true);
        SetVisual(true);
        PlaySound(activateSound);
        Debug.Log("[EchoButton] " + gameObject.name + " aktif. Platform muncul selama " + duration + "s.");

        yield return new WaitForSeconds(duration);

        // Matikan platform
        isActive = false;
        SetPlatforms(false);
        SetVisual(false);
        PlaySound(deactivateSound);
        Debug.Log("[EchoButton] " + gameObject.name + " platform hilang. Cooldown " + cooldown + "s.");

        // Cooldown
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
        Debug.Log("[EchoButton] " + gameObject.name + " siap diaktifkan kembali.");
    }

    private void SetPlatforms(bool active)
    {
        foreach (GameObject p in echoPlatforms)
            if (p != null) p.SetActive(active);
    }

    private void SetVisual(bool active)
    {
        if (activeVisual != null) activeVisual.SetActive(active);
        if (inactiveVisual != null) inactiveVisual.SetActive(!active);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}
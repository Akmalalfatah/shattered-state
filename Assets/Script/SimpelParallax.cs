using UnityEngine;

public class SimpelParallax : MonoBehaviour
{
    [Tooltip("Kecepatan gerak (0 = diam, 1 = ikut player, >1 = lebih cepat)")]
    public float parallaxFactor; // Sempurna untuk jurang parkour

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    void Start()
    {
        // Mencari kamera utama secara otomatis
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        // Menghitung delta pergerakan kamera (untuk game side-scroller)
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // Geser posisi objek background berdasarkan faktor
        transform.position += new Vector3(deltaMovement.x * parallaxFactor, 0f, 0f);

        lastCameraPosition = cameraTransform.position;
    }
}
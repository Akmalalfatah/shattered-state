using UnityEngine;

public class TiledParallax : MonoBehaviour
{
    public Transform mainCamera;
    public float parallaxMultiplier;
    private float startPositionX;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main.transform;

        // Simpan titik awal
        startPositionX = transform.position.x;
    }

    void LateUpdate()
    {
        // Hanya menggeser posisi dengan mulus, TANPA matematika kloning yang bikin error!
        float distance = mainCamera.position.x * parallaxMultiplier;
        transform.position = new Vector3(startPositionX + distance, transform.position.y, transform.position.z);
    }
}
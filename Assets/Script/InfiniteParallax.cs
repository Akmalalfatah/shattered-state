using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InfiniteParallax : MonoBehaviour
{
    [Header("Pengaturan Kamera")]
    public Transform mainCamera;

    [Header("Kecepatan Parallax")]
    public float parallaxEffectMultiplier;

    private float startPositionX;
    private float spriteLength;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }

        startPositionX = transform.position.x;

        // Mengambil ukuran mutlak gambar di dunia (mengabaikan Scale Inspector)
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;

        CreateClone(spriteLength);
        CreateClone(-spriteLength);
    }

    void CreateClone(float worldOffset)
    {
        GameObject clone = new GameObject(gameObject.name + "_AutoClone");

        // --- FIX ATAP TERPOTONG ---
        // Atur posisi kloningan di World Space TERLEBIH DAHULU agar jaraknya 100% presisi.
        // Kita juga menambahkan 0.01f di sumbu Z untuk mencegah Z-Fighting.
        clone.transform.position = transform.position + new Vector3(worldOffset, 0, 0.01f);

        // SETELAH posisinya pas di dunia, baru kita masukkan sebagai Child (Anak)
        clone.transform.parent = transform;

        // Copy visual gambar
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        SpriteRenderer cloneSR = clone.AddComponent<SpriteRenderer>();
        cloneSR.sprite = sr.sprite;
        cloneSR.color = sr.color;
        cloneSR.material = sr.material;

        // --- FIX KEDIPAN ---
        // Pastikan kloningan selalu digambar tepat satu lapisan di BELAKANG gambar asli
        cloneSR.sortingLayerID = sr.sortingLayerID;
        cloneSR.sortingOrder = sr.sortingOrder - 1;
    }

    void LateUpdate()
    {
        float temp = (mainCamera.position.x * (1 - parallaxEffectMultiplier));
        float distance = (mainCamera.position.x * parallaxEffectMultiplier);

        transform.position = new Vector3(startPositionX + distance, transform.position.y, transform.position.z);

        if (temp > startPositionX + spriteLength)
        {
            startPositionX += spriteLength;
        }
        else if (temp < startPositionX - spriteLength)
        {
            startPositionX -= spriteLength;
        }
    }
}
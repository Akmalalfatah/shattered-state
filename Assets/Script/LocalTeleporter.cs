using UnityEngine;

public class LocalTeleporter : MonoBehaviour
{
    [Header("Pengaturan Teleport")]
    [Tooltip("Seret objek kosong (Spawn Point) tujuan ke sini")]
    public Transform targetTujuan;

    private void OnTriggerEnter(Collider other)
    {
        // Memastikan hanya objek dengan Tag "Player" yang bisa teleportasi
        if (other.CompareTag("Player"))
        {
            if (targetTujuan != null)
            {
                // Pindahkan posisi Player ke posisi target tujuan
                other.transform.position = targetTujuan.position;

                // Menghentikan gaya gerak/jatuh Player agar tidak meluncur saat keluar pintu
                // Karena kamu menggunakan Unity 6, kita gunakan linearVelocity
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                }

                Debug.Log("Player berhasil diteleportasi ke: " + targetTujuan.name);
            }
            else
            {
                Debug.LogWarning("Target Tujuan belum dimasukkan di Inspector!");
            }
        }
    }
}
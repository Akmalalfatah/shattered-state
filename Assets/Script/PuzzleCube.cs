using UnityEngine;

public class PuzzleCube : MonoBehaviour
{
    [Header("Pengaturan Level Kubus")]
    public int currentLevel = 1;
    [Tooltip("Jumlah warna/tahap yang dimiliki kubus ini (Contoh: 3 untuk Level 3, 5 untuk Level 4)")]
    public int maxLevel = 5;

    [Header("Mekanik Baru Level 4")]
    [Tooltip("Centang ini HANYA untuk Box yang statis/tidak bisa diubah")]
    public bool isFixedCube = false;

    [Tooltip("Masukkan Box Pengunci. Jika Box Pengunci berada di level target, Box ini akan membeku.")]
    public PuzzleCube lockingCubes;
    [Tooltip("Level berapa yang memicu Box Pengunci aktif?")]
    public int activationLevel = 3;

    [Header("Visual Level")]
    [Tooltip("Masukkan semua objek visual warna di sini sesuai urutan level")]
    public GameObject[] levelVisuals;

    [Header("Hubungan Puzzle")]
    public PuzzleCube[] linkedCubes;

    private BoxPuzzleManager manager;
    private bool isSolved = false;

    void Start()
    {
        manager = FindAnyObjectByType<BoxPuzzleManager>();
        UpdateVisual();
    }

    // Dipanggil dari luar (misal EchoButton) untuk force update visual
    public void ForceUpdateVisual() => UpdateVisual();

    public void Interact()
    {
        if (isSolved) return;

        // 1. Cek Apakah Statis
        if (isFixedCube)
        {
            Debug.Log(gameObject.name + " adalah Box Statis! Warnanya tidak bisa diubah.");
            return;
        }

        // 2. Cek Apakah Terkunci
        if (lockingCubes != null && lockingCubes.currentLevel == activationLevel)
        {
            Debug.Log(gameObject.name + " sedang TERKUNCI oleh " + lockingCubes.gameObject.name + "!");
            return;
        }

        // 3. Ubah level kubus ini
        AdvanceLevel();

        // 4. Ubah level kubus yang terhubung (Linked Cubes)
        foreach (PuzzleCube cube in linkedCubes)
        {
            if (cube != null)
            {
                // Tetangga harus mematuhi aturan: tidak bisa diubah jika Fixed atau sedang Terkunci
                if (!cube.isFixedCube && !(cube.lockingCubes != null && cube.lockingCubes.currentLevel == cube.activationLevel))
                {
                    cube.AdvanceLevel();
                }
            }
        }

        // 5. Lapor ke Manager untuk cek kondisi menang
        if (manager != null)
        {
            manager.CheckWinCondition();
        }
    }

    public void AdvanceLevel()
    {
        currentLevel++;
        if (currentLevel > maxLevel)
        {
            currentLevel = 1;
        }
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        // Validasi agar tidak error jika jumlah visual tidak sesuai maxLevel
        if (levelVisuals.Length >= currentLevel)
        {
            for (int i = 0; i < levelVisuals.Length; i++)
            {
                levelVisuals[i].SetActive(i == (currentLevel - 1));
            }
        }
        else
        {
            Debug.LogWarning("Jumlah objek di 'Level Visuals' kurang dari 'Max Level' pada objek " + gameObject.name);
        }
    }

    public void LockCube()
    {
        isSolved = true;
    }
}
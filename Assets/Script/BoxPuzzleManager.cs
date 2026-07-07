using UnityEngine;

public class BoxPuzzleManager : MonoBehaviour
{
    [Header("Target Puzzle")]
    [Tooltip("Jembatan yang akan MUNCUL saat puzzle selesai")]
    public GameObject targetBridge;

    [Header("Daftar Kubus")]
    [Tooltip("Masukkan SEMUA kubus puzzle ke sini")]
    public PuzzleCube[] allCubes;

    [Header("Mode Level 5 (Opsional)")]
    [Tooltip("Aktifkan ini untuk Level 5. Semua kubus harus menyamai level Fixed Cube.")]
    public bool useFixedCubeTarget = false;
    [Tooltip("(Level 5) Fixed Cube yang menjadi target absolut. Isi jika useFixedCubeTarget = true.")]
    public PuzzleCube fixedCube;

    [Header("Win Effect (Opsional)")]
    public GameObject winEffect;

    void Start()
    {
        if (targetBridge != null)
            targetBridge.SetActive(false);

        if (winEffect != null)
            winEffect.SetActive(false);
    }

    public void CheckWinCondition()
    {
        if (useFixedCubeTarget && fixedCube != null)
            CheckWinLevel5();
        else
            CheckWinClassic();
    }

    // Mode klasik Level 2-4: semua kubus harus sama satu sama lain
    private void CheckWinClassic()
    {
        int firstCubeLevel = allCubes[0].currentLevel;
        bool isAllSame = true;

        for (int i = 1; i < allCubes.Length; i++)
        {
            if (allCubes[i].currentLevel != firstCubeLevel)
            {
                isAllSame = false;
                break;
            }
        }

        if (isAllSame)
            TriggerWin();
    }

    // Mode Level 5: semua kubus (kecuali fixedCube) harus menyamai level fixedCube
    private void CheckWinLevel5()
    {
        int targetLevel = fixedCube.currentLevel;

        foreach (PuzzleCube cube in allCubes)
        {
            if (cube == fixedCube) continue; // skip fixed cube itu sendiri
            if (cube.currentLevel != targetLevel) return; // belum semua sama
        }

        TriggerWin();
    }

    private void TriggerWin()
    {
        Debug.Log("Puzzle Berhasil! Jembatan Muncul.");

        if (targetBridge != null)
            targetBridge.SetActive(true);

        if (winEffect != null)
            winEffect.SetActive(true);

        foreach (PuzzleCube cube in allCubes)
            cube.LockCube();
    }
}
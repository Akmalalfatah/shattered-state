using System.Collections.Generic;
using UnityEngine;

public class PressurePlate3D : MonoBehaviour
{
    [Header("Bagian yang Bergerak")]
    public Transform pressablePart;
    public float pressDistance = 0.1f;
    public float smoothSpeed = 8f;

    [Header("Target Puzzle")]
    [Tooltip("Pintu yang akan HILANG saat tombol diinjak")]
    public GameObject targetDoor;

    [Tooltip("Platform Parkour yang akan MUNCUL saat tombol diinjak")]
    public GameObject targetPlatform;

    private Vector3 unpressedLocalPos;
    private Vector3 targetLocalPos;
    private List<Collider> objectsOnPlate = new List<Collider>();

    void Start()
    {
        if (pressablePart != null)
        {
            unpressedLocalPos = pressablePart.localPosition;
            targetLocalPos = unpressedLocalPos;
        }

        // Memastikan platform parkour tidak terlihat saat level baru dimulai
        if (targetPlatform != null)
        {
            targetPlatform.SetActive(false);
        }
    }

    void Update()
    {
        for (int i = objectsOnPlate.Count - 1; i >= 0; i--)
        {
            if (objectsOnPlate[i] == null)
            {
                objectsOnPlate.RemoveAt(i);

                if (objectsOnPlate.Count == 0)
                {
                    DeactivatePlate();
                }
            }
        }

        if (pressablePart != null)
        {
            pressablePart.localPosition = Vector3.Lerp(pressablePart.localPosition, targetLocalPos, Time.deltaTime * smoothSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name.Contains("Echo"))
        {
            if (!objectsOnPlate.Contains(other))
            {
                objectsOnPlate.Add(other);

                if (objectsOnPlate.Count == 1)
                {
                    ActivatePlate();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name.Contains("Echo"))
        {
            if (objectsOnPlate.Contains(other))
            {
                objectsOnPlate.Remove(other);

                if (objectsOnPlate.Count == 0)
                {
                    DeactivatePlate();
                }
            }
        }
    }

    void ActivatePlate()
    {
        targetLocalPos = unpressedLocalPos + (Vector3.down * pressDistance);

        if (targetDoor != null) targetDoor.SetActive(false);

        // --- LOGIKA BARU: Munculkan Platform ---
        if (targetPlatform != null) targetPlatform.SetActive(true);
    }

    void DeactivatePlate()
    {
        targetLocalPos = unpressedLocalPos;

        if (targetDoor != null) targetDoor.SetActive(true);

        // --- LOGIKA BARU: Sembunyikan Platform ---
        if (targetPlatform != null) targetPlatform.SetActive(false);
    }
}
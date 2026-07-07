using UnityEngine;

public class EchoShift : MonoBehaviour
{
    [Header("Soul Unbound Settings")]
    public float speedMultiplier = 1.5f;

    [Tooltip("Tombol untuk membatalkan Echo dan TETAP berada di posisi roh saat ini")]
    public KeyCode stayAtCurrentPosKey = KeyCode.E; // Default tombol E

    [Header("Echo/Visual Settings")]
    public GameObject bodyMarkerPrefab;
    public Sprite echoIdlingSprite;
    public RuntimeAnimatorController echoAnimator;

    [SerializeField] private float echoOffsetY = 0.15f;

    private bool isSoulActive = false;
    private Vector3 originalPosition;
    private GameObject leftBehindBody;

    private PlayerMovement playerMovement;
    private float originalSpeed;
    private Collider playerCollider;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            originalSpeed = playerMovement.moveSpeed;
        }

        playerCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (!isSoulActive)
        {
            // Tekan Shift untuk keluar dari tubuh
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ActivateSoul();
            }
        }
        else
        {
            // OPSI 1: Tekan Shift lagi -> Kembali ke posisi tubuh yang ditinggalkan (Echo)
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                SnapBack(true);
            }
            // OPSI 2: Tekan tombol E -> Hancurkan Echo, Player tetap berada di posisi roh saat ini
            else if (Input.GetKeyDown(stayAtCurrentPosKey))
            {
                SnapBack(false);
            }
        }
    }

    void ActivateSoul()
    {
        isSoulActive = true;

        originalPosition = transform.position + (Vector3.up * echoOffsetY);

        if (bodyMarkerPrefab != null)
        {
            leftBehindBody = Instantiate(bodyMarkerPrefab, originalPosition, Quaternion.identity);
            leftBehindBody.transform.localScale = transform.localScale;

            Rigidbody bodyRb = leftBehindBody.GetComponent<Rigidbody>();
            if (bodyRb != null)
            {
                bodyRb.isKinematic = false;
                bodyRb.constraints = RigidbodyConstraints.FreezeRotationX |
                                     RigidbodyConstraints.FreezeRotationY |
                                     RigidbodyConstraints.FreezeRotationZ |
                                     RigidbodyConstraints.FreezePositionZ;
            }

            Collider echoCollider = leftBehindBody.GetComponent<Collider>();
            if (playerCollider != null && echoCollider != null)
            {
                Physics.IgnoreCollision(playerCollider, echoCollider, true);
            }

            SpriteRenderer bodySR = leftBehindBody.GetComponentInChildren<SpriteRenderer>();
            if (bodySR != null)
            {
                if (echoIdlingSprite != null)
                {
                    bodySR.sprite = echoIdlingSprite;
                }

                if (echoAnimator != null)
                {
                    Animator bodyAnim = bodySR.gameObject.GetComponent<Animator>();
                    if (bodyAnim == null)
                    {
                        bodyAnim = bodySR.gameObject.AddComponent<Animator>();
                    }
                    bodyAnim.runtimeAnimatorController = echoAnimator;
                }
            }
        }

        if (playerMovement != null)
        {
            playerMovement.moveSpeed = originalSpeed * speedMultiplier;
        }

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    // --- LOGIKA BARU: bool returnToBody menentukan titik akhir Player ---
    void SnapBack(bool returnToBody)
    {
        isSoulActive = false;

        // Jika memilih Opsi 1 (Kembali ke tubuh)
        if (returnToBody)
        {
            if (leftBehindBody != null)
            {
                transform.position = leftBehindBody.transform.position;
            }
            else
            {
                transform.position = originalPosition;
            }
        }
        // Jika memilih Opsi 2 (returnToBody = false), posisi Player dibiarkan di lokasi roh saat ini

        // Sisanya adalah pembersihan memori dan reset fisika yang sama
        if (leftBehindBody != null)
        {
            Destroy(leftBehindBody);
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }

        if (playerMovement != null)
        {
            playerMovement.moveSpeed = originalSpeed;
        }

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
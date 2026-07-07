using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float pushRayLength = 0.6f;
    public LayerMask wallLayer;

    [Header("Hit / Interaction Puzzle")]
    public float hitRayLength = 1.2f;    // Jarak jangkauan pukulan
    public LayerMask puzzleLayer;       // Pastikan ini diatur ke 'Puzzle' di Inspector
    public KeyCode hitKey = KeyCode.E;  // Tombol untuk memukul

    [Header("Audio Clips")]
    public AudioClip jumpSound;
    public AudioClip walkSound;

    [Header("Components")]
    private Rigidbody rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private AudioSource sfxSource;

    private bool isGrounded;
    private bool isPushing;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        sfxSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 1. LOGIKA GERAKAN
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveVelocity = new Vector3(moveX * moveSpeed, rb.linearVelocity.y, moveZ * moveSpeed);
        rb.linearVelocity = moveVelocity;

        if (moveX > 0f) sprite.flipX = false;
        else if (moveX < 0f) sprite.flipX = true;

        // 2. LOGIKA LOMPAT
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            sfxSource.PlayOneShot(jumpSound);
            isGrounded = false;
        }

        // 3. LOGIKA INTERAKSI PUZZLE
        if (Input.GetKeyDown(hitKey))
        {
            if (anim != null) anim.SetTrigger("Hit");

            Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;

            // Coba SEMUA arah horizontal jika tidak kena
            Vector3[] directions = new Vector3[]
            {
        sprite.flipX ? Vector3.left : Vector3.right,  // Arah hadap utama
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
            };

            bool hitSomething = false;
            foreach (Vector3 dir in directions)
            {
                RaycastHit hit;
                Debug.DrawRay(rayOrigin, dir * hitRayLength, Color.red, 2f);

                if (Physics.Raycast(rayOrigin, dir, out hit, hitRayLength, puzzleLayer))
                {
                    // Cek PuzzleCube
                    PuzzleCube cube = hit.collider.GetComponent<PuzzleCube>();
                    if (cube == null)
                        cube = hit.collider.GetComponentInParent<PuzzleCube>();

                    if (cube != null)
                    {
                        cube.Interact();
                        Debug.Log("Interact berhasil pada: " + hit.collider.name + " | Arah: " + dir);
                        hitSomething = true;
                        break;
                    }

                    // Cek EchoButton (mekanik Level 5)
                    EchoButton echoBtn = hit.collider.GetComponent<EchoButton>();
                    if (echoBtn == null)
                        echoBtn = hit.collider.GetComponentInParent<EchoButton>();

                    if (echoBtn != null)
                    {
                        echoBtn.Interact();
                        Debug.Log("EchoButton diaktifkan: " + hit.collider.name + " | Arah: " + dir);
                        hitSomething = true;
                        break;
                    }
                }
            }

            if (!hitSomething)
                Debug.Log("Tidak ada PuzzleCube yang terkena. Cek Layer dan jarak.");
        }

        CheckForWalls(moveX);
        HandleWalkingSound(moveX, moveZ);

        // 4. ANIMASI
        if (anim != null)
        {
            float currentSpeed = Mathf.Max(Mathf.Abs(moveX), Mathf.Abs(moveZ));
            anim.SetFloat("Speed", currentSpeed);
            anim.SetBool("isJumping", !isGrounded);
            anim.SetBool("isPushing", isPushing);
        }
    }

    private void HandleWalkingSound(float moveX, float moveZ)
    {
        bool isMoving = (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f);
        if (isMoving && isGrounded)
        {
            if (sfxSource.isPlaying && sfxSource.clip != walkSound) return;
            if (sfxSource.clip != walkSound || !sfxSource.isPlaying)
            {
                sfxSource.clip = walkSound;
                sfxSource.loop = true;
                sfxSource.Play();
            }
        }
        else if (sfxSource.clip == walkSound && sfxSource.isPlaying)
        {
            sfxSource.loop = false;
            sfxSource.Stop();
            sfxSource.clip = null;
        }
    }

    private void CheckForWalls(float moveX)
    {
        if (Mathf.Abs(moveX) > 0.1f && isGrounded)
        {
            Vector3 rayDirection = sprite.flipX ? Vector3.left : Vector3.right;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, pushRayLength, wallLayer))
            {
                isPushing = true;
                return;
            }
        }
        isPushing = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (sprite != null)
        {
            Gizmos.color = Color.red;
            Vector3 rayDirection = sprite.flipX ? Vector3.left : Vector3.right;
            Gizmos.DrawRay(transform.position + Vector3.up * 0.5f, rayDirection * hitRayLength);
        }
    }
}
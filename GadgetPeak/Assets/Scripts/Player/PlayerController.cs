using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isSlippery = false;
    private float iceVelX = 0f;
    float heatDuration = 5f;
    Color hotColor = Color.red;
    private float heat = 0f;
    private Color baseColor;
    private SpriteRenderer sr;
    private bool inLavaScene = false;
    private Rigidbody2D rb;
    private bool isGrounded;
    public JoystickMovement joystick;
    public Transform groundCheck; 
    public float groundCheckRadius = 0.2f; 
    public LayerMask groundLayer;
    private bool onStickyWall = false;
    public bool disableHorizontalControl = false;
    private bool autoWalk = false;
    private float autoWalkSpeed = 0f;
    public BoxCollider2D levelBounds;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "ICEGAME") {
            isSlippery = true;
        }

        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
        if (SceneManager.GetActiveScene().name == "LAVAGAME") {
            inLavaScene = true;
        }

    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        Timer.UpdateTimer();

        // Checks if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (autoWalk)
        {
            rb.linearVelocity = new Vector2(autoWalkSpeed, rb.linearVelocity.y);
            return;
        }

        // Handles jump input here
        // float moveX = joystick.Horizontal();
        float moveY = joystick.Vertical();

        // Move(moveX);

        if (moveY > 0.7f) Jump();

        if (onStickyWall)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            return;
        }
        if (inLavaScene)
        {
            float moveX = joystick.Horizontal();
            bool isStill = Mathf.Abs(moveX) < 0.05f;

            if (isStill)
            {
                heat += Time.deltaTime / heatDuration;
            }
            else
            {
                heat -= Time.deltaTime * 2f;
            }

            heat = Mathf.Clamp01(heat);

            sr.color = Color.Lerp(baseColor, hotColor, heat);

            if (heat >= 1f)
            {
                transform.position = RespawnPoint.current.position;
            }
        }
    }

    void FixedUpdate()
    {
        float moveX = joystick.Horizontal();
        Move(moveX);
        //rb.AddForce(Vector2.down * 0.5f, ForceMode2D.Force);
    }


    void LateUpdate()
    {
        Bounds b = levelBounds.bounds;
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, b.min.x, b.max.x);
        pos.y = Mathf.Clamp(pos.y, b.min.y, b.max.y);

        transform.position = pos;
    }
    public void Move(float direction) {
        // If grounded, full control
        if (disableHorizontalControl) return;

        if (isSlippery)
        {
            float targX = direction * moveSpeed;

            if (Mathf.Abs(direction) < 0.01f)
            {
                float friction = 4f;
                iceVelX = Mathf.MoveTowards(iceVelX, 0f, friction * Time.deltaTime);
            }
            else
            {
                float accel = 4f;
                iceVelX = Mathf.MoveTowards(iceVelX, targX, accel * Time.deltaTime);
            }

            Vector2 vel = rb.linearVelocity;
            vel.x = iceVelX;
            rb.linearVelocity = vel;
            return;
        }



        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
            return;
        }

        float targetX = direction * moveSpeed;
        float newX = Mathf.Lerp(rb.linearVelocity.x, targetX, 0.1f);

        //rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
        Vector2 v2 = rb.linearVelocity;
        v2.x = newX;
        rb.linearVelocity = v2;

    }



    public void Jump() {
        bool jumped = false;
        if (onStickyWall) { 
            rb.linearVelocity = Vector2.zero;
        }
        if (isGrounded || onStickyWall) { 
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); 
            jumped = true;
        }
        if (onStickyWall) { 
            ExitStickyWall(); 
        }
        if (jumped) {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.jumpSFX);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground")) {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void StartAutoWalk(float speed)
    {
        autoWalk = true;
        autoWalkSpeed = speed;
        disableHorizontalControl = true;

        if (joystick != null)
            joystick.enabled = false;
    }

    public void StopAutoWalk()
    {
        autoWalk = false;
        disableHorizontalControl = false;

        if (joystick != null)
            joystick.enabled = true;
    }

    public IEnumerator WalkOffScreen(float walkSpeed, float duration)
    {
        StartAutoWalk(walkSpeed);

        yield return new WaitForSeconds(duration);

        StopAutoWalk();
    }


    public void EnterStickyWall()
    {
        onStickyWall = true;
        SoundManager.Instance.PlaySFX(SoundManager.Instance.stickywallSFX);
    }

   public void ExitStickyWall()
    {
        onStickyWall = false;
        StartCoroutine(RestoreGravityNextFrame());  
    }

    private IEnumerator RestoreGravityNextFrame() { 
        yield return null;
        rb.gravityScale = 1f;
    }

    public void LaunchFromJumpPad(float force)
    {
        SoundManager.Instance.sfxSource.PlayOneShot(
            SoundManager.Instance.jumpPadSFX,
            2f
        );

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
        rb.gravityScale = 1f;

        if (onStickyWall)
            ExitStickyWall();
    }
    
}


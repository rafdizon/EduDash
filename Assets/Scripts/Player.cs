using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isGrounded { get; private set; }

    private Rigidbody2D player;
    private Vector2 direction;

    public float jumpForce;

    private int jumpCounter = 0;
    private float jumpCD = 0.2f;
    private float jumpCDTimer = 0f;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        direction = Vector2.zero;
    }

    private void Update()
    {
        if (jumpCD > 0)
        {
            jumpCDTimer -= Time.deltaTime;
        }

        bool jumpInput = Input.touchCount > 0 || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);

        if (jumpInput && (isGrounded || jumpCounter < 2) && jumpCDTimer <= 0)
        {
            direction.y = jumpForce;
            player.velocity = direction;
            isGrounded = false;
            jumpCounter++;
            jumpCDTimer = jumpCD;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCounter = 0;
            jumpCDTimer = 0f;
        }
    }
}

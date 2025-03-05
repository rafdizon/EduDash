using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isGrounded { get; private set; }

    private Rigidbody2D player;
    private Vector2 direction;

    public float jumpForce;
    public float gravityForceApex;
    public float gravityForce;

    private float screenUpperLimit;

    public float flightDuration = 3.0f;
    public float flightTimer = 3.0f;

    public Transform magnet;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        magnet = transform.Find("Magnet Aura");
        
        screenUpperLimit = Camera.main.transform.position.y + 1;
    }
    private void Start()
    {
        magnet.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        direction = Vector2.zero;
    }

    private void Update()
    {
        bool jumpInput = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space);
        
        if (isGrounded)
        {
            flightTimer += 1.5f * Time.deltaTime;
            if(flightTimer >= flightDuration)
            {
                flightTimer = flightDuration;
            }
        }
        if (jumpInput && flightTimer > 0)
        {
            flightTimer -= Time.deltaTime;
            if (flightTimer <= 0)
            {
                flightTimer = 0;
            }
            if (transform.position.y < screenUpperLimit)
            {
                direction.y = jumpForce;
                player.velocity = direction;
                isGrounded = false;
            }
            else if(transform.position.y >= screenUpperLimit)
            {
                direction.y = 0;
                player.gravityScale = 0;
                player.velocity = direction;
            }
        }
        else if (Input.touchCount > 0 && flightTimer > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                flightTimer -= Time.deltaTime;
                if (flightTimer <= 0)
                {
                    flightTimer = 0;
                }
                if (transform.position.y < screenUpperLimit)
                {
                    direction.y = jumpForce;
                    player.velocity = direction;
                    isGrounded = false;
                }
                else if (transform.position.y >= screenUpperLimit)
                {
                    direction.y = 0;
                    player.gravityScale = 0;
                    player.velocity = direction;
                }
            }
        }
        else
        {
            player.gravityScale = gravityForce;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

using UnityEngine;

public class Coin : MonoBehaviour
{
    private float leftEdge;

    public float speed = 5;

    Rigidbody2D rb;

    private GameObject player;
    private void Awake()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
        rb.velocity = Vector2.left * GameManager.Instance.gameSpeed;
    }

    public void MoveToPlayer()
    {
        transform.position = Vector3.Lerp(this.transform.position, player.transform.position, Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.collectCoin);
            GameManager.Instance.coinCount++;
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float leftEdge;
    public float speedMultiplier;

    public GameObject hitEffect;

    private void Awake()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 5f;
    }
    private void Start()
    {
        GameManager.Instance.isObstacleOnScreen = true;
    }
    private void Update()
    {
        transform.position += (float)(GameManager.Instance.gameSpeed * speedMultiplier) * Time.deltaTime * Vector3.left;

        if (transform.position.x < leftEdge)
        {
            GameManager.Instance.isObstacleOnScreen = false;
            Destroy(gameObject);
        }
        
    }

    private void TriggerHit()
    {
        GameObject whiteOverlay = Instantiate(hitEffect, Vector3.zero, Quaternion.identity);
        whiteOverlay.transform.position = Camera.main.transform.position + Vector3.forward * 10; // Slightly in front of camera
        whiteOverlay.transform.localScale = new Vector3(
            Camera.main.orthographicSize * 2 * Camera.main.aspect,
            Camera.main.orthographicSize * 2,
            1
        );

        whiteOverlay.GetComponent<Hit_Effect>().TriggerHitEffect();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TriggerHit();
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
    }
}

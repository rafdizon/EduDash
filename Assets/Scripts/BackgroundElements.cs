using UnityEngine;

public class BackgroundElements : MonoBehaviour
{
    private float speedMulti = 0.17f;
    private float leftEdge;
    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 10f;
    }
    private void Update()
    {
        transform.position += (GameManager.Instance.gameSpeed * Time.deltaTime * Vector3.left) * speedMulti;
        if(transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }
} 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Magnet : MonoBehaviour
{
    private float leftEdge;
    private Spawner_PowerUps spawner_powerups;

    private void Awake()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 5f;
    }
    private void Start()
    {
        spawner_powerups = FindObjectOfType<Spawner_PowerUps>();
    }
    private void Update()
    {
        transform.position += GameManager.Instance.gameSpeed * Time.deltaTime * Vector3.left;

        if (transform.position.x < leftEdge)
        {
            spawner_powerups.isBuffOnScreen = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spawner_powerups.isBuffOnScreen = false;
            GameManager.Instance.PowerUpMagnet();
            Destroy(gameObject);
        }
    }
}

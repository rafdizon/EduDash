using UnityEngine;

public class Spawner_Coins : MonoBehaviour
{
    public GameObject coin;
    public float spawnInterval;

    private Spawner_Questions spawner_questions;
    private float spawnCD;

    private void Start()
    {
        spawner_questions = FindObjectOfType<Spawner_Questions>();
        spawnCD = Time.time + Random.Range(2f,5f);
    }

    private void Spawn()
    {
        int goldAmt = Random.Range(5, 8);
        Vector3 spawnPos = transform.position;

        for (int i = 0; i < goldAmt; i++)
        {
            spawnPos = new Vector3(spawnPos.x + spawnInterval, spawnPos.y);
            Instantiate(coin,spawnPos, Quaternion.identity);
        }
    }
    private void Update()
    {
        if (spawner_questions.isChoicesOnScreen)
        {
            spawnCD = Time.time + 1;
        }
        else if (!spawner_questions.isChoicesOnScreen && Time.time >= spawnCD)
        {
            Spawn();
            spawnCD = Time.time + Random.Range(8f, 10f);
        }
    }
}

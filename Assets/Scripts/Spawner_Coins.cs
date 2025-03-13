using UnityEngine;

public class Spawner_Coins : Spawner
{
    public GameObject coin;
    public float spawnInterval;

    private Spawner_Questions spawner_questions;
    private float spawnCD;

    private float screenUpperLimit;
    private float screenLowerLimit;

    private void Start()
    {
        spawner_questions = FindObjectOfType<Spawner_Questions>();
        spawnCD = Time.time + Random.Range(2f,5f);
    }
    private void Awake()
    {
        screenUpperLimit = Camera.main.transform.position.y + 5; // -3f
        screenLowerLimit = Camera.main.transform.position.y - 5; // +1.5f
    }

    private void Spawn()
    {
        int goldAmt = Random.Range(5, 8);
        Vector3 spawnPos = transform.position;
        spawnPos.y = Random.Range(screenLowerLimit + 1.5f, screenUpperLimit - 3f);

        for (int i = 0; i < goldAmt; i++)
        {
            spawnPos = new Vector3(spawnPos.x + spawnInterval, spawnPos.y);
            Instantiate(coin,spawnPos, Quaternion.identity);
        }
    }
    public override void Activate()
    {
        gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        gameObject?.SetActive(false);
    }
    private void Update()
    {
        if (spawner_questions.isChoicesOnScreen)
        {
            spawnCD = Time.time + 3;
        }
        else if (!spawner_questions.isChoicesOnScreen && Time.time >= spawnCD)
        {
            Spawn();
            spawnCD = Time.time + Random.Range(8f, 10f);
        }
    }
    public bool AreCoinsOnScreen()
    {
        return GameObject.FindGameObjectWithTag("Coin") != null;
    }
}

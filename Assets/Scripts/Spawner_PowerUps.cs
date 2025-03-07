using UnityEngine;

public class Spawner_PowerUps : MonoBehaviour
{
    private float screenUpperLimit;
    private float screenLowerLimit;

    private float spawnCD;

    private Spawner_Questions spawner_questions;
    private Spawner_Coins spawner_coins;
    public bool isBuffOnScreen;

    [System.Serializable]
    public struct Power_Up_Object
    {
        public GameObject prefab;
    }

    public Power_Up_Object[] powerups;

    private void Awake()
    {
        screenUpperLimit = Camera.main.transform.position.y + 5; // -3f
        screenLowerLimit = Camera.main.transform.position.y - 5; // +1.5f
    }

    private void Start()
    {
        spawner_questions = FindObjectOfType<Spawner_Questions>();
        spawner_coins = FindObjectOfType<Spawner_Coins>();

        spawnCD = Time.time + Random.Range(10f, 20f);
        isBuffOnScreen = false;
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
    public void Spawn()
    {
        isBuffOnScreen = true;
        float spawnChance = Random.value;

        Power_Up_Object selectedPowerUp = powerups[Random.Range(0, powerups.Length)];

        GameObject spawn = Instantiate(selectedPowerUp.prefab);
        Vector3 newPos = spawn.transform.position;
        newPos.x = transform.position.x;
        newPos.y = Random.Range(screenLowerLimit + 1.5f, screenUpperLimit - 3f);
        spawn.transform.position = newPos;
    }

    private void Update()
    {
        bool powerUpActive = GameManager.Instance.isPowerUp2x || GameManager.Instance.isPowerUpMagnet || GameManager.Instance.isPowerUpStrongLungs;

        if (isBuffOnScreen || spawner_coins.AreCoinsOnScreen() || powerUpActive)
        {
            spawnCD = Time.time + Random.Range(1f, 3f);
        }
        else if (!powerUpActive && Time.time >= spawnCD)
        {
            Spawn();
            spawnCD = Time.time + 10;
        }
    }
}

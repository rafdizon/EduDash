using UnityEngine;
using static Spawner_BG;

public class Spawner_Obstacles : MonoBehaviour
{
    private float screenUpperLimit;
    private float screenLowerLimit;

    private Spawner_Questions spawner_questions;

    private float spawnCD;

    [System.Serializable]
    public struct Obstacle_Object
    {
        public GameObject prefab;
        public float spawnChance;
        public float spawnOffsetUpper;
        public float spawnOffsetLower;
    }

    public Obstacle_Object[] obstacles;

    private void Awake()
    {
        screenUpperLimit = Camera.main.transform.position.y + 5; // -3f
        screenLowerLimit = Camera.main.transform.position.y - 5; // +1.5f

    }
    private void Start()
    {
        spawner_questions = FindObjectOfType<Spawner_Questions>();
        spawnCD = Time.time + Random.Range(3f, 6f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnEnable()
    {

    }

    public void Spawn()
    {
        float spawnChance = Random.value;

        foreach (Obstacle_Object obj in obstacles)
        {
            if (spawnChance < obj.spawnChance)
            {
                GameObject spawn = Instantiate(obj.prefab);
                Vector3 newPos = spawn.transform.position;
                newPos.x = transform.position.x;
                newPos.y = Random.Range(screenLowerLimit + obj.spawnOffsetLower, screenUpperLimit - obj.spawnOffsetUpper);
                spawn.transform.position = newPos;

                break;
            }
            spawnChance -= obj.spawnChance;
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
            spawnCD = Time.time + Random.Range(2f, 4f);
        }
    }
}

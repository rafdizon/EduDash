using UnityEngine;

public class Spawner_BG : MonoBehaviour
{
    [System.Serializable]
    public struct BGObject
    {
        public GameObject prefab;
        public float spawnChance;
    }

    public BGObject[] objects;
    private float minSpawnTime = 15f;
    private float maxSpawnTime = 30f;

    private void OnEnable()
    {
        Invoke("Spawn", Random.Range(minSpawnTime, maxSpawnTime));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Spawn()
    {
        float spawnChance = Random.value;

        foreach (BGObject obj in objects)
        {
            if(spawnChance < obj.spawnChance)
            {
                GameObject spawn = Instantiate(obj.prefab);
                Vector3 newPos = spawn.transform.position;
                newPos.x = transform.position.x;
                spawn.transform.position = newPos;

                break;
            }
            spawnChance -= obj.spawnChance;
        }
        Invoke("Spawn", Random.Range(minSpawnTime, maxSpawnTime));
    }
}

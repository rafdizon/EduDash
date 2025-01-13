using UnityEngine;

public class Spawner_Questions : MonoBehaviour
{
    [System.Serializable]
    public struct Portal_Object
    {
        public GameObject prefab;
        public string letterChoice;
    }

    public Portal_Object[] portals;

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnEnable()
    {
        Invoke("Spawn", 10f);
    }

    private void Spawn()
    {
        foreach (Portal_Object portal in portals)
        {
            GameObject spawn = Instantiate(portal.prefab);
            Vector3 newPos = spawn.transform.position;
            newPos.x = transform.position.x;
            spawn.transform.position = newPos;
        }
        Invoke("Spawn", 10f);
    }
}

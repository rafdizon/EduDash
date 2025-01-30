using UnityEngine;

public class Listener_Question : MonoBehaviour
{
    private float leftEdge;
    private Spawner_Questions spawner_questions;

    private void Awake()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
        spawner_questions = FindObjectOfType<Spawner_Questions>();
    }
    private void Start()
    {
        spawner_questions.isQuestionOnScreen = true;
        spawner_questions.isChoicesOnScreen = true;
    }
    private void Update()
    {
        transform.position += GameManager.Instance.gameSpeed * Time.deltaTime * Vector3.left;

        if (transform.position.x < leftEdge)
        {
            spawner_questions.isQuestionOnScreen = false;
            spawner_questions.isChoicesOnScreen = false;
            spawner_questions.spawnCD = Time.time + Random.Range(5f, 10f);
            Destroy(gameObject);
        }
    }
}

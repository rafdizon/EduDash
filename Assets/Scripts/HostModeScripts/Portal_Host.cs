using UnityEngine;

public class Portal_Host : MonoBehaviour
{
    private float leftEdge;
    private Spawner_Questions_Host spawner_questions;
    public string choice;
    public string letterChoice;
    private void Awake()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
        spawner_questions = FindObjectOfType<Spawner_Questions_Host>();
        spawner_questions.isAnswerSelected = false;
    }
    private void Update()
    {
        transform.position += (GameManager.Instance.gameSpeed * 0.75f) * Time.deltaTime * Vector3.left;

        if(transform.position.x < leftEdge)
        {
            Destroy(gameObject);

            spawner_questions.isAnswerSelected = false;
            spawner_questions.isQuestionOnScreen = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spawner_questions.AnswerSelected(letterChoice);
            //spawner_questions.answerSelected = letterChoice;
            Destroy(gameObject);
            //spawner_questions.isAnswerSelected = true;

            /*
            if (spawner_questions != null && !spawner_questions.isAnswerSelected)
            {
                spawner_questions.isAnswerSelected = true;
            }
            */
        }
    }
}

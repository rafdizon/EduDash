using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner_Questions : MonoBehaviour
{
    public bool isQuestionOnScreen;
    public bool isChoicesOnScreen;
    public bool isAnswerSelected;
    public float spawnCD;
    public string question;
    public string answerSelected;
    public string correctAnswer;
    [System.Serializable]
    public struct Portal_Object
    {
        public GameObject prefab;
        public string letterChoice;
    }

    public Portal_Object[] portals;
    public GameObject listener;
    public TextMeshProUGUI controlPanelText;
    public GameObject damageEffect;

    private List<GameObject> activePortals = new List<GameObject>();
    private List<Questions> questionsList;
    private Questions_DB_Manager dbManager;

    private void Start()
    {
        if (portals.Length > 0)
        {
            GameObject tempPortal = Instantiate(portals[0].prefab);
            Destroy(tempPortal);
        }
        string subject = GameParamManager.subject;
        string difficulty = GameParamManager.difficulty;
        spawnCD = Time.time + Random.Range(4f, 7f);
        isAnswerSelected = false;
        isQuestionOnScreen = false;
        isChoicesOnScreen = false;

        controlPanelText.text = "";
        dbManager = FindObjectOfType<Questions_DB_Manager>();
        dbManager.GetQuestions(subject, difficulty, (questions) =>
        {
            questionsList = questions;
        }
        );
        Debug.Log($"Get success: {questionsList != null}");
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    public void Spawn()
    {
        if (questionsList != null)
        {
            isQuestionOnScreen = true;
            Debug.Log($"Number: {questionsList.Count}");
            var randomQuestionIndex = Random.Range(0, (questionsList.Count - 1));
            //Debug.Log($"Index: {randomQuestionIndex}");
            question = questionsList[randomQuestionIndex].Question;
            controlPanelText.text = question;

            correctAnswer = questionsList[randomQuestionIndex].Correct_Answer;
            //Debug.Log($"Q: {questionsList[randomQuestionIndex].Question}");
            Debug.Log($"Answer: {questionsList[randomQuestionIndex].Correct_Answer}");
            float i = (portals.Length - 1) * 0.7f;

            //portals = portals.OrderBy(p => Random.value).ToArray();

            foreach (Portal_Object portal in portals)
            {
                GameObject spawn = Instantiate(portal.prefab);
                Vector3 newPos = spawn.transform.position;
                newPos.x = transform.position.x + i;
                spawn.transform.position = newPos;


                var portalScript = spawn.GetComponent<Portal>();
                if (portalScript != null)
                {
                    portalScript.letterChoice = portal.letterChoice;
                }

                string choice = "";

                switch (portalScript.letterChoice)
                {
                    case "A":
                        choice = questionsList[randomQuestionIndex].Choices_A;
                        break;
                    case "B":
                        choice = questionsList[randomQuestionIndex].Choices_B;
                        break;
                    case "C":
                        choice = questionsList[randomQuestionIndex].Choices_C;
                        break;
                }

                spawn.GetComponentInChildren<TMP_Text>().text = choice;
                
                activePortals.Add(spawn);
                i -= 0.7f;
            }
            GameObject spawnListener = Instantiate(listener);
            Vector3 listenerPos = spawnListener.transform.position;
            listenerPos.x = transform.position.x;
            spawnListener.transform.position = listenerPos;
        }
    }

    private void Update()
    {    
        if (!isQuestionOnScreen && Time.time >= spawnCD)
        {
            Spawn();
        }
    }
    public void AnswerSelected(string answer)
    {
        isAnswerSelected = true;
        foreach (GameObject portal in activePortals)
        {
            if (portal != null)
            {
                BoxCollider2D br = portal.GetComponent<BoxCollider2D>();
                br.enabled = false;
            }
        }

        if(correctAnswer == answer)
        {
            Debug.Log("CORRECT");
            GameManager.Instance.score += GameManager.Instance.isPowerUp2x ? 2 : 1;
            controlPanelText.text = "Correct! Oxygen Replenished..."; 
            if(GameManager.Instance.oxygenLevel < 80)
            {
                GameManager.Instance.oxygenLevel += 20;
            }
            else
            {
                GameManager.Instance.oxygenLevel = 100;
            }
        }else
        {
            TriggerDamage();
            controlPanelText.text = "Warning! Oxygen Leak!";
            if (GameManager.Instance.isPowerUpStrongLungs)
            {
                if (GameManager.Instance.oxygenLevel > 10)
                {
                    GameManager.Instance.oxygenLevel -= 10;
                }
                else
                {
                    GameManager.Instance.oxygenLevel = 0;
                }
            } else
            {
                if (GameManager.Instance.oxygenLevel > 20)
                {
                    GameManager.Instance.oxygenLevel -= 20;
                }
                else
                {
                    GameManager.Instance.oxygenLevel = 0;
                }
            }
            Debug.Log("WRONG");
        }
        
        StartCoroutine(ClearControlPanelText());
        answerSelected = "";
        activePortals.Clear();
    }

    private IEnumerator ClearControlPanelText()
    {
        yield return new WaitForSeconds(3);
        controlPanelText.text = "";
    }

    public void TriggerDamage()
    {
        GameObject redOverlay = Instantiate(damageEffect, Vector3.zero, Quaternion.identity);
        redOverlay.transform.position = Camera.main.transform.position + Vector3.forward * 10; 
        redOverlay.transform.localScale = new Vector3(
            Camera.main.orthographicSize * 2 * Camera.main.aspect,
            Camera.main.orthographicSize * 2,
            1
        );

        redOverlay.GetComponent<Damage_Effect>().TriggerDamageEffect();
    }
}
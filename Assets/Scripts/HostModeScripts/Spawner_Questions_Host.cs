using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
public class Spawner_Questions_Host : Spawner
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
    private List<Dictionary<string, object>> questionsList;

    private void Start()
    {
        if (portals.Length > 0)
        {
            GameObject tempPortal = Instantiate(portals[0].prefab);
            Destroy(tempPortal);
        }
        string subject = GameParamManager.subject;
        string topic = GameParamManager.topic;
        spawnCD = Time.time + Random.Range(4f, 7f);
        isAnswerSelected = false;
        isQuestionOnScreen = false;
        isChoicesOnScreen = false;

        controlPanelText.text = "";
        questionsList = QuizDataHolder.Instance.quizData;
        Debug.Log($"Get success: {questionsList != null}");
    }
    private void OnDisable()
    {
        CancelInvoke();
    }

    public override void Activate()
    {
        gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        gameObject?.SetActive(false);
    }

    public void Spawn()
    {
        if (questionsList != null)
        {
            if (isQuestionOnScreen) return;
            isQuestionOnScreen = true;
            Debug.Log($"Number: {questionsList.Count}");
            var randomQuestionIndex = Random.Range(0, (questionsList.Count - 1));
            //Debug.Log($"Index: {randomQuestionIndex}");
            question = questionsList[randomQuestionIndex]["question"].ToString();
            controlPanelText.text = question;

            correctAnswer = questionsList[randomQuestionIndex]["correct_answer"].ToString();
            //Debug.Log($"Q: {questionsList[randomQuestionIndex].Question}");
            Debug.Log($"Answer: {questionsList[randomQuestionIndex]["correct_answer"]}");
            float i = (portals.Length - 1) * 0.7f;

            foreach (Portal_Object portal in portals)
            {
                GameObject spawn = Instantiate(portal.prefab);
                Vector3 newPos = spawn.transform.position;
                newPos.x = transform.position.x + i;
                spawn.transform.position = newPos;


                var portalScript = spawn.GetComponent<Portal_Host>();
                if (portalScript != null)
                {
                    portalScript.letterChoice = portal.letterChoice;
                }

                string choice = "";

                //Debug.Log(questionsList[randomQuestionIndex].Choice_A);

                switch (portalScript.letterChoice)
                {
                    case "A":
                        choice = questionsList[randomQuestionIndex]["choice_a"].ToString();
                        break;
                    case "B":
                        choice = questionsList[randomQuestionIndex]["choice_b"].ToString();
                        break;
                    case "C":
                        choice = questionsList[randomQuestionIndex]["choice_c"].ToString();
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
        Debug.Log("Answer Selected: " + answer);
        foreach (GameObject portal in activePortals)
        {
            if (portal != null)
            {
                BoxCollider2D br = portal.GetComponent<BoxCollider2D>();
                br.enabled = false;
            }
        }

        if(correctAnswer.ToLower() == answer.ToLower())
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
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float gameSpeedInit = 6f;
    public float gameSpeedMulti = 0.105f;

    public TextMeshProUGUI oxygenLevelText;
    public TextMeshProUGUI coinCountText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI controlPanelText;
    public Button playAgainBtn;

    public RectTransform gameOverScreen;

    public Spawner_BG spawnerBG;
    public Spawner_Coins spawnerCoins;
    public Spawner_Questions spawnerQuestions;
    public Spawner_Obstacles spawnerObstacles;

    public float gameSpeed { get; private set; }

    public float oxygenLevel;
    public int coinCount;
    public int score;

    public bool isObstacleOnScreen = false;
    private Player player;
    private bool isOxygenEmpty;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    private void Start()
    {
        //Application.targetFrameRate = 15;
        player = FindObjectOfType<Player>();

        NewGame();
    }
    private void NewGame()
    {
        
        gameSpeed = gameSpeedInit;
        enabled = true;

        isOxygenEmpty = false;

        player.gameObject.SetActive(true);
        gameOverScreen.gameObject.SetActive(false);

        spawnerBG.gameObject.SetActive(true);
        spawnerCoins.gameObject.SetActive(true);
        spawnerQuestions.gameObject.SetActive(true);
        spawnerObstacles.gameObject.SetActive(true);

        oxygenLevel = 100;
        coinCount = 0;
        score = 0;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        gameSpeed += gameSpeedMulti * Time.deltaTime;
        oxygenLevel -= Time.deltaTime / 1.5f;
        oxygenLevelText.text = "Oxygen: " + oxygenLevel.ToString("0");
        coinCountText.text = "Coins: " + coinCount.ToString();
        ScoreText.text = "Score: " + score.ToString();

        if(oxygenLevel <= 0)
        {
            isOxygenEmpty = true;
            GameOver();
        }
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;

        StartCoroutine(GameOverScreen(2));

        spawnerBG.gameObject.SetActive(false);
        spawnerCoins.gameObject.SetActive(false);
        spawnerQuestions.gameObject.SetActive(false);
        spawnerObstacles.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
    }

    private IEnumerator GameOverScreen(int delay)
    {
        yield return new WaitForSeconds(delay);
        gameOverScreen.gameObject.SetActive(true);

        if(isOxygenEmpty)
        {
            controlPanelText.text = "You ran out of oxygen!";
        }
        else
        {
            controlPanelText.text = "You got struck!";
        }
    }
}

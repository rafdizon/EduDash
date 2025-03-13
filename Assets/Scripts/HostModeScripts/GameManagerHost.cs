using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerHost : MonoBehaviour
{
    public static GameManagerHost Instance { get; private set; }

    public Dictionary<string, object> customQuiz;

    private const float MAGNET_DURATION = 30f;
    private const float DOUBLE_SCORE_DURATION = 35f;
    private const float STRONG_LUNGS_DURATION = 18f;

    public float gameSpeedInit = 6f;
    public float gameSpeedMulti = 0.105f;

    public TextMeshProUGUI oxygenLevelText;
    public TextMeshProUGUI coinCountText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI controlPanelText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI totalCoinsText;
    public TextMeshProUGUI powerUpText;
    public Button playAgainBtn;

    public RectTransform gameOverScreen;
    public RectTransform pauseScreen;

    public Spawner_BG spawnerBG;
    public Spawner_Coins_Host spawnerCoins;
    public Spawner_Questions_Host spawnerQuestions;
    public Spawner_Obstacles_Host spawnerObstacles;

    public Slider oxygenLevelBar;
    public Slider powerUpBarDuration;
    public Slider fuelBar;

    public Image buffIcon;
    public Sprite[] buffIcons;

    public float gameSpeed { get; private set; }

    public float oxygenLevel;
    public int coinCount;
    public int score;

    public bool isPowerUp2x = false;
    public bool isPowerUpMagnet = false;
    public bool isPowerUpStrongLungs = false;

    public bool isObstacleOnScreen = false;
    private Player player;
    private bool isOxygenEmpty;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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
        player = FindObjectOfType<Player>();
        pauseScreen.gameObject.SetActive(false);
        oxygenLevelBar.maxValue = 100;
        fuelBar.maxValue = player.flightDuration;
        fuelBar.value = player.flightDuration;
        powerUpBarDuration.gameObject.SetActive(false);
        buffIcon.gameObject.SetActive(false);
        NewGame();
    }
    private void NewGame()
    {
        Time.timeScale = 1f;
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
        oxygenLevel -= isPowerUpStrongLungs ? (Time.deltaTime / 1.5f) / 2 : (Time.deltaTime / 1.5f);
        oxygenLevelBar.value = oxygenLevel;
        oxygenLevelText.text = "Oxygen: " + oxygenLevel.ToString("F0");
        coinCountText.text = "Coins: " + coinCount.ToString();
        ScoreText.text = "Score: " + score.ToString();
        fuelBar.value = player.flightTimer;
        if (oxygenLevel <= 0)
        {
            isOxygenEmpty = true;
            GameOver();
        }
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;

        User_Info userInfo = SaveSystem.LoadInfo();
        userInfo.coins += coinCount;

        if (userInfo.highScore < score)
        {
            userInfo.highScore = score;
        }
        SaveSystem.SaveInfo(userInfo);

        StartCoroutine(GameOverScreen(2, userInfo.highScore, userInfo.coins));

        spawnerBG.gameObject.SetActive(false);
        spawnerCoins.gameObject.SetActive(false);
        spawnerQuestions.gameObject.SetActive(false);
        spawnerObstacles.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
    }
    public void Pause()
    {
        pauseScreen.gameObject.SetActive(true);

        Time.timeScale = 0f;

        spawnerBG.gameObject.SetActive(false);
        spawnerCoins.gameObject.SetActive(false);
        spawnerQuestions.gameObject.SetActive(false);
        spawnerObstacles.gameObject.SetActive(false);

    }
    public void Resume()
    {
        pauseScreen.gameObject.SetActive(false);

        Time.timeScale = 1f;

        spawnerBG.gameObject.SetActive(true);
        spawnerCoins.gameObject.SetActive(true);
        spawnerQuestions.gameObject.SetActive(true);
        spawnerObstacles.gameObject.SetActive(true);
    }

    public void PowerUp2xScore()
    {
        isPowerUp2x = true;
        powerUpBarDuration.maxValue = DOUBLE_SCORE_DURATION;
        powerUpBarDuration.gameObject.SetActive(true);
        buffIcon.gameObject.SetActive(true);
        buffIcon.sprite = buffIcons[0];
        StartCoroutine(PowerUp2xScoreDuration(DOUBLE_SCORE_DURATION));
    }
    public void PowerUpMagnet()
    {
        isPowerUpMagnet = true;
        player.magnet.gameObject.SetActive(true);
        powerUpBarDuration.maxValue = MAGNET_DURATION;
        powerUpBarDuration.gameObject.SetActive(true);
        buffIcon.gameObject.SetActive(true);
        buffIcon.sprite = buffIcons[1];
        StartCoroutine(PowerUpMagnetDuration(MAGNET_DURATION));
    }
    public void PowerUpStrongLungs()
    {
        isPowerUpStrongLungs = true;
        powerUpBarDuration.maxValue = STRONG_LUNGS_DURATION;
        powerUpBarDuration.gameObject.SetActive(true);
        buffIcon.gameObject.SetActive(true);
        buffIcon.sprite = buffIcons[2];
        StartCoroutine(PowerUpStrongLungsDuration(STRONG_LUNGS_DURATION));
    }
    private IEnumerator GameOverScreen(int delay, int highScore, int totalCoins)
    {
        yield return new WaitForSeconds(delay);
        gameOverScreen.gameObject.SetActive(true);

        highScoreText.text = $"{highScore}";
        totalCoinsText.text = $"{totalCoins}";
        if (isOxygenEmpty)
        {
            controlPanelText.text = "You ran out of oxygen!";
        }
        else
        {
            controlPanelText.text = "You got struck!";
        }
    }

    private IEnumerator PowerUp2xScoreDuration(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            powerUpBarDuration.value = duration - elapsed;
            elapsed += Time.deltaTime;
            yield return null;
        }
        powerUpBarDuration.gameObject.SetActive(false);
        buffIcon.gameObject.SetActive(false);
        isPowerUp2x = false;
        powerUpBarDuration.value = 0;
    }

    private IEnumerator PowerUpMagnetDuration(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            powerUpBarDuration.value = duration - elapsed;
            elapsed += Time.deltaTime;
            yield return null;
        }
        powerUpBarDuration.gameObject.SetActive(false);
        isPowerUpMagnet = false;
        player.magnet.gameObject.SetActive(false);
        buffIcon.gameObject.SetActive(false);
        powerUpBarDuration.value = 0;
    }
    private IEnumerator PowerUpStrongLungsDuration(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            powerUpBarDuration.value = duration - elapsed;
            elapsed += Time.deltaTime;
            yield return null;
        }
        powerUpBarDuration.gameObject.SetActive(false);
        buffIcon.gameObject.SetActive(false);
        isPowerUpStrongLungs = false;
        powerUpBarDuration.value = 0;
    }
}

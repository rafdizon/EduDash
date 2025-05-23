using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    public Spawner spawnerCoins;
    public Spawner spawnerQuestions;
    public Spawner spawnerObstacles;

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

    public bool isHostMode = false;
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
        AudioManager.Instance.PlayMusic(AudioManager.Instance.musicGame);
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
        spawnerCoins.Activate();
        spawnerQuestions.Activate();
        spawnerObstacles.Activate();

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
        if(oxygenLevel <= 0)
        {
            isOxygenEmpty = true;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.deathNoOxygen);
            GameOver();
        }
    }

    public async void GameOver(string gameOverText = "")
    {
        gameSpeed = 0f;
        enabled = false;

        User_Info userInfo = SaveSystem.LoadInfo();
        userInfo.coins += coinCount;

        if(userInfo.highScore < score)
        {
            userInfo.highScore = score;
        }
        SaveSystem.SaveInfo(userInfo);

        if(isHostMode)
        {
            await FirestoreManager.Instance.AddToHistory(score);
        }

        StartCoroutine(GameOverScreen(2, userInfo.highScore, userInfo.coins, gameOverText));

        spawnerBG.gameObject.SetActive(false);
        spawnerCoins.Deactivate();
        spawnerQuestions.Deactivate();
        spawnerObstacles.Deactivate();
        player.gameObject.SetActive(false);
    }
    public void Pause()
    {
        pauseScreen.gameObject.SetActive(true);

        Time.timeScale = 0f;

        spawnerBG.gameObject.SetActive(false);
        spawnerCoins.Deactivate();
        spawnerQuestions.Deactivate();
        spawnerObstacles.Deactivate();
        
    }
    public void Resume()
    {
        pauseScreen.gameObject.SetActive(false);

        Time.timeScale = 1f;

        spawnerBG.gameObject.SetActive(true);
        spawnerCoins.Activate();
        spawnerQuestions.Activate();
        spawnerObstacles.Activate();
    }

    public void PowerUp2xScore()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.collectBuff);
        isPowerUp2x = true;
        powerUpBarDuration.maxValue = DOUBLE_SCORE_DURATION;
        powerUpBarDuration.gameObject.SetActive(true);
        buffIcon.gameObject.SetActive(true);
        buffIcon.sprite = buffIcons[0];
        StartCoroutine(PowerUp2xScoreDuration(DOUBLE_SCORE_DURATION));
    }
    public void PowerUpMagnet()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.collectBuff);
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
        AudioManager.Instance.PlaySFX(AudioManager.Instance.collectBuff);
        isPowerUpStrongLungs = true;
        powerUpBarDuration.maxValue = STRONG_LUNGS_DURATION;
        powerUpBarDuration.gameObject.SetActive(true);
        buffIcon.gameObject.SetActive(true);
        buffIcon.sprite = buffIcons[2];
        StartCoroutine(PowerUpStrongLungsDuration(STRONG_LUNGS_DURATION));
    }
    private IEnumerator GameOverScreen(int delay, int highScore, int totalCoins, string gameOverText)
    {
        yield return new WaitForSeconds(delay);
        gameOverScreen.gameObject.SetActive(true);
        if(isHostMode)
        {
            playAgainBtn.gameObject.SetActive(false);
        }

        highScoreText.text = $"{highScore}";
        totalCoinsText.text = $"{totalCoins}";
        if(gameOverText == "")
        {
            if (isOxygenEmpty)
            {
                controlPanelText.text = "You ran out of oxygen!";
            }
            else
            {
                controlPanelText.text = "You got struck!";
            }
        }
        else
        {
            controlPanelText.text = gameOverText;
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
        //yield return new WaitForSeconds(duration);
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
        //yield return new WaitForSeconds(duration);
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
        //yield return new WaitForSeconds(duration);
        powerUpBarDuration.gameObject.SetActive(false);
        buffIcon.gameObject.SetActive(false);
        isPowerUpStrongLungs = false;
        powerUpBarDuration.value = 0;
    }
}

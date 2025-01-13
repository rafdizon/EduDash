using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float gameSpeedInit = 6f;
    public float gameSpeedMulti = 0.105f;

    //public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI coinCountText;
    //public TextMeshProUGUI gameOverText;
    //public Button playAgainButton;
    //public Button homeButton;
    public float gameSpeed { get; private set; }

    public float oxygenLevel;
    public int coinCount;
    public int score;

    private Player player;

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
        player = FindObjectOfType<Player>();

        NewGame();
    }
    public void NewGame()
    {
        gameSpeed = gameSpeedInit;
        enabled = true;

        player.gameObject.SetActive(true);

        //gameOverText.gameObject.SetActive(false);
        //playAgainButton.gameObject.SetActive(false);
        //homeButton.gameObject.SetActive(false);

        coinCount = 0;
        score = 0;
    }
    private void Update()
    {
        gameSpeed += gameSpeedMulti * Time.deltaTime;


        //scoreText.text = $"Score: {score}";
        //coinCountText.text = $"{coinCount}";

    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);

        //gameOverText.gameObject.SetActive(true);
        //playAgainButton.gameObject.SetActive(true);
        //homeButton.gameObject.SetActive(true);

    }
}

using UnityEngine;

public class Animate_Coin : MonoBehaviour
{
    public Sprite[] spinning_coins;

    private SpriteRenderer sr;
    private int frame;
    private Coin coin;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        coin = GetComponent<Coin>();
    }
    private void OnEnable()
    {
        Invoke("Animate", 0f);
    }
    private void Animate()
    {
        frame++;
        if (frame >= spinning_coins.Length)
        {
            frame = 0;
        }
        if (frame >= 0 && frame < spinning_coins.Length) 
        {
            sr.sprite = spinning_coins[frame];
        }
        Invoke("Animate", (1f/GameManager.Instance.gameSpeed) * 0.5f);
    }
}

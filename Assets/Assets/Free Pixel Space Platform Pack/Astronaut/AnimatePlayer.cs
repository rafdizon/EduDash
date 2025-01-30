using UnityEngine;

public class AnimatePlayer : MonoBehaviour
{
    public Sprite[] sprites_running;
    public Sprite[] sprites_jumping;

    private SpriteRenderer sr;
    private int frame;
    private Player player;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();
    }
    private void OnEnable()
    {
        Invoke("Animate", 0f);
    }

    private void Animate()
    {
        Sprite[] sprites = player.isGrounded ? sprites_running : sprites_jumping;
        frame++;

        if (frame >= sprites.Length)
        {
            frame = 0;
        }

        if (frame >= 0 && frame < sprites.Length)
        {
            sr.sprite = sprites[frame];
        }
        Invoke("Animate", 1f / GameManager.Instance.gameSpeed);
    }
}

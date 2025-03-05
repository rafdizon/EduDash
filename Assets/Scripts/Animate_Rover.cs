using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate_Rover : MonoBehaviour
{
    public Sprite[] rover_sprites;

    private SpriteRenderer sr;
    private int frame;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        Invoke("Animate", 0f);
    }

    private void Animate()
    {
        frame++;
        if (frame >= rover_sprites.Length)
        {
            frame = 0;
        }
        if (frame >= 0 && frame < rover_sprites.Length)
        {
            sr.sprite = rover_sprites[frame];
        }
        Invoke("Animate", (1f / GameManager.Instance.gameSpeed) * 1);
    }
}

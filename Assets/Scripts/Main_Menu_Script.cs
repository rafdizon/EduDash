using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Main_Menu_Script : MonoBehaviour
{
    public TextMeshProUGUI totalCoinsText;
    public Button muteMusic;
    public Button muteSFX;

    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;
    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.musicMenu);
    }
    private void Update()
    {
        User_Info userInfo = SaveSystem.LoadInfo();
        totalCoinsText.text = userInfo.coins.ToString();
    }
    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMuteMusic();
        muteMusic.image.sprite = AudioManager.Instance.musicSource.mute ? musicOffSprite : musicOnSprite;
    }
    public void ToggleSfx()
    {
        AudioManager.Instance.ToggleMuteSfx();
        muteSFX.image.sprite = AudioManager.Instance.sfxSource.mute ? sfxOffSprite : sfxOnSprite;
    }
}

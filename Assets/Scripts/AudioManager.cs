using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip musicMenu;
    public AudioClip musicGame;
    public AudioClip answerCorrect;
    public AudioClip answerWrong;
    public AudioClip collectBuff;
    public AudioClip collectCoin;
    public AudioClip deathNoOxygen;
    public AudioClip deathHit;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void PlayMusic(AudioClip clip)
    {
        if (clip != musicSource.clip)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
    public void ToggleMuteMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleMuteSfx()
    {
        sfxSource.mute = !sfxSource.mute;
    }
}

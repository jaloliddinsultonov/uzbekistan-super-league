using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Goal Sound Effects")]
    public AudioClip[] goalSounds;

    [Header("Background Music")]
    public AudioClip backgroundMusic;

    [Header("Audio Sources")]
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;

    [HideInInspector] public float sfxVolume = 1f;
    [HideInInspector] public float musicVolume = 0.5f;

    private bool musicStarted = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Listen for scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This fires automatically every time any scene loads !
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Match")
        {
            StopBackgroundMusic();
            musicStarted = false;
        }
        else if (scene.name == "MainMenu")
        {
            if(!musicStarted)
            {
                PlayBackgroundMusic();
                musicStarted = true;
            }
        }
    }


    public void PlayBackgroundMusic()
    {
        if (backgroundMusic == null) return;

        musicSource.clip = backgroundMusic;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }

    public void PlayGoalSound()
    {
        if (goalSounds.Length == 0) return;

        // select random goal sound from a list
        int randomIndex = Random.Range(0, goalSounds.Length);
        sfxSource.volume = sfxVolume;
        sfxSource.PlayOneShot(goalSounds[randomIndex]);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = volume;
    }
}

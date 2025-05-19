using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    [Header("Music")]
    public AudioSource songSource;
    public AudioClip[] playlist;

    [Header("Nonogram Sounds")]
    public AudioSource soundSource;
    public AudioClip gridTileSFX;
    public AudioClip undoRedoSFX;
    public AudioClip restartSFX;
    public AudioClip completeSFX;
    public AudioClip pauseSFX;
    public AudioClip clueSolvedSFX;
    public AudioClip clueUnsolvedSFX;

    [Header("Start/Map Sounds")]
    public AudioClip nodeClickSFX;
    public AudioClip hoverSFX;

    [Header("General Sounds")]
    public AudioClip generalUIButton;
    public AudioClip emptyInput;

    void Awake()
    {
        songSource = GetComponent<AudioSource>();
        PlayNext(0);
    }

    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        if (newScene.name == "NonogramGameScene")
        {
            PlayNext(Random.Range(1, playlist.Length));
        }
        else if (songSource.clip != playlist[0])
        {
            PlayNext(0);
        }
    }

    void Update()
    {
        if (!songSource.isPlaying)
        {
            PlayNext(Random.Range(1, playlist.Length));
        }
    }

    public void PlayNext(int songIndex)
    {
        songSource.clip = playlist[songIndex];
        songSource.Play();
    }

    public void PlaySFX(AudioClip sound)
    {
        soundSource.PlayOneShot(sound);
    }
}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    public AudioSource songSource;
    public AudioClip[] playlist;
    public float volume;

    [Header("Nonogram Sounds")]
    public AudioSource soundSource;
    public AudioClip gridTileSFX;
    public AudioClip undoRedoSFX;
    public AudioClip restartSFX;
    public AudioClip completeSFX;
    public AudioClip pauseSFX;

    [Header("Start/Map Sounds")]
    public AudioClip nodeClickSFX;
    public AudioClip hoverSFX;

    [Header("General Sounds")]
    public AudioClip generalUIButton;
    public AudioClip invalidAction;

    void Start()
    {
        songSource = GetComponent<AudioSource>();

        if (!songSource.isPlaying)
        {
            PlayNext(Random.Range(0, playlist.Length));
        }
    }

    void Update()
    {
        songSource.volume = volume;

        if (!songSource.isPlaying)
        {
            PlayNext(Random.Range(0, playlist.Length));
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

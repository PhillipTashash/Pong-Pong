using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // The one shared instance. Anyone can read it; only this class can set it.
    public static AudioManager Instance { get; private set; }

    [Header("UI Sounds")]
    [SerializeField] private AudioClip[] hoverSounds;
    [SerializeField] private AudioClip[] typeSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip pongStartSound;

    private AudioSource audioSource;

    void Awake()
    {
        // If another AudioManager already exists, this one is a duplicate. Remove it.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);   // survive scene changes
        audioSource = GetComponent<AudioSource>();
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Play any clip (for game sounds later: paddle hits, scoring...)
    public void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip);
    }

    // UI shortcuts so both menus sound the same
    public void PlayHover()
    {
        if (hoverSounds.Length == 0) return;
        PlaySound(hoverSounds[Random.Range(0, hoverSounds.Length)]);
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = Mathf.Clamp01(volume);
    }

    public void PlayTypeSound()
    {
        if (typeSound.Length == 0) return;
        PlaySound(typeSound[Random.Range(0, typeSound.Length)]);
    }

    public void PlayClick()
    {
        PlaySound(clickSound);
        PlaySound(pongStartSound);
    }
}
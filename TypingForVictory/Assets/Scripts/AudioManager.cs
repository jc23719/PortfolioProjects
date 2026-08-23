using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip[] sfxClips;
    public AudioClip skeletonSFX;
    public AudioClip zombieSFX;

    private List<AudioSource> ambient3DSources = new List<AudioSource>();

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject); // prevent duplicates
        }
    }

    public void PlayMusic(AudioClip clip) {
        if (musicSource != null && clip != null) {
            if (musicSource.clip != clip) {
                musicSource.clip = clip;
                musicSource.loop = true;
                musicSource.Play();
            } else if (!musicSource.isPlaying) {
                musicSource.Play();
            }
        }
    }


    public void PlaySFX(string name) {
        AudioClip clip = System.Array.Find(sfxClips, c => c.name == name);
        if (clip != null) {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void Play3DSFX(string clipName, Vector3 position) {
        AudioClip clip = null;

        if (clipName.ToLower() == "skeleton") {
            clip = skeletonSFX;
        } else if (clipName.ToLower() == "zombie") {
            clip = zombieSFX;
        } 

        if (clip != null) {
            PlayClipAtPoint(clip, position, sfxSource.volume);
        }
    }

    private void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume) {
        if (clip == null) return;

        // Create a temporary GameObject
        GameObject tempGO = new GameObject("Audio_" + clip.name);
        tempGO.transform.position = position;

        // Add an AudioSource component
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();

        // 3D Sound Configuration
        audioSource.clip = clip;
        audioSource.volume = volume; 
        audioSource.spatialBlend = 1.0f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic; // Sound gets quieter with distance

        audioSource.Play();

        // Destroy the GameObject after the clip finishes playing
        Destroy(tempGO, clip.length);
    }

    public void SetMusicVolume(float volume) {
        if (musicSource != null) {
            musicSource.volume = Mathf.Clamp01(volume);

            if (!musicSource.isPlaying && musicSource.clip != null) {
                Debug.Log("[AudioManager] Restarting playback");
                musicSource.Play();
            }
        }
    }

    public void RegisterAmbientSource(AudioSource source)
    {
        if (!ambient3DSources.Contains(source))
        {
            ambient3DSources.Add(source);
        }
    }

    public void UnregisterAmbientSource(AudioSource source)
    {
        ambient3DSources.Remove(source);
    }

    public void SetSFXVolume(float volume) {
        float clampedVolume = Mathf.Clamp01(volume);
        
        if (sfxSource != null) {
            sfxSource.volume = clampedVolume;
        }

        for (int i = ambient3DSources.Count - 1; i >= 0; i--)
        {
            AudioSource source = ambient3DSources[i];
            if (source == null)
            {
                ambient3DSources.RemoveAt(i);
            }
            else
            {
                source.volume = clampedVolume;
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class NamedAudioClip
    {
        public string name;
        public AudioClip clip;
        public bool loop; // For looping sounds like walk
    }

    public List<NamedAudioClip> audioClips;
    private Dictionary<string, AudioClip> clipDict;
    private Dictionary<string, bool> loopDict;

    private List<AudioSource> sfxSources;
    private int sfxSourceCount = 5;
    private int currentIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildClipDictionaries();
            CreateAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void BuildClipDictionaries()
    {
        clipDict = new Dictionary<string, AudioClip>();
        loopDict = new Dictionary<string, bool>();

        foreach (var item in audioClips)
        {
            clipDict[item.name] = item.clip;
            loopDict[item.name] = item.loop;
        }
    }

    private void CreateAudioSources()
    {
        sfxSources = new List<AudioSource>();
        for (int i = 0; i < sfxSourceCount; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(source);
        }
    }

    public void PlaySound(string name)
    {
        if (!clipDict.ContainsKey(name))
        {
            Debug.LogWarning($"Sound '{name}' not found.");
            return;
        }

        var source = sfxSources[currentIndex];
        source.loop = loopDict[name]; // Loop if needed
        source.clip = clipDict[name];
        source.Play();

        currentIndex = (currentIndex + 1) % sfxSources.Count;
    }

    public void StopSound(string name)
    {
        foreach (var source in sfxSources)
        {
            if (source.isPlaying && source.clip == clipDict[name])
            {
                source.Stop();
            }
        }
    }
}

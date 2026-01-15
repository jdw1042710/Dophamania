using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public float bgmVolume { get; set; } = 0.5f;

    private AudioSource bgmPlayer;
    private Dictionary<string, AudioClip> bgmClips;
    private readonly string addressableBGMLabel = "BGM";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        bgmClips = new Dictionary<string, AudioClip>();
        bgmPlayer = GetComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
    }

    private void Start()
    {
        LoadBGMClips();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayBGM(SceneManager.GetActiveScene());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGM(scene);
    }

    private void PlayBGM(Scene scene)
    {
        var clipName = $"{scene.name}BGM";
        if(bgmClips.TryGetValue(clipName, out AudioClip clip))
        {
            bgmPlayer.clip = clip;
            bgmPlayer.Play();
        }
    }

    void LoadBGMClips()
    {
        var handle = Addressables.LoadAssetsAsync<AudioClip>(addressableBGMLabel);
        handle.WaitForCompletion();
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var clip in handle.Result)
            {
                if (bgmClips.ContainsKey(clip.name)) continue;
                bgmClips.Add(clip.name, clip);
                Debug.Log($"Asset {clip.name} Is Loaded");
            }
        }
        else
        {
            Debug.LogError("BGM Load Failed");
        }
    }
}

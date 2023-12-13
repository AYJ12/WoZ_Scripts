using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // ���� �̸�
    public AudioClip clip; // ��
}

public class SoundManager
{
    AudioSource[] audioSources = new AudioSource[(int)Define.Sound.Count];
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();


    public void Init()
    {
        GameObject root = GameObject.Find("Sound");

        if (root == null)
        {
            root = new GameObject { name = "Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            //AsyncOperationHandle<AudioMixer> handle = Addressables.LoadAssetAsync<AudioMixer>("Assets/Resources_moved/Sounds/AudioMixer.mixer");
            //handle.Completed += OnAudioMixerLoaded;
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }
        }
    }

    //private void OnAudioMixerLoaded(AsyncOperationHandle<AudioMixer> handle)
    //{
    //    if (handle.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        audioMixer = handle.Result;
    //        audioSources[0].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[1];
    //        audioSources[1].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[2];
    //    }
    //}

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudoiClip(path, type);
        Play(audioClip, type, pitch);
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.Bgm)
        {

            AudioSource source = audioSources[(int)Define.Sound.Bgm];

            if (source.isPlaying)
            {
                source.Stop();
            }

            source.pitch = pitch;
            source.clip = audioClip;
            source.Play();
        }
        else
        {
            AudioSource source = audioSources[(int)Define.Sound.Effect];
            source.pitch = pitch;
            source.PlayOneShot(audioClip);
        }
    }
    public void isPlaying(string path, Define.Sound type = Define.Sound.Effect)
    {
        AudioClip audioClip = GetOrAddAudoiClip(path, type);
        isPlaying(audioClip, type);
    }


    public void isPlaying(AudioClip audioClip, Define.Sound type = Define.Sound.Effect)
    {
        AudioSource source = audioSources[(int)Define.Sound.Effect];

        if (source.isPlaying == false)
        {
            source.loop = true;
            Play(audioClip, type);
        }
    }

    public void StopPlaying(string path, Define.Sound type = Define.Sound.Effect)
    {
        AudioClip audioClip = GetOrAddAudoiClip(path, type);
        StopPlaying(audioClip, type);
    }


    public void StopPlaying(AudioClip audioClip, Define.Sound type = Define.Sound.Effect)
    {
        AudioSource source = audioSources[(int)Define.Sound.Effect];

        if (source.isPlaying == true)
        {
            source.loop = false;
            source.Stop();
        }
    }


    AudioClip GetOrAddAudoiClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm)
        {
            audioClip = Manager.Resource.Load<AudioClip>(path);
        }
        else
        {
            if (audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Manager.Resource.Load<AudioClip>(path);
                audioClips.Add(path, audioClip);
            }
        }

        return audioClip;
    }

    public void Clear()
    {
        foreach (AudioSource source in audioSources)
        {
            source.clip = null;
            source.Stop();
        }
        audioClips.Clear();
    }

    public AudioSource[] audioSourceEffects; // ȿ����
    public AudioSource audioSourceBgm; // ���

    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;


    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying) // ��� �� X
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[i].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play(); // ��� O
                        return;
                    }
                }
                return;
            }
        }
    }

    // ��� ���� ����
    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    // �Ϻ� ���� ����
    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
    }
    //TODO
}

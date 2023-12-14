using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
            
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }
        }
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudoiClip(path, type);
        Play(audioClip, type, pitch);
    }

    public AudioMixerGroup SettingMixer(AudioSource source, Define.Sound type)
    {
        string name;
        if (type == Define.Sound.Bgm)
            name = "Music";
        else
            name = "SFX";
        AudioMixerGroup mixer = Manager.Setting.masterAudioMixerGroup.audioMixer.FindMatchingGroups(name)[0];
        source.outputAudioMixerGroup = mixer;
        return mixer;
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
            if(source.outputAudioMixerGroup == null)
                source.outputAudioMixerGroup = SettingMixer(source, type);
            source.pitch = pitch;
            source.clip = audioClip;
            source.Play();
        }
        else
        {
            AudioSource source = audioSources[(int)Define.Sound.Effect];
            if (source.outputAudioMixerGroup == null)
                source.outputAudioMixerGroup = SettingMixer(source, type);
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
        if (source.outputAudioMixerGroup == null)
            source.outputAudioMixerGroup = SettingMixer(source, type);
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
        if (source.outputAudioMixerGroup == null)
            source.outputAudioMixerGroup = SettingMixer(source, type);
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

    //TODO
}

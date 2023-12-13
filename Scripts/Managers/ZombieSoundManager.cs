using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSoundManager : MonoBehaviour
{
    [System.Serializable]
    public class ZombieSoundData
    {
        public ZombieType zombieType;
        public AudioClip chaseSound;
        public AudioClip attackSound;
    }

    public List<ZombieSoundData> zombieSounds = new List<ZombieSoundData>();
    private Dictionary<ZombieType, ZombieSoundData> zombieSoundDictionary = new Dictionary<ZombieType, ZombieSoundData>();

    private void Awake()
    {
        foreach (ZombieSoundData data in zombieSounds)
        {
            if (!zombieSoundDictionary.ContainsKey(data.zombieType))
            {
                zombieSoundDictionary.Add(data.zombieType, data);
            }
        }
    }

    public ZombieSoundData GetZombieSoundData(ZombieType type)
    {
        if (zombieSoundDictionary.TryGetValue(type, out ZombieSoundData soundData))
        {
            return soundData;
        }
        return null;
    }
}

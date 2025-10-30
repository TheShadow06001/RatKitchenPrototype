using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]

public struct SoundInstance
{
    [SerializeField] AudioSource source;
    public SoundEffects effects;

    public void PlaySoundEffect()
    {
        source.Play();
    }
}

public enum SoundEffects
{
    RatRunning,
    RatJumping,
    RatDeath,
    RatSwim,
    RatDash,
    KnifeTrapWhoosh,
    KnifeTrapChop,
    Frying,
    BoilingWater,
    GasStoveTick,
    GasStoveFire,
    

}

public class SoundManager : MonoBehaviour
{
    

    #region Singleton

    public static SoundManager Instance;

    private void Awake()
    {
        if (SoundManager.Instance != null && Instance != this)
        {
            Debug.LogError("[SoundManager] Mutiple soundmanagers!");
            Destroy(gameObject);
            return;
        }
        Instance = this;      
    }
    #endregion

    [SerializeField] List<SoundInstance> soundInstances = new();
    
    public void PlaySoundEffect(SoundEffects anEffect)
    {
        for (int i = 0; i < soundInstances.Count; i++)
        {
           if (soundInstances[i].effects == anEffect)
            {
                soundInstances[i].PlaySoundEffect();
             return;

            } 
        }
    }

}

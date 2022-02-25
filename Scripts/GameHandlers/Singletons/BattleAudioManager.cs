using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAudioManager : MonoBehaviour
{
   public static BattleAudioManager Singleton;
   public AudioClip superEffectiveHit;
   public AudioClip lowEffectiveHit;
   AudioSource audioSource;
   void Awake()
   {
       audioSource = GetComponent<AudioSource>();
   }
   void Start()
   {
       if (Singleton == null)
       {
           Singleton = this;
       }
   }

   public void PlayAudio(AudioClip clip)
   {
       audioSource.PlayOneShot(clip);
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudManager : Singleton<AudManager>
{
    public AudioClip[] clips;
   
    public List<AudioSource> auds;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(int index)
    {
        for (int i = 0; i < auds.Count; i++)
        {
            if (!auds[i].isPlaying)
            {
                auds[i].clip = clips[index];
                auds[i].Play();
                return;
            }
        }
        AudioSource aud = gameObject.AddComponent<AudioSource>();
        aud.clip = clips[index];
        aud.Play();
        auds.Add(aud);
    }
}

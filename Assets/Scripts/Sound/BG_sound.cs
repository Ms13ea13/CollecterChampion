using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_sound : MonoBehaviour {

    public AudioClip Musicclip;
    public AudioSource source;

    void start()
    {
        source.clip = Musicclip;

    }

    void Update()
    {
        if (source.name == "")
        {

        }
        source.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_button_Manager : MonoBehaviour
{
    public AudioSource source;//
    public AudioClip hover;//
    public AudioClip click;//
                           //_____________________________________________________________

    public void OnHover()
    {
        source.PlayOneShot(hover);
    }

    public void OnClick()
    {
        source.PlayOneShot(click);
    }
}

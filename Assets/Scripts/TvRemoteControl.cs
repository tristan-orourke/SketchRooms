using UnityEngine;
using VRTK;

public class TvRemoteControl : VRTK_InteractableObject
{
    AudioSource audioSrc;
    MovieTexture movie;
    private bool markForPause = false;
    private bool markForPlay = true;

    protected override void Start()
    {
        base.Start();
        if (name == "Screen")
        {
            audioSrc = GetComponent<AudioSource>();
            movie = (MovieTexture)GetComponent<Renderer>().material.mainTexture;
        } else
        {
            GameObject screen = transform.parent.Find("Screen").gameObject;
            audioSrc = screen.GetComponent<AudioSource>();
            movie = (MovieTexture)screen.GetComponent<Renderer>().material.mainTexture;
        }
    }

    public override void StartUsing(GameObject usingObject)
    {
        base.StartUsing(usingObject);
        toggleTv();
        Debug.unityLogger.Log("Start using tv");
    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);
        toggleTv();
        Debug.unityLogger.Log("Stop using tv");
    }

    private void toggleTv()
    {
        if (movie.isPlaying)
        {
            markForPause = true;
        } else
        {
            markForPlay = true;
        }
    }

    protected override void Update()
    {
        if (markForPlay)
        {
            movie.Play();
            audioSrc.Play();
            markForPlay = false;
        }
        if (markForPause)
        {
            movie.Pause();
            audioSrc.Pause();
            markForPause = false;
        }
    }
}
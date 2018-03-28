using UnityEngine;

public class RestartAudioVideoOnSpace : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {

            Renderer r = GetComponent<Renderer>();
            MovieTexture movie = (MovieTexture)r.material.mainTexture;
            movie.loop = true;
            AudioSource audio = GetComponent<AudioSource>();
            audio.loop = true;

            movie.Stop();
            audio.Stop();
            movie.Play();
            audio.Play();
   
        }
    }
}
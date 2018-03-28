using UnityEngine;

public class PlayAudioVideoOnSpace : MonoBehaviour
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

            if (movie.isPlaying)
            {
                movie.Pause();
                audio.Pause();
            }
            else
            {
                movie.Play();
                audio.Play();
            }
        }
    }
}
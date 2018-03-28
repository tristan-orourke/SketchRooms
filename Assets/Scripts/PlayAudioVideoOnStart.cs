using UnityEngine;

public class PlayAudioVideoOnStart : MonoBehaviour
{
    void Start()
    {
        Renderer r = GetComponent<Renderer>();
        MovieTexture movie = (MovieTexture)r.material.mainTexture;

        AudioSource audio = GetComponent<AudioSource>();

        if (movie != null)
        { 
            movie.loop = true;
            if (!movie.isPlaying)
            {
                movie.Play();
                Debug.unityLogger.Log(string.Concat("Start movie ", movie.GetInstanceID().ToString()));
            }
        }
        if (audio != null)
        {
            audio.loop = true;
            if (!audio.isPlaying) {
                audio.Play();
                Debug.unityLogger.Log(string.Concat("Start AudioClip ",audio.clip.GetInstanceID().ToString(), " on source ", audio.GetInstanceID().ToString()));
                Debug.unityLogger.Log(string.Concat("Clip is playing: ", audio.isPlaying.ToString()));
            }
        }
    }
}
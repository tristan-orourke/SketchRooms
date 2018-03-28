using UnityEngine;

public class PlayVideoOnStart : MonoBehaviour
{
    void Start()
    {
        Renderer r = GetComponent<Renderer>();
        MovieTexture movie = (MovieTexture)r.material.mainTexture;

        if (movie != null)
        {
            movie.loop = true;
            if (!movie.isPlaying)
            {
                movie.Play();
                Debug.unityLogger.Log(string.Concat("Start movie ", movie.GetInstanceID().ToString()));
            }
        }
    }
}
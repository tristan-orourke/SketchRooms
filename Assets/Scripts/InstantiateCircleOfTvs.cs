using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InstantiateCircleOfTvs : MonoBehaviour
{
    public GameObject prefab;
    public float radius = 2f;
    public bool savePrefab;
    public string savedPrefabName = "circlePrefab";
    public List<MovieTexture> movie;
    public List<AudioClip> audioClips;
    private int numberOfObjects;

    void Awake()
    {
        if (movie.Count != audioClips.Count)
        {
            throw new MissingComponentException("movies and audioClips must be of the same length");
        }
        numberOfObjects = movie.Count;


        GameObject circleParent = new GameObject();
        if (prefab != this)
        {
            for (int i = 0; i < numberOfObjects; i++)
            {
                Vector3 pos = getIthPositionInCircle(i, numberOfObjects, radius) + transform.position;
                Quaternion rot = Quaternion.identity * getIthRotationInCircle(i, numberOfObjects);
                GameObject instance = (GameObject)Instantiate(prefab, pos, rot, circleParent.transform);

                GameObject screen = getChildGameObject(instance, "Screen");
                if (screen != null)
                {
                    Renderer renderer = screen.GetComponent<Renderer>();
                    renderer.material.mainTexture = movie[i];
                    ((MovieTexture)renderer.material.mainTexture).Stop();
                }

                AudioSource[] audioSources = instance.GetComponentsInChildren<AudioSource>();
                if (audioSources != null && audioSources.Length > 0)
                {
                    //audioSources[0].clip = (AudioClip.Instantiate<AudioClip>(audioClips[i]));
                    audioSources[0].clip = audioClips[i];
                    audioSources[0].Stop();
                    audioSources[0].maxDistance = radius * 2;
                    Debug.unityLogger.Log(string.Concat("Assign audio clip ", audioSources[0].clip.GetInstanceID().ToString(), " to source ", audioSources[0].GetInstanceID().ToString()));
                }
                Debug.unityLogger.Log("Prefab instanced");
            }
#if UNITY_EDITOR
            if (savePrefab)
            {

                Object prefabSave = PrefabUtility.CreateEmptyPrefab("Assets/Prefabs/" + savedPrefabName + ".prefab");
                PrefabUtility.ReplacePrefab(circleParent, prefabSave, ReplacePrefabOptions.ConnectToPrefab);
            }
#endif
        }

    }

    private Vector3 getIthPositionInCircle(int i, int numberOfObjects, float radius)
    {
        float angle = i * Mathf.PI * 2 / numberOfObjects;
        Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        return pos;
    }

    private Quaternion getIthRotationInCircle(int i, int numberOfObejcts)
    {
        float angle = 360f / numberOfObjects * i;
        return Quaternion.Euler(0, -angle, 0);
    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
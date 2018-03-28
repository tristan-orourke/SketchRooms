using UnityEngine;
using System;

public class InstantiateExample : MonoBehaviour
{
    public GameObject prefab;
    private bool cloningEnabled = true;

    void Start()
    {
        if (cloningEnabled)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject instance = (GameObject)Instantiate(prefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
                InstantiateExample ex = instance.GetComponent<InstantiateExample>();
                if (ex != null) {
                    ex.DisableCloning();
                }
            }
        }
    }

    public void DisableCloning()
    {
        cloningEnabled = false;
    }
}
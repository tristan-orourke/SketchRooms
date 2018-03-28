using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InstantiateCircleOfPrefabs : MonoBehaviour
{
    public GameObject prefab;
    public int numberOfObjects = 3;
    public float radius = 2f;
    public bool savePrefab;
    public string savedPrefabName = "circlePrefab";

    void Start()
    {
        GameObject circleParent = new GameObject();
        if (prefab != this)
        {
            for (int i = 0; i < numberOfObjects; i++) {
                Vector3 pos = getIthPositionInCircle(i, numberOfObjects, radius) + transform.position;
                Quaternion rot = Quaternion.identity * getIthRotationInCircle(i, numberOfObjects);
                GameObject instance = (GameObject)Instantiate(prefab, pos, rot, circleParent.transform);
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
}
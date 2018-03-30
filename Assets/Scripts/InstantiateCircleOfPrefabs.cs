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
        if (prefab != this)
        {
            var circleParent = GenerateCircleOfPrefabs(prefab, transform.position, numberOfObjects, radius);

            #if UNITY_EDITOR
            if (savePrefab)
            {
                Object prefabSave = PrefabUtility.CreateEmptyPrefab("Assets/Prefabs/" + savedPrefabName + ".prefab");
                PrefabUtility.ReplacePrefab(circleParent, prefabSave, ReplacePrefabOptions.ConnectToPrefab);   
            }
            #endif
        }

    }

    public static GameObject GenerateCircleOfPrefabs(GameObject prefab, Vector3 origin, int number, float radius)
    {
        var circleParent = new GameObject();
        circleParent.transform.position = origin;
        for (var i = 0; i < number; i++) {
            Vector3 pos = prefab.transform.position + GetIthPositionInCircle(i, number, radius) + origin;
            Quaternion rot = Quaternion.identity * GetIthRotationInCircle(i, number);
            var instance = Instantiate(prefab, pos, rot, circleParent.transform);
        }
        return circleParent;
    }

    private static Vector3 GetIthPositionInCircle(int i, int numberOfObjects, float radius)
    {
        float angle = i * Mathf.PI * 2 / numberOfObjects;
        Vector3 pos = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius;
        return pos;
    }

    private static Quaternion GetIthRotationInCircle(int i, int numberOfObejcts)
    {
        float angle = 360f / numberOfObejcts * i;
        return Quaternion.Euler(0, angle, 0);
    }
}
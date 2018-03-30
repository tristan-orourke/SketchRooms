using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoomPrefab : MonoBehaviour
{

    public float PanelWidth = 11f;
    public float PanelHeight = 8.5f;
    [Tooltip("Should be 1x1 unit square")] public GameObject PanelPrefab;
    [Tooltip("Should be 1x1 unit square")] public GameObject FloorPrefab;
    [Tooltip("Should be 1x1 unit square")] public GameObject CeilingPrefab;
    public string RoomFolderPath = "Assets/Rooms/TestRoom";


	// Use this for initialization
	void Start ()
	{
	    var room = GenerateRoom(RoomFolderPath, PanelWidth, PanelHeight, PanelPrefab, FloorPrefab, CeilingPrefab, transform.position);
        room.transform.SetParent(transform);
	}

    public static GameObject GenerateRoom(string roomFolderPath, float panelWidth, float panelHeight, GameObject panelPrefab, GameObject floorPrefab, GameObject ceilingPrefab, Vector3 origin)
    {
        var n = 5;
        var r = (float) CalculateApothem(n, panelWidth);
        var outerRadius = (float) CalculateOuterRadius(n, panelWidth);

        //Create copy of panel prefab that can be modified
        var panel = Instantiate(panelPrefab);

        //Set Panel Dimensions
        panel.transform.localScale = new Vector3(panelWidth, panelHeight, 1);
        //Raise panel so its sitting on the floor
        panel.transform.Translate(Vector3.up * panelHeight/2);
  
        //Instantiate room object with panel walls
        var room = InstantiateCircleOfPrefabs.GenerateCircleOfPrefabs(panel, origin, n, r);

        //Destroy panel, or it will be left in scene
        Destroy(panel);

        //Instantiate floor and ceiling as children of room object
        var floor = Instantiate(floorPrefab, origin, floorPrefab.transform.rotation, room.transform);
        var ceiling = Instantiate(ceilingPrefab, origin, ceilingPrefab.transform.rotation, room.transform);
        //Set Floor and Ceiling dimensions
        floor.transform.localScale = new Vector3(outerRadius * 0.2f, 1, outerRadius * 0.2f);
        ceiling.transform.localScale = new Vector3(outerRadius * 0.2f, 1, outerRadius * 0.2f);
        //Move ceiling into position
        ceiling.transform.localPosition = (Vector3.up * panelHeight);

        return room;
    }

    public static double CalculateApothem(int numberOfSides, float lengthOfSide)
    {
        return lengthOfSide / (2 * Math.Tan(Math.PI / numberOfSides));
    }

    public static double CalculateOuterRadius(int numberOfSides, float lengthOfSide)
    {
        return lengthOfSide / (2 * Math.Sin(Math.PI / numberOfSides));
    }
}

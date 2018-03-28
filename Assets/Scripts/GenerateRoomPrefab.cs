using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoomPrefab : MonoBehaviour
{

    public GameObject PanelPrefab;
    public string RoomFolderPath = "Assets/Rooms/TestRoom";


	// Use this for initialization
	void Start ()
	{
	    var room = GenerateRoom(RoomFolderPath, PanelPrefab, transform.position);
        room.transform.SetParent(transform);
	}

    public static GameObject GenerateRoom(string roomFolderPath, GameObject panelPrefab, Vector3 origin)
    {
        var room = InstantiateCircleOfPrefabs.GenerateCircleOfPrefabs(panelPrefab, origin, 5, 20);
        return room;
    }
}

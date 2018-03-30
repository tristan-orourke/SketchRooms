using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoomPrefab : MonoBehaviour
{

    public double PanelWidth;
    public double PanelHeight;
    public GameObject PanelPrefab;
    public string RoomFolderPath = "Assets/Rooms/TestRoom";


	// Use this for initialization
	void Start ()
	{
	    var room = GenerateRoom(RoomFolderPath, PanelWidth, PanelHeight, PanelPrefab, transform.position);
        room.transform.SetParent(transform);
	}

    public static GameObject GenerateRoom(string roomFolderPath, double panelWidth, double panelHeight, GameObject panelPrefab, Vector3 origin)
    {
        var n = 5;
        var r = (float) CalculateApothem(n, panelWidth);
        var room = InstantiateCircleOfPrefabs.GenerateCircleOfPrefabs(panelPrefab, origin, n, r);
        return room;
    }

    public static double CalculateApothem(int numberOfSides, double lengthOfSide)
    {
        return lengthOfSide / (2 * Math.Tan(Math.PI / numberOfSides));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GenerateRoomSetPrefab : MonoBehaviour {

    public float PanelWidth = 11f;
    public float PanelHeight = 8.5f;
    [Tooltip("Should be 1x1 unit square")] public GameObject PanelPrefab;
    [Tooltip("Should be 1x1 unit square")] public GameObject FloorPrefab;
    [Tooltip("Should be 1x1 unit square")] public GameObject CeilingPrefab;
    public string RoomsDirectory = "Assets/Rooms";
    public GameObject PlayerPrefab;
    public string startingRoom;

    // Use this for initialization
    void Start ()
    {
        GenerateRoomSet(RoomsDirectory, PanelWidth, PanelHeight, PanelPrefab, FloorPrefab, CeilingPrefab, PlayerPrefab, startingRoom);
    }

    public static GameObject GenerateRoomSet(string roomsDirectory, float panelWidth, float panelHeight,
        GameObject panelPrefab, GameObject floorPrefab, GameObject ceilingPrefab, GameObject playerPrefab, string startingRoom)
    {
        var roomNames = Directory.GetDirectories(roomsDirectory).Select(Path.GetFileName);

        var roomSet = new GameObject();
        roomSet.name = roomsDirectory;

        var roomPosition = roomSet.transform.position;

        var playerPlaced = false;

        foreach (var roomName in roomNames)
        {
            var roomWidth = CalculateRoomOuterRadius(roomsDirectory, roomName, panelWidth) * 2;

            //adjust this room's position to avoid overlap
            roomPosition += Vector3.forward * roomWidth;

            Debug.unityLogger.Log("Next origin " + roomPosition);
            //Create room
            var room = GenerateRoomPrefab.GenerateRoom(roomsDirectory, roomName, panelWidth, panelHeight, panelPrefab,
                floorPrefab, ceilingPrefab, roomPosition);
            //Add room to set
            room.transform.SetParent(roomSet.transform);

            //Instantiate player in room if its the correct starting room
            if (roomName.Equals(startingRoom))
            {
                var player = Instantiate(playerPrefab, roomPosition, Quaternion.identity);
                playerPlaced = true;
            }

            //adjust next room's position to avoid overlap
            roomPosition += Vector3.forward * roomWidth;
        }

        if (!playerPlaced)
        {
            throw new ArgumentException("startingRoom must name a valid room to start player in.");
        }

        return roomSet;
    }

    public static float CalculateRoomOuterRadius(string roomsDirectory, string roomName, float panelWidth)
    {
        var roomPath = Path.Combine(roomsDirectory, roomName);
        var imagePaths = GenerateRoomPrefab.GetImagePaths(roomPath);

        var n = imagePaths.Length;
        var r = (float) GenerateRoomPrefab.CalculateApothem(n, panelWidth);
        return r;
    }
}

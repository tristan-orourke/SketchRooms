using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GenerateRoomPrefab : MonoBehaviour
{
    [Tooltip("px")] public int ImageWidth = 1000;
    [Tooltip("px")] public int ImageHeight = 1000;
    public float PanelWidth = 11f;
    public float PanelHeight = 8.5f;
    [Tooltip("Should be 1x1 unit square")] public GameObject PanelPrefab;
    [Tooltip("Should be 1x1 unit square")] public GameObject FloorPrefab;
    [Tooltip("Should be 1x1 unit square")] public GameObject CeilingPrefab;
    public string RoomsDirectory = "Assets/Rooms";
    public string RoomName = "TestRoom";


	// Use this for initialization
	void Start ()
	{
	    var room = GenerateRoom(RoomsDirectory, RoomName, ImageWidth, ImageHeight, PanelWidth, PanelHeight, PanelPrefab, FloorPrefab, CeilingPrefab, transform.position);
        room.transform.SetParent(transform);
	}

    public static GameObject GenerateRoom(string roomsDirectory, string roomName, int imgWidth, int imgHeight, float panelWidth, float panelHeight, GameObject panelPrefab, GameObject floorPrefab, GameObject ceilingPrefab, Vector3 origin)
    {
        var roomPath = Path.Combine(roomsDirectory, roomName);
        var imagePaths = GetImagePaths(roomPath);

        var n = imagePaths.Length;
        var r = (float) CalculateApothem(n, panelWidth);

        //Create copy of panel prefab that can be modified
        var panel = Instantiate(panelPrefab);

        //Set Panel Dimensions
        panel.transform.localScale = new Vector3(panelWidth, panelHeight, 1);
        //Raise panel so its sitting on the floor
        panel.transform.Translate(Vector3.up * panelHeight/2);
  
        //Instantiate room object with panel walls
        var room = InstantiateCircleOfPrefabs.GenerateCircleOfPrefabs(panel, origin, n, r);
        room.name = roomName;

        //Add images to room panels
        AddImagesToRoomPanels(room, imagePaths, imgWidth, imgHeight);

        //Destroy panel, or it will be left in scene
        Destroy(panel);

        AddFloorAndCeling(panelWidth, panelHeight, floorPrefab, ceilingPrefab, n, room);

        return room;
    }

    private static void AddImagesToRoomPanels(GameObject room, string[] imagePaths, int imgWidth, int imgHeight)
    {
        if (imagePaths.Length != room.transform.childCount)
        {
            throw new ArgumentException("imagePaths.Length must be equal to number of children of room object.");
        }

        for (var i = 0; i < imagePaths.Length; i++)
        {
            var panel = room.transform.GetChild(i);
            var imagePath = imagePaths[i];

            //Set panel object name
            var imageName = Path.GetFileNameWithoutExtension(imagePath);
            panel.name = Path.Combine(room.name, imageName);

            panel.GetComponent<Renderer>().material = GenerateMaterialFromImagePath(imagePath, imgWidth, imgHeight);
        }
    }

    private static Material GenerateMaterialFromImagePath(string imagePath, int width, int height)
    {
        var texture = new Texture2D(width, height);
        var imgData = File.ReadAllBytes(imagePath);
        texture.LoadImage(imgData);

        Material material = new Material(Shader.Find("Sprites/Default"));
        material.SetTexture("texture", texture);
        material.mainTexture = texture;

        return material;
    }

    private static void AddFloorAndCeling(float panelWidth, float panelHeight, GameObject floorPrefab,
        GameObject ceilingPrefab, int numberOfSides, GameObject room)
    {
        var outerRadius = (float) CalculateOuterRadius(numberOfSides, panelWidth);
        //Instantiate floor and ceiling as children of room object
        var floor = Instantiate(floorPrefab, room.transform.position, floorPrefab.transform.rotation, room.transform);
        var ceiling = Instantiate(ceilingPrefab, room.transform.position, ceilingPrefab.transform.rotation, room.transform);
        //Set Floor and Ceiling dimensions
        floor.transform.localScale = new Vector3(outerRadius * 0.2f, 1, outerRadius * 0.2f);
        ceiling.transform.localScale = new Vector3(outerRadius * 0.2f, 1, outerRadius * 0.2f);
        //Move ceiling into position
        ceiling.transform.localPosition = (Vector3.up * panelHeight);
    }

    public static double CalculateApothem(int numberOfSides, float lengthOfSide)
    {
        return lengthOfSide / (2 * Math.Tan(Math.PI / numberOfSides));
    }

    public static double CalculateOuterRadius(int numberOfSides, float lengthOfSide)
    {
        return lengthOfSide / (2 * Math.Sin(Math.PI / numberOfSides));
    }

    public static string[] GetImagePaths(string roomDirectory)
    {
        var imagePaths = Directory.GetFiles(roomDirectory);

        //filter out meta files
        return imagePaths.Where(image => !image.EndsWith(".meta")).ToArray();
    }
}

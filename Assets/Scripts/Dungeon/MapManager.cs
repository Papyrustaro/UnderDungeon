using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;

    [SerializeField] private GameObject roadMass;
    [SerializeField] private GameObject wallMass;
    [SerializeField] private GameObject playerOnRoadMass;

    [SerializeField] private GameObject cameraObject;

    private List<GameObject> mapData = new List<GameObject>();

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        MoveCamara();
    }

    private void MoveCamara()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetAxis("Horizontal") > 0) this.cameraObject.transform.Translate(1, 0, 0);
            if (Input.GetAxis("Horizontal") < 0) this.cameraObject.transform.Translate(-1, 0, 0);
            if (Input.GetAxis("Vertical") > 0) this.cameraObject.transform.Translate(0, 1, 0);
            if (Input.GetAxis("Vertical") < 0) this.cameraObject.transform.Translate(0, -1, 0);
        }
    }
    private void GenerateMap()
    {
        for(int i = 0; i < this.mapWidth; i++)
        {
            for(int j = 0; j < this.mapHeight; j++)
            {
                if(i == 0 || i == this.mapWidth-1 || j == 0 || j == this.mapHeight - 1)
                {
                    this.mapData.Add(Instantiate(this.wallMass, new Vector3((float)i, (float)j, 0f), Quaternion.identity));
                }
                else
                {
                    if(UnityEngine.Random.Range(0, 3) == 0)
                    {
                        this.mapData.Add(Instantiate(this.wallMass, new Vector3((float)i, (float)j, 0f), Quaternion.identity));
                    }
                    else
                    {
                        this.mapData.Add(Instantiate(this.roadMass, new Vector3((float)i, (float)j, 0f), Quaternion.identity));
                    }
                }
            }
        }
    }
}

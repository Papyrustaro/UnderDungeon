using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;

    /*[SerializeField] private GameObject roadMass;
    [SerializeField] private GameObject wallMass;
    [SerializeField] private GameObject playerOnRoadMass;*/

    [SerializeField] private GameObject cameraObject;

    [SerializeField] private DungeonSquare wallSquare;

    /// <summary>
    /// 出現する可能性のあるマスイベントクラス
    /// </summary>
    [SerializeField] private List<DungeonSquare> mayApeearDungeonSquares = new List<DungeonSquare>();

    /// <summary>
    /// 出現する可能性のあるマスイベントの種類
    /// </summary>
    private List<E_DungeonSquareType> mayApeearDungeonSquareTypes = new List<E_DungeonSquareType>();


    private List<GameObject> mapData = new List<GameObject>();

    /// <summary>
    /// マップ横のマス目数(row)
    /// </summary>
    public int MapWidth => this.mapWidth;

    /// <summary>
    /// マップ縦のマス目数(column)
    /// </summary>
    public int MapHeight => this.mapHeight;


    private void Awake()
    {
        SetMayApeearDungeonSquareTypes();
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        //MoveCamara();
    }

    /// <summary>
    /// マスイベントクラス群から、マスイベント種類群を生成
    /// </summary>
    private void SetMayApeearDungeonSquareTypes()
    {
        this.mayApeearDungeonSquareTypes = new List<E_DungeonSquareType>();
        foreach(DungeonSquare ds in this.mayApeearDungeonSquares)
        {
            if (this.mayApeearDungeonSquareTypes.Contains(ds.SquareType))
            {
                Debug.Log("error: 同じマスタイプが複数あります");
            }
            else
            {
                this.mayApeearDungeonSquareTypes.Add(ds.SquareType);
            }
        }
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
    public void GenerateFloor(E_DungeonSquareType[,] currentFloorDungeonSquares)
    {
        currentFloorDungeonSquares = new E_DungeonSquareType[mapWidth, mapHeight];
        int countOfDungeonSquaresKind = this.mayApeearDungeonSquares.Count;
        for(int i = 0; i < this.mapWidth; i++)
        {
            for(int j = 0; j < this.mapHeight; j++)
            {
                currentFloorDungeonSquares[i, j] = this.mayApeearDungeonSquareTypes[UnityEngine.Random.Range(0, countOfDungeonSquaresKind)];
            }
        }
        /*for(int i = 0; i < this.mapWidth; i++)
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
        }*/
    }

    public void SetFlagUnderstandDungeonSquareType(bool[,] understandDungeonSquareTypes, bool flag)
    {
        understandDungeonSquareTypes = new bool[mapWidth, mapHeight];
        if (flag)
        {
            for(int i = 0; i < mapWidth; i++)
            {
                for(int j = 0; j < mapHeight; j++)
                {
                    understandDungeonSquareTypes[i, j] = true;
                }
            }
        }
    }

    
}

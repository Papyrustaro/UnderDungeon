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

    //[SerializeField] private GameObject cameraObject;

    [SerializeField] private DungeonSquare wallSquare;

    /// <summary>
    /// 出現する可能性のあるマスイベントクラス
    /// </summary>
    [SerializeField] private List<DungeonSquare> mayAppearDungeonSquares = new List<DungeonSquare>();

    /// <summary>
    /// 出現する可能性のあるマスイベントの種類
    /// </summary>
    private List<E_DungeonSquareType> mayAppearDungeonSquareTypes = new List<E_DungeonSquareType>();


    private List<GameObject> mapData = new List<GameObject>();

    /// <summary>
    /// マップ横のマス目数(row)
    /// </summary>
    public int MapWidth => this.mapWidth;

    /// <summary>
    /// マップ縦のマス目数(column)
    /// </summary>
    public int MapHeight => this.mapHeight;

    public List<DungeonSquare> MayAppearDungeonSquares => this.mayAppearDungeonSquares;

    public List<E_DungeonSquareType> MayAppearDungeonSquareTypes => this.mayAppearDungeonSquareTypes;

    public static MapManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);


        SetMayAppearDungeonSquareTypes();
    }

    private void Update()
    {
        //MoveCamara();
    }

    /// <summary>
    /// マスイベントクラス群から、マスイベント種類群を生成
    /// </summary>
    private void SetMayAppearDungeonSquareTypes()
    {
        this.mayAppearDungeonSquareTypes = new List<E_DungeonSquareType>();
        foreach(DungeonSquare ds in this.mayAppearDungeonSquares)
        {
            if (this.mayAppearDungeonSquareTypes.Contains(ds.SquareType))
            {
                Debug.Log("error: 同じマスタイプが複数あります");
            }
            else
            {
                this.mayAppearDungeonSquareTypes.Add(ds.SquareType);
            }
        }
    }

    /// <summary>
    /// ランダムでマスタイプ設置
    /// </summary>
    /// <param name="currentFloorDungeonSquares">マスタイプ保持配列</param>
    public void GenerateFloor(E_DungeonSquareType[,] currentFloorDungeonSquares)
    {
        currentFloorDungeonSquares = new E_DungeonSquareType[mapWidth, mapHeight];
        int countOfDungeonSquaresKind = this.mayAppearDungeonSquares.Count;
        for(int i = 0; i < this.mapWidth; i++)
        {
            for(int j = 0; j < this.mapHeight; j++)
            {
                currentFloorDungeonSquares[i, j] = this.mayAppearDungeonSquareTypes[UnityEngine.Random.Range(0, countOfDungeonSquaresKind)];
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

    public void SetFlagUnderstandDungeonSquareType(ref bool[,] understandDungeonSquareTypes, bool flag)
    {
        if (flag) Debug.Log("マップ全体可視化");
        else Debug.Log("マップ全体忘却");

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

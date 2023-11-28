using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BoardManager : MonoBehaviour
{
    //The number of each tile ranges from 1 - 12
    //Tile 0 is the Start Tile while Tile -1 is the Goal/End Tile

    [SerializeField] private Tile[] m_Tiles;
    [SerializeField] private Tile m_StartTile;
    [SerializeField] private Tile m_DestinationTiLe;

    [SerializeField] private GameObject LevelWonScreen;
    public bool LevelWon;
    
    int [,] Grid = new int [4,3] {
        {0, 1, 2} ,   /*  initializers for row indexed by 0 */
        {3, 4, 5} ,   /*  initializers for row indexed by 1 */
        {7, 8, 9} ,   /*  initializers for row indexed by 2 */
        {10, 11, 12} /*  initializers for row indexed by 3 */
    };
    

    [SerializeField] private float m_RotationDuration;

    List<int[]> m_PossibleMoves = new List<int[]>();

    private static BoardManager m_Instance;
    private int m_Index;
    
    int maxRow = 3;
    int maxColumn = 2;
    
    public static BoardManager Instance
    {
        get { return m_Instance; }
        set => m_Instance = value;
    }
    
    void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnLevelComplete()
    {
        LevelWon = true;
        LevelWonScreen.SetActive(true);
        Debug.Log("LEVEL COMPLETE, LETS GOOOOO");
    }
    
    void Start()
    {
        SetTiles();
    }

    public void SetTiles()
    {
        m_Index = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int[] TilePosition = { i, j };
                m_Tiles[m_Index].SetTileNumber(TilePosition);
                m_Index += 1;
            }
        }
    }

    public float GetRotationTime()
    {
        return m_RotationDuration;
    }

    public void GetPossibleMoves(int[] currentLocation, eTileTypes tileType)
    {
        int Row = currentLocation[0];
        int Column = currentLocation[1];
        
        m_PossibleMoves.Clear();

        int[] NeighboringTile1 = new int[2];
        
        if (tileType == eTileTypes.Start)
        {
            NeighboringTile1[0] = 0;
            NeighboringTile1[1]= 1;
            m_PossibleMoves.Add(NeighboringTile1);
        }
        else if (tileType == eTileTypes.Path)
        {
            //Regardless of Grid size, the max amount of tiles one can travel will always be 4.
            if (Row + 1 <= maxRow)
            {
                NeighboringTile1[0] = Row + 1;
                NeighboringTile1[1]= Column;
                m_PossibleMoves.Add(NeighboringTile1);
            }

             if (Row - 1 >= 0)
             {
                int[] NeighboringTile2 = new int[2];
                NeighboringTile2[0] = Row - 1;
                NeighboringTile2[1]= Column;
                m_PossibleMoves.Add(NeighboringTile2);
             }

             if (Column + 1 <= maxColumn)
             {
                int[] NeighboringTile3 = new int[2];
                NeighboringTile3[0] = Row;
                NeighboringTile3[1]= Column + 1;
                m_PossibleMoves.Add(NeighboringTile3);
             }

             if (Column - 1 >= 0)
             {
                int[] NeighboringTile4 = new int[2];
                NeighboringTile4[0] = Row ;
                NeighboringTile4[1]= Column - 1;
                m_PossibleMoves.Add(NeighboringTile4);
             }

            if (Row == 3 && Column == 1)
            {
                m_PossibleMoves.Add(m_DestinationTiLe.GetTileNumber());
            }
        }
    }

    public bool IsValidTile(Tile currentTile, Tile targetTile)
    {
        int[] tileNumber = currentTile.GetTileNumber();
        GetPossibleMoves(tileNumber,currentTile.GetTileType());
        
        for (int i = 0; i < m_PossibleMoves.Count(); i++)
        {
            bool isSameRow = m_PossibleMoves[i][0] == targetTile.GetTileNumber()[0];
            bool isSameColumn = m_PossibleMoves[i][1] == targetTile.GetTileNumber()[1];
            bool isConnected = currentTile.m_ConnectedTiles.Contains(targetTile);
            if (isSameRow && isSameColumn && isConnected) return true;
        }
        return false;
    }
}

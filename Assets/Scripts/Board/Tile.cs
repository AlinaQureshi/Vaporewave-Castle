using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eTileTypes
{
    Start,
    Path,
    Goal
}

public class Tile : MonoBehaviour
{
    [SerializeField] private eTileTypes m_TileType;
    [SerializeField] public Tile[] m_ConnectedTiles;


    //1x2 array, where the first number represents row, and second represents column.
    int[] m_TileNumber = new int[2]; 

    private bool m_CanMove;
    private bool m_IsRotating;    
    
    private float m_RotationDuration;
    private Vector3 m_CenterPoint;

    private void Start()
    { 
        m_RotationDuration = BoardManager.Instance.GetRotationTime();
        m_CenterPoint = this.gameObject.GetComponent<Renderer>().bounds.center;
    }

    private void Update()
    {
        if (m_CanMove && !m_IsRotating)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(RotateAround(-1));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(RotateAround(1));
            }
        }
    }

    public void SetTileNumber(int[] index)
    {
        m_TileNumber = index;
    }


    IEnumerator RotateAround(int direction)
     {
         m_IsRotating = true;
         
         float TimeDelta= 0;
         float currentTime = 0.0f;
         
         float angle = direction == 1 ? 90.0f : -90.0f;
         float angleDelta = angle/m_RotationDuration; //How many degrees to rotate per second
         
         
         while (currentTime < m_RotationDuration)
         {
             currentTime += Time.deltaTime;
             TimeDelta = Time.deltaTime;
             
             if (currentTime > m_RotationDuration)
             {
                 TimeDelta-= (currentTime-m_RotationDuration);
             }
                 
             this.transform.RotateAround(m_CenterPoint, Vector3.up,angleDelta * TimeDelta);
             yield return null;
         }

         m_IsRotating = false;
     }

     public eTileTypes GetTileType()
     {
         return m_TileType;
     }
     
     
     private void OnTriggerStay(Collider other)
     {
         if (other.gameObject.CompareTag("Player"))
         {
             switch (m_TileType)
             {
                case eTileTypes.Path:
                    if (!m_CanMove)
                    {
                        m_CanMove = true;
                    }
                    break;
                case eTileTypes.Goal:
                    if (!BoardManager.Instance.LevelWon)
                    {
                        BoardManager.Instance.OnLevelComplete();
                    }
                    break;
             }
         }
     }
     
     private void OnTriggerExit(Collider other)
     {
         if (other.gameObject.CompareTag("Player") && m_TileType == eTileTypes.Path)
         {
             switch (m_TileType)
             {
                 case eTileTypes.Path:
                     m_CanMove = false;
                     break;
             }
         }
     }

     public int[] GetTileNumber()
     {
         return m_TileNumber;
     }
     
}

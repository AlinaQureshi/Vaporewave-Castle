using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathConnector : MonoBehaviour
{
    [SerializeField] public Tile m_Tile;
    [SerializeField] private int m_SavedTileIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PathCollider"))
        {
            for (int i = 0; i < m_Tile.m_ConnectedTiles.Length; i++)
            {
                if (!m_Tile.m_ConnectedTiles[i])
                {
                    m_Tile.m_ConnectedTiles[i] = other.GetComponent<PathConnector>().m_Tile;
                    m_SavedTileIndex = i;
                    return;
                }
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PathCollider"))
        {
            m_Tile.m_ConnectedTiles[m_SavedTileIndex] = null;
        }
    }

    
}

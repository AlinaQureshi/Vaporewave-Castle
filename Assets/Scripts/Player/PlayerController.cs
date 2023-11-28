using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Tile m_CurrentTile;
    [SerializeField] float m_MoveSpeed = 5f;

    private bool m_IsMoving;
    private Vector3 m_TargetPosition;
    
    private static PlayerController m_Instance;

    public static PlayerController Instance
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Tile") && !m_IsMoving )
                {
                    Tile targetTile = hit.transform.GetComponent<Tile>();
                    if (BoardManager.Instance.IsValidTile(m_CurrentTile, targetTile))
                    {
                        m_TargetPosition = hit.transform.gameObject.GetComponent<Renderer>().bounds.center;
                        m_TargetPosition = new Vector3(m_TargetPosition.x, this.transform.position.y, m_TargetPosition.z);
                        m_CurrentTile = targetTile;
                        MoveToDestination();
                    }               
                }
            }
        }
    }

    private void MoveToDestination() {
        if (m_IsMoving) return;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        m_IsMoving = true;
        while (this.transform.position != m_TargetPosition)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, m_TargetPosition, m_MoveSpeed * Time.deltaTime);
            yield return 0;
        }
        m_IsMoving = false;
    }

}

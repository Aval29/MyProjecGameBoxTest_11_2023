using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour 
{
    private Camera _mainCamera;
    private NavMeshAgent _Player;

    void Start()
    {
        _mainCamera = Camera.main;
        _Player = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit _hit; /// перемешения игрока
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                _Player.SetDestination(_hit.point);
            }
        }
    }

}

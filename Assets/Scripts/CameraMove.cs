using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    private Vector3 _startPosition;
    private Vector3 _nowPosition;

    
    void Start()
    {
        _startPosition = _player.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        _nowPosition = _player.transform.position;
        if(_nowPosition!= _startPosition)
        {
            var _cameraPosition = Camera.main.transform.position;
            Camera.main.transform.position = _cameraPosition - (_startPosition - _nowPosition);
            _startPosition = _nowPosition;
        }
    }

}

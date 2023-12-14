using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInWorldMap : MonoBehaviour
{
    Transform _player;
    public float heightAbovePlayer = 50.0f; 

    void Start()
    {
        _player = Player.Instance.gameObject.transform;
    }

    void LateUpdate()
    {
        if (_player != null)
        {
            Vector3 newPosition = _player.position + Vector3.up * heightAbovePlayer;
            transform.position = newPosition;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3Control : PlayerBase
{
    void Start ()
    {
        _playercontrol = GetComponentInParent<CharacterController>();
        _rigibody = GetComponentInParent<Rigidbody>();
    }
	
	void Update ()
    {
        Vector3 move = new Vector3(Input.GetAxis("HorizontalJoy2"), 0, Input.GetAxis("VerticalJoy2"));

        if (_playercontrol.isGrounded)
        {
            _velocity.y = 0f;

            if (move != Vector3.zero)
            {
                transform.forward = move;
            }
        }

        _velocity.x /= 1 + _drag.x * Time.deltaTime;
        _velocity.y /= 1 + _drag.y * Time.deltaTime;
        _velocity.z /= 1 + _drag.z * Time.deltaTime;
        _velocity.y -= _gravity * Time.deltaTime;

        _playercontrol.Move(_velocity * Time.deltaTime + move * Time.deltaTime * _speed);
    }
}

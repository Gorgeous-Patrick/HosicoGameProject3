using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
  [SerializeField] GameObject container;
    [SerializeField] float rotationSpd;

  PlayerActions _playerActions;
  Rigidbody _rbody;
  Vector2 _moveInput;

  void Awake()
  {
    _playerActions = new PlayerActions();
    _rbody = container.GetComponent<Rigidbody>();
    if (_rbody is null)
    {
      Debug.LogError("Rigidbody2D is NULL!");
    }
    }

  void OnEnable()
  {
    _playerActions.Player_Map.Enable();
  }

  void OnDisable()
  {
    _playerActions.Player_Map.Disable();
  }


  void FixedUpdate()
  {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(horizontalInput, 0, verticalInput);
        moveDir.Normalize();
    _moveInput = _playerActions.Player_Map.Movement.ReadValue<Vector2>();
    _rbody.velocity = new Vector3(_moveInput.x, 0, _moveInput.y) * speed;

        if(_rbody.velocity != Vector3.zero)
        {
            Quaternion rotTowards = Quaternion.LookRotation(moveDir, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTowards, rotationSpd * Time.deltaTime);
        }
    }

    
}

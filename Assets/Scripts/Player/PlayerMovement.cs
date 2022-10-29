using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField] float speed;
  [SerializeField] GameObject container;

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
    _moveInput = _playerActions.Player_Map.Movement.ReadValue<Vector2>();
    _rbody.velocity = new Vector3(_moveInput.x, 0, _moveInput.y) * speed;
  }
}

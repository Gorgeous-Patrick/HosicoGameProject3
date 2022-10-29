using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed;
    private PlayerActions _playerActions;
    private Rigidbody _rbody;
    private Vector2 _moveInput;

    void Awake() {
        _playerActions = new PlayerActions();
        _rbody = GetComponent<Rigidbody>();
        if (_rbody is null) {
            Debug.LogError("Rigidbody2D is NULL!");
        }
    }


    private void OnEnable() {
        _playerActions.Player_Map.Enable();
    }

    private void OnDisable() {
        _playerActions.Player_Map.Disable();
    }


    private void FixedUpdate()
    {
        _moveInput = _playerActions.Player_Map.Movement.ReadValue<Vector2>();
        // _moveInput.y = 0f;
        _rbody.velocity = new Vector3(_moveInput.x, 0, _moveInput.y) * _speed;
    }

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{

  PlayerInput _playerInput;
  [SerializeField] GameObject _player;

  static public PlayerInput playerInput
  {
    get
    {
      return instance._playerInput;
    }
  }

  static public GameObject player
  {
    get
    {
      return instance._player;
    }
  }

  void OnEnable()
  {
    playerInput.Gameplay.Enable();
  }

  void OnDisable()
  {
    playerInput.Gameplay.Disable();
  }

  void Awake()
  {
    print("initializating gameplay");
    if (_instance != null && _instance != this)
      Destroy(this.gameObject);
    else
      _instance = this;
    _playerInput = new PlayerInput();
  }

  // Singleton
  static Gameplay _instance;
  public static Gameplay instance
  {
    get
    {
      return _instance;
    }
  }


  static public Vector2 playerLookDirection
  {
    get
    {
      return Camera.main.ScreenToWorldPoint(playerInput.Gameplay.Look.ReadValue<Vector2>())
        - player.transform.position;
    }
  }

}

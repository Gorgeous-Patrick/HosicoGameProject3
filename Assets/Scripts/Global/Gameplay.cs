using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{

  PlayerInput _playerInput;
  [SerializeField] GameObject _player;
  [SerializeField] float batteryDrainInterval = 7, batteryChargeInterval = 0.1f;
  float _batteryLevel;
  Coroutine batteryDrainCoroutine, batteryChargeCoroutine;

  // triggered when player presses E to interact with objects in the scene
  public System.Action funcUseItem;

  static public float batteryLevel
  {
    get
    {
      return instance._batteryLevel;
    }
  }
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
    if (_instance != null && _instance != this)
      Destroy(this.gameObject);
    else
      _instance = this;
    _playerInput = new PlayerInput();
  }

  void Start()
  {
    _batteryLevel = 1;
    batteryDrainCoroutine = StartCoroutine(coroutine_batteryDrain());
    EventBus.Subscribe<EventHeadlightStatusChange>(handler_EventHeadlightStatusChange);
  }

  void handler_EventHeadlightStatusChange(EventHeadlightStatusChange e)
  {
    if (e.enabled == true && batteryDrainCoroutine == null)
    {
      batteryDrainCoroutine = StartCoroutine(coroutine_batteryDrain());
      return;
    }
    if (e.enabled == false && batteryDrainCoroutine != null)
    {
      StopCoroutine(batteryDrainCoroutine);
      batteryDrainCoroutine = null;
      return;
    }
  }

  IEnumerator coroutine_batteryDrain()
  {
    while (_batteryLevel > 0)
    {
      yield return new WaitForSeconds(batteryDrainInterval);
      _batteryLevel -= 0.01f;
    }
    // the player failed...
    EventBus.Publish(new EventFailure());
  }

  IEnumerator coroutine_batteryCharge()
  {
    while (_batteryLevel < 1)
    {
      yield return new WaitForSeconds(batteryChargeInterval);
      _batteryLevel += 0.03f;
    }
    if (_batteryLevel > 1) _batteryLevel = 1;
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

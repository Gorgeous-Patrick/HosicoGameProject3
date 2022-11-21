using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameplay : MonoBehaviour
{

  PlayerInput _playerInput;

  [SerializeField] GameObject _player;
  [SerializeField] GameObject _destination;

  [SerializeField] float batteryDrainInterval = 7, batteryChargeInterval = 0.1f;
  [SerializeField] float maxBattery = 1;

  [SerializeField] float _batteryLevel = 0;
  Coroutine batteryDrainCoroutine, batteryChargeCoroutine;

  // triggered when player presses E to interact with objects in the scene
  System.Action _funcInteract;

  static public float batteryLevel
  {
    get => instance._batteryLevel;
  }
  static public PlayerInput playerInput
  {
    get => instance._playerInput;
  }
  static public GameObject player
  {
    get => instance._player;
  }

  static public GameObject destination
  {
    get => instance._destination;
  }
  static public System.Action funcInteract
  {
    get => instance._funcInteract;
    set => instance._funcInteract = value;
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
    EventBus.Subscribe<EventFailure>(handler_EventFailure);
  }

  void Start()
  {
    if (_batteryLevel == 0)
      _batteryLevel = maxBattery;
    else maxBattery = _batteryLevel;
    batteryDrainCoroutine = StartCoroutine(coroutine_batteryDrain());
    EventBus.Subscribe<EventHeadlightStatusChange>(handler_EventHeadlightStatusChange);
    EventBus.Subscribe<EventBatteryStatusChange>(haandler_EventBatteryStatusChange);
    EventBus.Subscribe<OnChangeGoal>(handler_EventChangeGoal);
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

  void haandler_EventBatteryStatusChange(EventBatteryStatusChange e)
  {
    if (e.charging == true && batteryChargeCoroutine == null)
    {
      batteryChargeCoroutine = StartCoroutine(coroutine_batteryCharge());
      return;
    }
    if (e.charging == false && batteryChargeCoroutine != null)
    {
      StopCoroutine(batteryChargeCoroutine);
      batteryChargeCoroutine = null;
      return;
    }
  }

  void handler_EventChangeGoal(OnChangeGoal e)
  {
    _destination = e._nextGoal.gameObject;
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

  static public Inventory inventory
  {
    get => instance._player.GetComponent<Inventory>();
  }

  private void handler_EventFailure(EventFailure obj)
  {
    _batteryLevel = maxBattery;
    StartCoroutine(coroutine_DelayedBatteryDrain());
  }

  private IEnumerator coroutine_DelayedBatteryDrain()
  {
    yield return new WaitForSeconds(0.5f);
    batteryDrainCoroutine = StartCoroutine(coroutine_batteryDrain());
  }

}

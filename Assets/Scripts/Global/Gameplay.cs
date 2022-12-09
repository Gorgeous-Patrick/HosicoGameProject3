using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameplay : MonoBehaviour
{

  PlayerInput _playerInput;

  [SerializeField] GameObject _player;
  [SerializeField] GameObject _destination;

  // how long a single battery lasts
  [SerializeField] int batteryEndurance;
  float batteryDrainSpeed { get => 1 / (float)batteryEndurance; }

  // how long it takes to restore a battery
  [SerializeField] float batteryChargeInterval = 0.2f;

  // for how long the battery indicator blinks
  [SerializeField] float batteryBarBlinkInterval = 0.3f;

  // maximum number of batteries the player can hold
  [SerializeField] int _maxBattery = 5;

  [SerializeField] int _batterys = 0;
  float _batteryLevel;
  Coroutine batteryDrainCoroutine, batteryChargeCoroutine;

  public bool batteryDraining
  {
    get => instance.batteryDrainCoroutine != null;
  }

  // triggered when player presses E to interact with objects in the scene
  System.Action _funcInteract;

  static public float batterys
  {
    get => instance._batterys;
  }
  static public float batteryLevel
  {
    get => instance._batteryLevel;
  }
  static public int maxBattery
  {
    get => instance._maxBattery;
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
    // EventBus.Subscribe<EventFailure>(handler_EventFailure);
  }

  void Start()
  {
    if (_batterys == 0)
      _batterys = _maxBattery;
    else _maxBattery = _batterys;
    EventBus.Subscribe<EventHeadlightStatusChange>(handler_EventHeadlightStatusChange);
    EventBus.Subscribe<EventBatteryStatusChange>(handler_EventBatteryStatusChange);
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

  void handler_EventBatteryStatusChange(EventBatteryStatusChange e)
  {
    if (e.charging == true && batteryChargeCoroutine == null)
    {
      batteryChargeCoroutine = StartCoroutine(coroutine_batteryCharge());
      return;
    }
    if (e.charging == false && batteryChargeCoroutine != null)
    {
      if (batteryChargeCoroutine != null)
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
    _batteryLevel = 1;
    while (_batteryLevel > 0)
    {
      _batteryLevel -= batteryDrainSpeed;
      yield return new WaitForSeconds(1);
    }
    EventBus.Publish(new EventBlinkBatteryBar { prevBatteryLevel = _batterys });
    yield return new WaitForSeconds(batteryBarBlinkInterval);
    _batterys -= 1;
    EventBus.Publish(new EventHeadlightStatusChange { enabled = false });
  }

  IEnumerator coroutine_batteryCharge()
  {
    while (true)
    {
      yield return new WaitForSeconds(batteryChargeInterval);
      _batterys += 1;
      if (_batterys > maxBattery) _batterys = maxBattery;
    }
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
      Vector2 LookVector = playerInput.Gameplay.Look.ReadValue<Vector2>();
      Vector3 LookVector3 = new Vector3(LookVector.x, LookVector.y, -10.0f);
      Vector3 pos = Camera.main.ScreenToWorldPoint(LookVector3) - player.transform.position;
      pos = new Vector3(-1.0f * pos.x, -1.0f * pos.y, pos.z);

      return pos;
    }
  }

  static public Inventory inventory
  {
    get => instance._player.GetComponent<Inventory>();
  }

  // void handler_EventFailure(EventFailure obj)
  // {
  //   _batteryLevel = maxBattery;
  //   StartCoroutine(coroutine_DelayedBatteryDrain());
  // }

}

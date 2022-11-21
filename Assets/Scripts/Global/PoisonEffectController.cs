using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PoisonEffectController : MonoBehaviour
{

  static PoisonEffectController _instance;
  public static PoisonEffectController instance
  {
    get
    {
      return _instance;
    }
  }

  void Awake()
  {
    if (_instance != null && _instance != this)
      Destroy(this.gameObject);
    else
      _instance = this;
  }

  Volume v;
  LensDistortion ld;
  DepthOfField dof;

  static public bool active
  {
    get => instance.enabled;
    set
    {
            if(value != null)
            {
                instance.v.enabled = value;
                instance.enabled = value;
            }
    
    }
  }

  void Start()
  {
    v = GetComponent<Volume>();
    v.profile.TryGet(out ld);
    v.profile.TryGet(out dof);
    active = false;
  }

  void Update()
  {
    ld.intensity.value = Mathf.Sin(Time.time) * 0.4f;
    dof.focalLength.value = Mathf.Sin(Time.time) * dof.focusDistance.value + dof.focusDistance.value;
  }

}

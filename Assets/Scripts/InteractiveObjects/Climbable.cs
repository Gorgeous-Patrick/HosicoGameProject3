using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ClimbType {Ladder, Rope, None}

[Serializable]
public struct ClimbObject
{
  [SerializeField]
  public GameObject obj; // Soft Rope or Ladder
  [SerializeField]
  public ClimbType type;

  public static GameObject peel(GameObject other)
  {
    Climbable c = other.GetComponent<Climbable>();
    if (c == null)
    {
      Debug.LogError("called ClimbObject::peel on non-climbable object");
      return null;
    }
    if (c.type == ClimbType.Ladder)
      return other;
    else if (c.type == ClimbType.Rope)
      return other.transform.parent.transform.parent.gameObject;
    return null;
  }

  public bool matches(GameObject other) => obj == peel(other);
  public void setTo(GameObject other)
  {
    type = other.GetComponent<Climbable>().type;
    obj = peel(other);
  }
  public void clear()
  {
    type = ClimbType.None;
    obj = null;
  }
}

public class Climbable : MonoBehaviour
{
  public ClimbType type;
}

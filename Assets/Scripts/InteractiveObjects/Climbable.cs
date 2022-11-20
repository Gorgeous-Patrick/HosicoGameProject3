using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClimbStatus {Ladder, Rope, None}

public class Climbable : MonoBehaviour
{
  public ClimbStatus type;
}

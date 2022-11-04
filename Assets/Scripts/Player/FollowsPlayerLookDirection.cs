using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowsPlayerLookDirection : MonoBehaviour
{
  void Update()
  {
    transform.up = Gameplay.playerLookDirection;
  }
}

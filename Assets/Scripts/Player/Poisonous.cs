using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poisonous : MonoBehaviour
{

  ParticleSystem ps;

  void Start()
  {
    ps = GetComponent<ParticleSystem>();
  }

  void OnParticleTrigger()
  {
    if (ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, new List<ParticleSystem.Particle>()) > 0)
    {
      var player = Gameplay.player.GetComponent<InhalesPoisonousGas>();
      if (player != null) player.inPoison = true;
    }
  }

}

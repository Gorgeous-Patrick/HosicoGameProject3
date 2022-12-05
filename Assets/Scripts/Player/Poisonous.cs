using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poisonous : MonoBehaviour
{

  ParticleSystem ps;

  void Start()
  {
    ps = GetComponent<ParticleSystem>();
    ps.trigger.AddCollider(Gameplay.player.transform);
  }

  void OnParticleTrigger()
  {
    if (ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, new List<ParticleSystem.Particle>()) > 0)
      Gameplay.player.GetComponent<InhalesPoisonousGas>().inPoison = true;
  }

}

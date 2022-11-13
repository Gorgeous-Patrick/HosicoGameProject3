using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratesCollapses : MonoBehaviour
{

  // if the quake strength at this point is lower than the threshold, no collapse will be triggered
  [SerializeField] int threshold;

  // when a collapse is triggered, generate prefabs from this list
  [SerializeField] List<GameObject> generatedPrefabs;

  // affects the amount of prefabs generated in a single collapse
  [SerializeField] int strength;

  float countdown;
  int remaining;

  void Start()
  {
    EventBus.Subscribe<EventQuake>(handler_EventQuake);
    countdown = 0;
    remaining = 0;
  }

  void handler_EventQuake(EventQuake e)
  {
    int quakeStrength = e.strengthAt(Utils.flatten(transform.position));
    if (quakeStrength >= threshold)
      remaining += strength * quakeStrength;
  }

  void Update()
  {
    if (countdown > 0)
      countdown -= Time.deltaTime;
    if (remaining > 0 & countdown <= 0)
    {
      remaining--;
      countdown = Random.Range(0, 0.5f);
      // TODO: trigger screen shake
      GameObject generatedObject = Utils.RandomChoice(generatedPrefabs);
      Vector2 pos = new Vector2(Random.Range(transform.position.x - transform.localScale.x / 2,
            transform.position.x + transform.localScale.x / 2),
          Random.Range(transform.position.y - transform.localScale.y / 2,
            transform.position.y + transform.localScale.y / 2) );
      Instantiate(generatedObject, pos, Quaternion.Euler(0, 0, Random.Range(0, 360)));
    }
  }

  void OnDrawGizmos()
  {
    Gizmos.color = new Color(255, 0, 0, 0.1f);
    Gizmos.DrawCube(transform.position, transform.localScale);
  }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsesWhenPlayerIsNear : Collapses
{

  protected override Color indicatorColor { get => new Color(255, 0, 0, 0.1f); }

  [SerializeField] float GenerationIntervalLowerBound, GenerationIntervalUpperBound;
  [SerializeField] int maxChildren = 5;

  HashSet<GameObject> generatedObjs;
  float countdown;

  void Start()
  {
    countdown = 0;
    generatedObjs = new HashSet<GameObject>();
    StartCoroutine(coroutine_GC());
  }

  void OnTriggerStay2D(Collider2D collisionInfo)
  {
    if (collisionInfo.gameObject.tag != "Player") return;
    if (countdown > 0)
      countdown -= Time.deltaTime;
    if (countdown <= 0)
    {
      countdown = Random.Range(GenerationIntervalLowerBound, GenerationIntervalUpperBound);
      // TODO: trigger screen shake
      if (generatedObjs.Count >= maxChildren) return;
      generatedObjs.Add(generate());
    }
  }

  void OnTriggerExit2D(Collider2D collisionInfo)
  {
    if (generatedObjs.Contains(collisionInfo.gameObject))
      generatedObjs.Remove(collisionInfo.gameObject);
  }

  IEnumerator coroutine_GC()
  {
    while (true)
    {
      generatedObjs.RemoveWhere((o) => o == null);
      yield return new WaitForSeconds(1);
    }
  }
}

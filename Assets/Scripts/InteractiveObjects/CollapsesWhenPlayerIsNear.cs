using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsesWhenPlayerIsNear : MonoBehaviour
{

  // when a collapse is triggered, generate prefabs from this list
  [SerializeField] List<GameObject> generatedPrefabs;

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
      GameObject generatedObject = Utils.RandomChoice(generatedPrefabs);
      Vector2 pos = new Vector2(Random.Range(transform.position.x - transform.localScale.x / 2,
            transform.position.x + transform.localScale.x / 2),
          Random.Range(transform.position.y - transform.localScale.y / 2,
            transform.position.y + transform.localScale.y / 2) );
      generatedObjs.Add(Instantiate(generatedObject, pos, Quaternion.Euler(0, 0, Random.Range(0, 360))));
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

  // show a red shade in the scene editor
  void OnDrawGizmos()
  {
    Gizmos.color = new Color(255, 0, 0, 0.1f);
    Gizmos.DrawCube(transform.position, transform.localScale);
  }
}

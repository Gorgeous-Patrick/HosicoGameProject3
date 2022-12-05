using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collapses : MonoBehaviour
{
  // when a collapse is triggered, generate prefabs from this list
  [SerializeField] protected  List<GameObject> generatedPrefabs;
  [SerializeField] protected Vector2 areaSize;
  abstract protected Color indicatorColor
  {
    get;
  }

  // show a shade in the scene editor
  void OnDrawGizmos()
  {
    Gizmos.color = indicatorColor;
    Gizmos.DrawCube(transform.position, areaSize);
  }

  protected GameObject generate()
  {
    GameObject generatedObject = Utils.RandomChoice(generatedPrefabs);
    Vector2 pos = new Vector2(Random.Range(transform.position.x - areaSize.x / 2,
                                           transform.position.x + areaSize.x / 2),
                              Random.Range(transform.position.y - areaSize.y / 2,
                                           transform.position.y + areaSize.y / 2) );
    return Instantiate(generatedObject, pos, Quaternion.Euler(0, 0, Random.Range(0, 360)));
  }

}

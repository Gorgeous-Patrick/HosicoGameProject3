using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private int numCubes = 8;
    [SerializeField] private float delay = 0.2f;
    [SerializeField] private float F = 100f;
    [SerializeField] private float radius = 2.1f;
  void Start()
  {

  }

  void Update()
  {

  }

  void OnCollisionEnter(Collision collisionInfo)
  {
    GameObject other = collisionInfo.gameObject;
    if (other.tag == "Player")
        {
            ExplodeFunc();
            Destroy(gameObject, delay);
        }
  }

    void ExplodeFunc()
    {
        for(int i_x = 0; i_x < numCubes; i_x++)
        {
            for(int i_y = 0; i_y < numCubes; i_y++)
            {
                for(int i_z = 0; i_z < numCubes; i_z++)
                {
                    CreateFrag(new Vector3(i_x, i_y, i_z));
                }
            }
        }
    }

    void CreateFrag(Vector3 loc_in)
    {
        GameObject frag = GameObject.CreatePrimitive(PrimitiveType.Cube);
        frag.layer = 10;

        Renderer rd = frag.GetComponent<Renderer>();
        rd.material = GetComponent<Renderer>().material;

        frag.transform.localScale = transform.localScale / (numCubes);

        Vector3 firstCube = transform.position - transform.localScale / 2 + frag.transform.localScale / 2;
        frag.transform.position = firstCube + Vector3.Scale(loc_in, frag.transform.localScale);

        frag.AddComponent<HasExpiration>();
        Rigidbody rb = frag.AddComponent<Rigidbody>();
        rb.AddExplosionForce(F, transform.position, radius);
    }

}

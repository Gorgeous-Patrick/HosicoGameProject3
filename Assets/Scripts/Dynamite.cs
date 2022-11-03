using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    [SerializeField] float explodeDelay = 3.0f;
    [SerializeField] GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExplodeAfterTime());
    }

    // To run on spawn - triggers 'explosion' 
    IEnumerator ExplodeAfterTime() {
        yield return new WaitForSeconds(explodeDelay);
        if (explosion != null)
            Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}

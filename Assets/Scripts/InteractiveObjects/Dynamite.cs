using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dynamite : MonoBehaviour
{
    public UnityEvent PlayFuseSound = new UnityEvent();
    [SerializeField] float explodeDelay = 3.0f;
    [SerializeField] GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExplodeAfterTime());
    }

    // To run on spawn - triggers 'explosion' 
    IEnumerator ExplodeAfterTime() {
        PlayFuseSound?.Invoke();
        yield return new WaitForSeconds(explodeDelay);
        if (explosion != null)
            Instantiate(explosion, transform.position, Quaternion.identity);
            AudioManager.instance.playSound("2-dynamite_explode", 1.0f);
        
        Destroy(this.gameObject);
    }
}

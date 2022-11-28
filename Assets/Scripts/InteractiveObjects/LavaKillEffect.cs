using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaKillEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DelayedKillPlayer()
    {
        yield return new WaitForSeconds(1);
        EventBus.Publish(new EventFailure());
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.instance.playSound("12-lava_hurt");
            StartCoroutine(DelayedKillPlayer());
        }
    }
}

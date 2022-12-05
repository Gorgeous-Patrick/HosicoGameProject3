using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaKillEffect : MonoBehaviour
{
    static bool isKilling = false;
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
        if (isKilling) {
            yield break;
        }
        isKilling = true;
        Gameplay.playerInput.Disable();
        yield return new WaitForSeconds(1f);
        EventBus.Publish(new EventFailure());
        isKilling = false;
        yield return null;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterSeconds : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !gameObject.CompareTag("PickaxePickup"))
            gameObject.SetActive(false);
    }

    public void StartDisable()
    {
        StartCoroutine(DisableCountdown());
    }
    private IEnumerator DisableCountdown()
    {
        yield return new WaitForSeconds(1.6f);
        gameObject.SetActive(false);
    }
}

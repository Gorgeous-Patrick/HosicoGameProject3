using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    [SerializeField] Color disguiseColor = Color.blue;
    [SerializeField] private bool isInvisible = false;
    [SerializeField] private float currTimer = 300f;

    private void FixedUpdate()
    {
        if (isInvisible && currTimer > 0)
        {
            currTimer--;
        }
        if (currTimer <= 0)
        {
            isInvisible = false;
            currTimer = 300f;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            EventBus.Publish(new EventPlayerDied());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickupPaint"))
        {
            isInvisible = true;
            currTimer = 390f;
            Destroy(other.gameObject);
            EventBus.Publish(new EventpromptShoot());
        }
    }

    public bool ReturnInvisibility()
    {
        return isInvisible;
    }
}

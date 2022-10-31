using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [SerializeField] private PaintProjectileController weapon;
    [SerializeField] private HasHealth health;

    private bool buttonPressed;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponentInParent<HasHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health.ReturnInvisibility())
        {
            if (Input.GetMouseButtonDown(0))
                buttonPressed = true;
            if (Input.GetMouseButtonUp(0))
                buttonPressed = false;
            if (buttonPressed) weapon.Fire();
        }
    }
}

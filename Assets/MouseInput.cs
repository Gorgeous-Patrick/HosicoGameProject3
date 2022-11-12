using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    Vector3 MousePosition;
    public LayerMask WhatIsPlatform;
    bool cheatModeOn = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(MousePosition, 0.2f);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cheatModeOn)
        {
            MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D overCollider = Physics2D.OverlapCircle(MousePosition, 0.01f, WhatIsPlatform);
            if (overCollider != null)
            {
                overCollider.transform.GetComponent<HasCanBeDugScript>().DestroyTileMapAtPoint(MousePosition);
            }
        }
    }
}

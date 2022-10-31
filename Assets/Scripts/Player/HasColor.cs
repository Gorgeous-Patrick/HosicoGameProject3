using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasColor : MonoBehaviour
{
    HasHealth healthScript;
    MeshRenderer playerMesh;
    [SerializeField] Material playerInivisible;
    [SerializeField] Material playerVisible;

    // Start is called before the first frame update
    void Start()
    {
        healthScript = GetComponentInParent<HasHealth>();
        playerMesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (healthScript.ReturnInvisibility())
        {
            playerMesh.material = playerInivisible;
        }
        else playerMesh.material = playerVisible;
    }
}

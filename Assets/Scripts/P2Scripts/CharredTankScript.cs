using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharredTankScript : MonoBehaviour
{
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnDestroyCharrTank()
    {
        sr.color = Color.grey;
    } 
}

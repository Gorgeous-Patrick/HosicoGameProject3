using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// super spaghetti and cheese time
public class SpriteFlasher : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color[] colorList =
    {
        Color.blue, Color.red, Color.green, Color.blue
    };
    private Color originalColor;
    [SerializeField] bool startFlash;
    private float countdown;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        startFlash = false;
        countdown = 100f;
    }

    private void Update()
    {
        if (startFlash)
        {
            bool renderOff = Time.frameCount % 4 == 0;
            sr.color = colorList[Random.Range(0, 4)];
            if (countdown <= 0)
            {
                countdown = 100f;
                startFlash = false;
                return;
            }
            countdown--;
        }
        else
        {
            sr.color = originalColor;
        }
    }

    public void FlashFunc()
    {
        startFlash = true;
    }


}

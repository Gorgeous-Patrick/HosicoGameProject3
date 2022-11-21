using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// not used anywhere now - should be deprecated very soon

// super spaghetti and cheese time
public class SpriteFlasher : MonoBehaviour
{
  private SpriteRenderer sr;
  private Color[] colorList =
  {
    Color.black, Color.white, Color.gray
  };
  private Color originalColor;
  [SerializeField] bool startFlash;
  private float countdown;

  private void Start()
  {
        EventBus.Subscribe<EventFailure>(handler_EventFailure);
        sr = GetComponent<SpriteRenderer>();
    originalColor = sr.color;
    startFlash = false;
    countdown = 100f;
  }

    private void handler_EventFailure(EventFailure obj)
    {
        FlashFunc();
    }

    private void Update()
  {
    if (startFlash)
    {
      bool renderOff = Time.frameCount % 4 == 0;
      sr.color = colorList[Random.Range(0, 3)];
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

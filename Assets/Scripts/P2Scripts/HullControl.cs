using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HullControl : MonoBehaviour
{
    Subscription<EventStopTank> sub_EventStopTank;
    public UnityEvent<float> OnSpdChangeDo = new UnityEvent<float>();
    // moving hull 
    public Rigidbody2D RB2D;
    [SerializeField] private float topSpd = 120;
    [SerializeField] private float topReverseSpd = 50;
    [SerializeField] private float turnSpd = 20;
    [SerializeField] private float accel = 45;
    private Vector2 locMoveVec;
    public float currSpd;
    private float currDir;
    // Start is called before the first frame update
    void Start()
    {
        RB2D = GetComponent<Rigidbody2D>();
        sub_EventStopTank = EventBus.Subscribe<EventStopTank>(OnEventStopTank);
    }

    private void OnEventStopTank(EventStopTank obj)
    {
        if (transform.CompareTag("Player"))
        {
            RB2D.velocity = Vector2.zero;
            currSpd = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var vector = currDir * currSpd * Time.deltaTime;
        if (currDir < 0)
        {
            if (currSpd > topReverseSpd)
                vector = currDir * topReverseSpd * Time.deltaTime;
        }
        RB2D.velocity = (Vector2)transform.up * vector;
        var angleCalc = -locMoveVec.x * turnSpd * Time.fixedDeltaTime;
        var quartenionCalc = Quaternion.Euler(0, 0, angleCalc);
        var rotationCalc = transform.rotation * quartenionCalc;
        RB2D.MoveRotation(rotationCalc);
    }
    public void MoveHull(Vector2 moveVec)
    {
        locMoveVec = moveVec;

        // calculate acceleration/deacceleration when moving
        if (Mathf.Abs(locMoveVec.y) > 0)
            currSpd += accel * Time.deltaTime;
        else currSpd -= accel * Time.deltaTime;
        OnSpdChangeDo?.Invoke(locMoveVec.magnitude);

        // bound speed within reasonable range
        if (currSpd > topSpd) currSpd = topSpd;
        if (currSpd < 0) currSpd = 0;

        // calculate direction and slow down if input oppose movement
        if (locMoveVec.y < 0)
        {
            if (currDir == 1) currSpd = 0;
            currDir = -1;
        }
        else if (locMoveVec.y > 0)
        {
            if (currDir == -1) currSpd = 0;
            currDir = 1;
        }
    }
}

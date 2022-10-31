using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasRotationController : MonoBehaviour
{
    Subscription<EventTurnGun> sub_EventTurnGun;

    // turning turret
    [SerializeField] private float turretTraverseSpeed = 80;
    [SerializeField] private Transform parentTurr;

    private Vector3 turretOrientation;
    private float turnStepMagnitude, getRadians, finalAngle;

    private void Start()
    {
        sub_EventTurnGun = EventBus.Subscribe<EventTurnGun>(OnTurnDo);
    }

    private void OnTurnDo(EventTurnGun obj)
    {
        RotateTurret(obj.Mpos3D);
    }

    private void RotateTurret(Vector3 MPos)
    {
        turnStepMagnitude = turretTraverseSpeed * Time.deltaTime;
        turretOrientation = MPos - parentTurr.position;
        // arctan of y/x
        getRadians = Mathf.Atan2(turretOrientation.y, turretOrientation.x);
        finalAngle = getRadians * Mathf.Rad2Deg;
        /*
        rotate turret towards the desired angle using the original turret angle, the desired
        angle and the rotation speed. 
        */
        parentTurr.rotation = Quaternion.RotateTowards(parentTurr.rotation,
                                                       Quaternion.Euler(0, 0, finalAngle),
                                                       turnStepMagnitude);
    }
}
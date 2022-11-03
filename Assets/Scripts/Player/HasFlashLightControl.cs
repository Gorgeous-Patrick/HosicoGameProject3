using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasFlashLightControl : MonoBehaviour
{
    // turning turret
    [SerializeField] private float turretTraverseSpeed = 80;
    [SerializeField] private Transform parentTurr;

    private Vector3 turretOrientation;
    private float turnStepMagnitude, getRadians, finalAngle;

    public void RotateTurret(Vector2 MPos)
    {
        turnStepMagnitude = turretTraverseSpeed * Time.deltaTime;
        turretOrientation = (Vector3)MPos - parentTurr.position;
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

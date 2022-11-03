using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasPlayerCharacterControl : MonoBehaviour
{
    // public GunControllerScript GunControl = null;
    public HasFlashLightControl TurretControl = null;
    public HullControl HullControl = null;
    // Start is called before the first frame update
    void Start()
    {
        // GunControl = GetComponentInChildren<GunControllerScript>();
        TurretControl = GetComponentInChildren<HasFlashLightControl>();
        HullControl = GetComponentInChildren<HullControl>();
    }

    public void Fire()
    {
        // if(GunControl != null) GunControl.Fire();
    }

    public void CallMoveHull(Vector2 moveTo)
    {
        if(HullControl != null) HullControl.MoveHull(moveTo);
    }

    public void CallTurnTurret(Vector2 turnTo)
    {
        if(TurretControl != null) TurretControl.RotateTurret(turnTo);
    }
}

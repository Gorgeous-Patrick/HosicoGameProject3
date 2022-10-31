using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPlayerDied
{
    public EventPlayerDied()
    {

    }
}

public class EventPlayerInvisible
{
    public EventPlayerInvisible()
    {

    }
}

public class EventBlindEnemy
{
    public EventBlindEnemy()
    {

    }
}

public class EventTurnGun
{
    public Vector3 Mpos3D;
    public EventTurnGun(Vector3 _Mpos3D)
    {
        Mpos3D = _Mpos3D;
    }
}

public class EventpromptShoot
{
    public EventpromptShoot()
    {

    }
}
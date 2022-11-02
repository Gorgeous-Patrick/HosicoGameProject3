using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDidFire
{

    public GameObject shooter;

    public EventDidFire(GameObject _shooter)
    {
        shooter = _shooter;
    }
}

public class EventDidBounce
{
    public GameObject shell;
    public Collision2D collision;

    public EventDidBounce(GameObject _shell, Collision2D _collision)
    {
        shell = _shell;
        collision = _collision;
    }
}

public class EventDidPen
{
    public GameObject shell;
    public Collision2D collision;

    public EventDidPen(GameObject _shell, Collision2D _collision)
    {
        shell = _shell;
        collision = _collision;
    }
}
public class EventChangeTxtSmoke
{
    public float valToText;

    public EventChangeTxtSmoke(float _val)
    {
        valToText = _val;
    }
}

public class EventDidKill
{
    public GameObject tank;
    public EventDidKill(GameObject _tank)
    {
        tank = _tank;
    }
}

public class EventDidPopSmoke
{
    public EventDidPopSmoke()
    {

    }
}

public class EventIsDetected
{
    public EventIsDetected()
    {

    }
}

public class EventStopTank
{

    public EventStopTank()
    {

    }
}

public class EventResupply
{
    public float ammoNum;
    public EventResupply(float _ammo)
    {
        ammoNum = _ammo;
    }
} 

public class EventDidRepair
{
    public float armorNum;

    public EventDidRepair(float _armor)
    {
        armorNum = _armor;
    }
}

public class EventShootAPHETutorial
{
    public EventShootAPHETutorial()
    {

    }
}

public class EventEnemyArmorTutorial
{
    public EventEnemyArmorTutorial()
    {

    }
}

public class EventShootAPCRTutorial
{
    public EventShootAPCRTutorial()
    {
    
    }
}

public class EventFlankEnemyTutorial
{
    public EventFlankEnemyTutorial()
    {

    }
}

public class EventArmorTutorial
{
    public EventArmorTutorial()
    {

    }
}

public class EventAngleArmorTutorial
{
    public EventAngleArmorTutorial()
    {

    }
}

public class EventRamTutorial
{
    public EventRamTutorial()
    {

    }
}

public class EventSmokeTutorial
{
    public EventSmokeTutorial()
    {

    }
}

public class EventBushTutorial
{
    public EventBushTutorial()
    {

    }
}

public class EventShakeCam
{
    public float time_in, amplitude;

    public EventShakeCam(float _amplitude, float _time)
    {
        amplitude = _amplitude;
        time_in = _time;
    }
}

public class EventAddHightlight
{
    public EventAddHightlight()
    {

    }
}


/* public class EventLowHealth
{
    public GameObject tank;
    public EventLowHealth(GameObject _tank)
    {
        tank = _tank;
    }
}

public class EventLowArmorDur
{
    public GameObject tank;

    public EventLowArmorDur(GameObject _tank)
    {
        tank = _tank;
    }
} */

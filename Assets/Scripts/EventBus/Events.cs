using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public abstract class Event
{
  // will be called by the event bus to display contents of the event
  public override string ToString()
  {
    string ret = "";
    FieldInfo[] ps = this.GetType().GetFields(
                       BindingFlags.DeclaredOnly |
                       BindingFlags.Public |
                       BindingFlags.Instance);
    foreach (var p in ps)
      ret += $"{p.Name}: {p.GetValue(this)}";
    return ret;
  }
}

public class EventFailure : Event
{
}

public class EventResetPlayer : Event
{
  public Vector2 pos;
}

public class EventHeadlightStatusChange : Event
{
  public bool enabled;
}

public class EventBatteryStatusChange : Event
{
  public bool charging;
}

public class EventChangeCheckpoint : Event
{
  public Transform checkpoint;
}

public class EventLoseHealth: Event
{
  public int health;
}

public class EventUpdateInventory : Event
{
  public string pickup;
  public int delta;
}

public class EventUpdatePickupUI : Event
{
  public string pickup;
  public int newAmount;
}

public class EventQuake: Event
{
  public int initialStrength;
  public Vector2 source;
  public int strengthAt(Vector2 pos)
  {
    int attenuation = (int)Math.Round(Vector2.Distance(source, pos) / 10);
    if (attenuation > initialStrength) return 0;
    else return initialStrength - attenuation;
  }
}

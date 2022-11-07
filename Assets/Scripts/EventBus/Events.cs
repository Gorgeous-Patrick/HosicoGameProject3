using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

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

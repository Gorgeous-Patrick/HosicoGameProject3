using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public abstract class Event
{
  public override string ToString()
  {
    string ret = "";
    PropertyInfo[] ps = this.GetType().GetProperties();
    foreach (PropertyInfo p in ps)
      ret += p.Name + ": " + p.GetValue(this) + "; ";
    return ret;
  }
}

public class Event_PlayerLookDirectionChanged
{
  public Vector2 dirvec;
  public Event_PlayerLookDirectionChanged(Vector2 dirvec_) => dirvec = dirvec_;
  public override string ToString()
  {
    string ret = "";
    PropertyInfo[] ps = this.GetType().GetProperties();
    foreach (PropertyInfo p in ps)
      ret += p.Name + ": " + p.GetValue(this) + "; ";
    return ret;
  }
}

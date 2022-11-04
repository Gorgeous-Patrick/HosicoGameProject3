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


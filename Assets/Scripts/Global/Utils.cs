using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Direction
{
  Up,
  Down,
  Left,
  Right,
  Undefined
}

[Serializable]
public class SerializablePair<First, Second>
{
  public First first;
  public Second second;
  public SerializablePair(First first_, Second second_)
  {
    first = first_;
    second = second_;
  }
  public SerializablePair() {}
}

public class CoroutineUtilities
{

  public static IEnumerator MoveObjectOverTime(Transform target, Vector3 initial_pos, Vector3 dest_pos, float duration_sec)
  {
    float initial_time = Time.time;
    float progress = (Time.time - initial_time) / duration_sec;
    while (progress < 1.0f)
    {
      progress = (Time.time - initial_time) / duration_sec;
      Vector3 new_position = Vector3.Lerp(initial_pos, dest_pos, progress);
      target.position = new_position;
      yield return null;
    }
    target.position = dest_pos;
  }
}

public class Utils
{

  static public Direction[] allDirections = { Direction.Left, Direction.Right, Direction.Up, Direction.Down };

  static public Direction inverse(Direction direction)
  {
    switch (direction)
    {
    case Direction.Right:
      return Direction.Left;
    case Direction.Up:
      return Direction.Down;
    case Direction.Left:
      return Direction.Right;
    case Direction.Down:
      return Direction.Up;
    }
    return Direction.Undefined;
  }

  public static Vector2 rotate(Vector2 v, float angle) =>
  new Vector2(
    v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle),
    v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle)
  );

  static public T RandomChoice<T>(List<T> choices)
  {
    return choices[(int)(UnityEngine.Random.Range(0f, choices.Count - 0.0001f))];
  }

  static public int RandomInt(int start, int end) // [start, end)
  {
    return (int)(UnityEngine.Random.Range(start, end - 0.0001f));
  }

  static public bool closeTo(Vector2 p1, Vector2 p2, float precision = 0.1f) =>
  Math.Abs(p1.x - p2.x) <= precision
  && Math.Abs(p1.y - p2.y) <= precision;

  static public Vector2 flatten(Vector3 v) => new Vector2(v.x, v.y);

  static public Vector2 dir2vec(Direction dir)
  {
    switch (dir)
    {
    case Direction.Up:
      return Vector2.up;
    case Direction.Down:
      return Vector2.down;
    case Direction.Left:
      return Vector2.left;
    case Direction.Right:
      return Vector2.right;
    default:
      return Vector2.zero;
    }
  }

  static public Direction vec2dir(Vector2 dirVec)
  {
    if (dirVec == Vector2.zero) return Direction.Undefined;
    if (Math.Abs(dirVec.y) >= Math.Abs(dirVec.x))
      return dirVec.y < 0 ? Direction.Down : Direction.Up;
    return dirVec.x < 0 ? Direction.Left : Direction.Right;
  }

}

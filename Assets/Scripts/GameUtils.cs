using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    None,
    Left,
    Right,
    Down,
    Up
}

public static class GameUtils
{
    public const float TileSize = 1f;

    public static readonly Dictionary<Direction, Vector3> Directions = new Dictionary<Direction, Vector3>()
    {
        { Direction.Left, Vector3.left },
        { Direction.Right, Vector3.right },
        { Direction.Down, Vector3.down },
        { Direction.Up, Vector3.up }
    };
    
    public static bool IsTargetInNextTile(this Vector3 position, Vector3 target, Direction direction)
    {
        var directionVector = Directions[direction];
        var tilePosition = position + directionVector * TileSize;
        return IsAt(tilePosition, target);
    }

    public static Direction GetDirectionTo(this Vector3 position, Vector3 target)
    {
        if (position.IsAt(target))
        {
            return Direction.None;
        }
        
        var diffX = target.x - position.x;
        var diffY = target.y - position.y;

        if (Mathf.Abs(diffX) >= Mathf.Abs(diffY))
        {
            return diffX > 0f ? Direction.Right : Direction.Left;
        }
        
        return diffY > 0f ? Direction.Up : Direction.Down;
    }

    public static bool IsAt(this Vector3 position, Vector3 target)
    {
        var distance = Vector2.Distance(position, target);
        var isInPosition = Mathf.Approximately(distance, 0f);
        return isInPosition;
    }
}

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
    private const float TileSize = 1f;

    private static readonly Dictionary<Direction, Vector3> Directions = new Dictionary<Direction, Vector3>()
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
        var distance = Vector2.Distance(tilePosition, target);
        var isInNextTile = Mathf.Approximately(distance, 0f);
        return isInNextTile;
    }
}

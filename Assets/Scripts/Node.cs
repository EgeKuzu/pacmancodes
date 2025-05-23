
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Node
{
    public Vector2Int Position;
    public bool IsWalkable;
    public int GCost;
    public int HCost;
    public Node Parent;

    public int FCost => GCost + HCost;

    public Node(Vector2Int pos, bool walkable)
    {
        Position = pos;
        IsWalkable = walkable;
    }
}

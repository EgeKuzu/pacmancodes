using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width;
    public int height;
    [SerializeField] private LayerMask wallMask; 
    private Node[,] grid;
    public Vector2 gridOrigin = Vector2.zero; 
    public float cellSize = 1f;            


    void Awake()
    {
        GenerateGrid();
    }

    void OnDrawGizmos()
    {
        if (grid == null)
            return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = (Vector3)gridOrigin + new Vector3((x + 0.5f) * cellSize, (y + 0.5f) * cellSize, 0);

                if (grid[x, y] != null && !grid[x, y].IsWalkable)
                    Gizmos.color = Color.red; 
                else
                    Gizmos.color = Color.green;

                Gizmos.DrawCube(position, Vector3.one * cellSize);

            }
        }
    }

    
    void GenerateGrid()
    {
        grid = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 worldPos = gridOrigin + new Vector2((x + 0.5f) * cellSize, (y + 0.5f) * cellSize);
                bool isWall = Physics2D.OverlapPoint(worldPos, wallMask);
                grid[x, y] = new Node(new Vector2Int(x, y), !isWall);
            }
        }
    }
    



    private bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }
    

    public List<Node> FindPath(Vector2Int start, Vector2Int target)
    {
        if (!IsInBounds(start) || !IsInBounds(target))
        {
            Debug.LogWarning($"[FindPath] Grid dışı koordinat: Start={start}, Target={target}");
            return null;
        }

        Node startNode = grid[start.x, start.y];
        Node targetNode = grid[target.x, target.y];

  
        if (!targetNode.IsWalkable)
        {
            Node fallback = null;
            foreach (Vector2Int dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int altPos = target + dir;
                if (IsInBounds(altPos))
                {
                    Node neighbor = grid[altPos.x, altPos.y];
                    if (neighbor.IsWalkable)
                    {
                        fallback = neighbor;
                        break;
                    }
                }
            }

            if (fallback == null)
            {
                Debug.Log("Hedef ve etrafı yürünemez. Yol bulunamadı.");
                return null;
            }
            targetNode = fallback;
        }


        List<Node> openSet = new List<Node> { startNode };
        HashSet<Node> closedSet = new HashSet<Node>();

        while (openSet.Count > 0)
        {
            Node current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
                if (openSet[i].FCost < current.FCost || (openSet[i].FCost == current.FCost && openSet[i].HCost < current.HCost))
                    current = openSet[i];

            openSet.Remove(current);
            closedSet.Add(current);

            if (current == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (!neighbor.IsWalkable || closedSet.Contains(neighbor))
                    continue;

                int newCost = current.GCost + GetDistance(current, neighbor);
                if (newCost < neighbor.GCost || !openSet.Contains(neighbor))
                {
                    neighbor.GCost = newCost;
                    neighbor.HCost = GetDistance(neighbor, targetNode);
                    neighbor.Parent = current;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null;
    }


    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int checkPos = node.Position + dir;
            if (checkPos.x >= 0 && checkPos.y >= 0 && checkPos.x < width && checkPos.y < height)
                neighbors.Add(grid[checkPos.x, checkPos.y]);
        }

        return neighbors;
    }

    int GetDistance(Node a, Node b)
    {
        return Mathf.Abs(a.Position.x - b.Position.x) + Mathf.Abs(a.Position.y - b.Position.y);
    }

    List<Node> RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }

    public Vector2Int WorldToSafeGridPosition(Vector3 worldPos)
    {
        Vector2Int gridPos = Vector2Int.FloorToInt(worldPos); 
        gridPos.x = Mathf.Clamp(gridPos.x, 0, width - 1);
        gridPos.y = Mathf.Clamp(gridPos.y, 0, height - 1);

        if (!grid[gridPos.x, gridPos.y].IsWalkable)
        {
            foreach (Vector2Int dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighbor = gridPos + dir;
                if (IsInBounds(neighbor) && grid[neighbor.x, neighbor.y].IsWalkable)
                    return neighbor;
            }

            Debug.Log("Yakında yürünebilir hücre bulunamadı!");
        }

        return gridPos;
    }

}

using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private Dictionary<Vector2Int, TileInfo> mapData;

    public Pathfinding(Dictionary<Vector2Int, TileInfo> mapData)
    {
        this.mapData = mapData;
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        List<Node> openList = new List<Node>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

        Node startNode = new Node(start, null, 0, GetDistance(start, target));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // Lấy node có F nhỏ nhất
            Node currentNode = openList[0];
            foreach (Node node in openList)
            {
                if (node.FCost < currentNode.FCost || (node.FCost == currentNode.FCost && node.hCost < currentNode.hCost))
                {
                    currentNode = node;
                }
            }

            openList.Remove(currentNode);
            closedSet.Add(currentNode.position);

            // Đã đến target
            if (currentNode.position == target)
            {
                return RetracePath(currentNode);
            }

            foreach (Vector2Int neighbor in GetNeighbors(currentNode.position))
            {
                if (!mapData.ContainsKey(neighbor) || !mapData[neighbor].walkable || closedSet.Contains(neighbor))
                    continue;

                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode.position, neighbor);
                Node neighborNode = openList.Find(n => n.position == neighbor);

                if (neighborNode == null)
                {
                    openList.Add(new Node(neighbor, currentNode, newCostToNeighbor, GetDistance(neighbor, target)));
                }
                else if (newCostToNeighbor < neighborNode.gCost)
                {
                    neighborNode.parent = currentNode;
                    neighborNode.gCost = newCostToNeighbor;
                    neighborNode.hCost = GetDistance(neighbor, target);
                }
            }
        }

        return null; // Không tìm thấy đường
    }

    private List<Vector2Int> RetracePath(Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;

        while (currentNode != null)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] directions = {
            new Vector2Int(0, 1),  // Up
            new Vector2Int(0, -1), // Down
            new Vector2Int(-1, 0), // Left
            new Vector2Int(1, 0)   // Right
        };

        foreach (Vector2Int dir in directions)
        {
            neighbors.Add(node + dir);
        }

        return neighbors;
    }

    private int GetDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private class Node
    {
        public Vector2Int position;
        public Node parent;
        public int gCost; // từ start
        public int hCost; // tới target
        public int FCost => gCost + hCost;

        public Node(Vector2Int position, Node parent, int gCost, int hCost)
        {
            this.position = position;
            this.parent = parent;
            this.gCost = gCost;
            this.hCost = hCost;
        }
    }
}

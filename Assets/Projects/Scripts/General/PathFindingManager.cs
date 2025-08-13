using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    public static PathfindingManager Instance;

    private Dictionary<Vector2Int, TileInfo> mapData;

    void Awake()
    {
        Instance = this;
    }

    public void Init(Dictionary<Vector2Int, TileInfo> map)
    {
        mapData = map;
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        Pathfinding pathfinding = new Pathfinding(mapData);
        return pathfinding.FindPath(start, end);
    }
}

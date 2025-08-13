using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class IslandMapGenerator : Singleton<IslandMapGenerator>
{
    [Header("Tilemap")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private RuleTile groundTile;
    [SerializeField] private Tilemap water;
    [SerializeField] private Tile waterTile;

    [Header("Map Settings")]
    [SerializeField] private int width = 100;
    [SerializeField] private int height = 100;
    [SerializeField] private float noiseScale = 0.1f;
    [SerializeField] private float threshold = 0.4f;

    [Header("Seed Settings")]
    [SerializeField] private int seed = 0;
    [SerializeField] private bool randomSeed = true;

    private Vector2 noiseOffset;

    private Dictionary<Vector2Int, TileInfo> mapData = new Dictionary<Vector2Int, TileInfo>();

    void Start()
    {
        GenerateMap();
    }

    #region GEN_MAP
    void InitializeNoiseOffset()
    {
        if (randomSeed)
        {
            seed = Random.Range(0, 999999);
        }

        Random.InitState(seed);
        noiseOffset = new Vector2(
            Random.Range(0f, 1000f),
            Random.Range(0f, 1000f)
        );
    }

    public void GenerateMap()
    {
        tilemap.ClearAllTiles();
        water.ClearAllTiles();
        mapData.Clear();

        InitializeNoiseOffset();

        Vector2 center = new Vector2(width / 2f, height / 2f);
        float maxDistance = Vector2.Distance(Vector2.zero, center);

        for (int x = 0; x < width; x += 2)
        {
            for (int y = 0; y < height; y += 2)
            {
                float nx = (x + noiseOffset.x) * noiseScale;
                float ny = (y + noiseOffset.y) * noiseScale;
                float noise = Mathf.PerlinNoise(nx, ny);

                float distanceToCenter = Vector2.Distance(new Vector2(x, y), center);
                float distanceRatio = distanceToCenter / maxDistance;
                noise -= distanceRatio * 0.3f;

                if (noise > threshold)
                {
                    PlaceChunk(x, y);
                    for (int dx = 0; dx < 2; dx++)
                    {
                        for (int dy = 0; dy < 2; dy++)
                        {
                            Vector2Int pos = new Vector2Int(x + dx, y + dy);
                            mapData[pos] = new TileInfo(TILETYPE.GROUND, true); // Ground + walkable
                        }
                    }
                }
            }
        }

        FillWater();
        SpawnTrees();
    }

    void PlaceChunk(int x, int y)
    {
        for (int dx = 0; dx < 2; dx++)
        {
            for (int dy = 0; dy < 2; dy++)
            {
                Vector3Int pos = new Vector3Int(x + dx, y + dy, 0);
                tilemap.SetTile(pos, groundTile);
            }
        }
    }

    void FillWater()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (!mapData.ContainsKey(pos))
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);
                    water.SetTile(tilePos, waterTile);
                    mapData[pos] = new TileInfo(TILETYPE.WATER, false); // Water + not walkable
                }
            }

            // Fill water hàng dưới cùng
            Vector2Int bottomKey = new Vector2Int(x, -1);
            Vector3Int bottomTilePos = new Vector3Int(x, -1, 0);
            water.SetTile(bottomTilePos, waterTile);
            mapData[bottomKey] = new TileInfo(TILETYPE.WATER, false);
        }
    }

    // Gọi từ Inspector để regenerate
    public void RegenerateMap()
    {
        GenerateMap();
    }

    // Lấy mapData cho cả spawn resource & A*
    public Dictionary<Vector2Int, TileInfo> GetMapData()
    {
        return mapData;
    }
    #endregion

    #region GEN_TREE

    [Header("Tree Spawn Settings")]
    [SerializeField] private GameObject treePrefab;
    [Range(0f, 1f)]
    [SerializeField] private float treeSpawnRate = 0.33f;
    [SerializeField] private Transform containTree; 
    

    private void SpawnTrees()
    {
        List<Vector2Int> groundTiles = new List<Vector2Int>();

        // Lấy toàn bộ tile GROUND
        foreach (var kvp in mapData)
        {
            if (kvp.Value.type == TILETYPE.GROUND && kvp.Value.walkable)
            {
                // Tile bên dưới không phải WATER
                Vector2Int below = new Vector2Int(kvp.Key.x, kvp.Key.y - 1);
                if (mapData.ContainsKey(below) && mapData[below].type != TILETYPE.WATER)
                {
                    groundTiles.Add(kvp.Key);
                }
            }
        }

        // Shuffle danh sách để tạo tính ngẫu nhiên
        for (int i = 0; i < groundTiles.Count; i++)
        {
            int randIndex = Random.Range(i, groundTiles.Count);
            var temp = groundTiles[i];
            groundTiles[i] = groundTiles[randIndex];
            groundTiles[randIndex] = temp;
        }

        // Tính số cây cần spawn
        int targetTreeCount = Mathf.RoundToInt(groundTiles.Count * treeSpawnRate);
        float minTreeDistance = 3f; // khoảng cách tối thiểu giữa các cây

        List<Vector2> placedTrees = new List<Vector2>();

        foreach (var gridPos in groundTiles)
        {
            if (placedTrees.Count >= targetTreeCount) break;

            Vector3 worldPos = new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 0);

            // Kiểm tra khoảng cách tới các cây đã spawn
            bool tooClose = false;
            foreach (var pos in placedTrees)
            {
                if (Vector2.Distance(pos, worldPos) < minTreeDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose) continue;

            // Spawn cây
            Instantiate(treePrefab, worldPos, Quaternion.identity, containTree);
            placedTrees.Add(worldPos);
        }
    }

    #endregion
}

public enum TILETYPE
{
    GROUND,
    WATER
}

public class TileInfo
{
    public TILETYPE type;
    public bool walkable;

    public TileInfo(TILETYPE type, bool walkable)
    {
        this.type = type;
        this.walkable = walkable;
    }
}

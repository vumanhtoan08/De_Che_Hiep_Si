using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public enum TileType
{
    Ground,
    Water
}

public class IslandMapGenerator : MonoBehaviour
{
    [Header("Tilemap")]
    public Tilemap tilemap;
    public RuleTile groundTile;
    public Tilemap water;
    public Tile waterTile;

    [Header("Map Settings")]
    public int width = 100;
    public int height = 100;
    public float noiseScale = 0.1f;
    public float threshold = 0.4f;

    [Header("Seed Settings")]
    public int seed = 0;
    public bool randomSeed = true;

    private Vector2 noiseOffset;
    private Dictionary<Vector2Int, TileType> mapData = new Dictionary<Vector2Int, TileType>();

    void Start()
    {
        GenerateMap();
    }

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
                            mapData[pos] = TileType.Ground;
                        }
                    }
                }
            }
        }

        FillWater();
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
                    mapData[pos] = TileType.Water;
                }
            }

            // Fill water hàng dưới cùng
            Vector2Int bottomKey = new Vector2Int(x, -1);
            Vector3Int bottomTilePos = new Vector3Int(x, -1, 0);
            water.SetTile(bottomTilePos, waterTile);
            mapData[bottomKey] = TileType.Water;
        }
    }

    // Gọi từ Inspector để regenerate
    public void RegenerateMap()
    {
        GenerateMap();
    }

    // Hàm để truy cập mapData từ bên ngoài
    public Dictionary<Vector2Int, TileType> GetMapData()
    {
        return mapData;
    }
}

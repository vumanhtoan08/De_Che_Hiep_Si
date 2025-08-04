using UnityEngine;
using UnityEngine.Tilemaps;

public class IslandMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public RuleTile groundTile;
    public Tilemap water;
    public Tile waterTile;

    public int width = 100;
    public int height = 100;

    public float noiseScale = 0.1f;
    public float threshold = 0.4f;

    public int seed = 0; // Thêm biến seed
    public bool randomSeed = true; // Cho phép tạo seed ngẫu nhiên

    private Vector2 noiseOffset;

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

        Random.InitState(seed); // Khởi tạo hệ thống random
        noiseOffset = new Vector2(
            Random.Range(0f, 1000f),
            Random.Range(0f, 1000f)
        );
    }

    void GenerateMap()
    {
        tilemap.ClearAllTiles();
        water.ClearAllTiles(); // Xóa map nước cũ

        InitializeNoiseOffset(); // Khởi tạo offset dựa trên seed

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
                }
            }
        }

        // Fill water ở những chỗ còn trống
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

    // Thêm hàm này để có thể generate lại map từ Inspector
    public void RegenerateMap()
    {
        GenerateMap();
    }
    void FillWater()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (tilemap.GetTile(pos) == null)
                {
                    water.SetTile(pos, waterTile);
                }
            }

            // Fill thêm một hàng water ở dưới cùng (y = -1)
            Vector3Int bottomPos = new Vector3Int(x, -1, 0);
            water.SetTile(bottomPos, waterTile);
        }
    }

}

// fill resouces vao map dieu kien la o duoi cua ruletile != water moi cho fill
using UnityEngine;
using UnityEngine.Tilemaps;

public class IslandMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public RuleTile groundTile;

    public int width = 100;
    public int height = 100;

    public float noiseScale = 0.1f;
    public float threshold = 0.4f;

    public Vector2 noiseOffset = new Vector2(0, 0);

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        tilemap.ClearAllTiles();

        Vector2 center = new Vector2(width / 2f, height / 2f);
        float maxDistance = Vector2.Distance(Vector2.zero, center);

        for (int x = 0; x < width; x += 2)
        {
            for (int y = 0; y < height; y += 2)
            {
                // Tạo noise
                float nx = (x + noiseOffset.x) * noiseScale;
                float ny = (y + noiseOffset.y) * noiseScale;
                float noise = Mathf.PerlinNoise(nx, ny);

                // Tính khoảng cách đến tâm bản đồ
                float distanceToCenter = Vector2.Distance(new Vector2(x, y), center);
                float distanceRatio = distanceToCenter / maxDistance;

                // Làm cho vùng xa tâm ít có khả năng được chọn
                noise -= distanceRatio * 0.3f; // bạn có thể thử điều chỉnh 0.6f

                if (noise > threshold)
                {
                    PlaceChunk(x, y);
                }
            }
        }
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
}

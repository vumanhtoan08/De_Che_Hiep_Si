using UnityEngine;

public class TreePosition : MonoBehaviour
{
    [Header("Grid Position (Map)")]
    public Vector2Int gridPosition; // Vị trí của cây trên lưới map

    private void Awake()
    {
        // Lấy tọa độ dạng Vector2Int từ vị trí thực tế trong Unity
        gridPosition = Vector2Int.RoundToInt(transform.position);
    }

    /// <summary>
    /// Trả về vị trí của cây trên world space
    /// </summary>
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    /// <summary>
    /// Trả về vị trí của cây trên grid
    /// </summary>
    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }
}

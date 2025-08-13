using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Soldier
{
    public Transform targetTree; // gán cây vào đây trong Inspector

    private List<Vector2Int> path;
    private int currentIndex = 0;

    void Start()
    {
        // Lấy vị trí bắt đầu (NPC) và vị trí kết thúc (cây)
        Vector2Int startPos = Vector2Int.RoundToInt(transform.position);
        Vector2Int treePos = Vector2Int.RoundToInt(targetTree.position);

        // Tìm ô trống gần cây
        Vector2Int targetPos = GetClosestWalkableToTree(treePos);

        // Tính path
        path = PathfindingManager.Instance.FindPath(startPos, targetPos);
    }

    void Update()
    {
        if (path == null || currentIndex >= path.Count) return;

        Vector3 targetWorldPos = new Vector3(path[currentIndex].x, path[currentIndex].y, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.05f)
        {
            currentIndex++;
        }
    }

    Vector2Int GetClosestWalkableToTree(Vector2Int treePos)
    {
        Dictionary<Vector2Int, TileInfo> mapData = PathfindingManager.Instance.FindPath(Vector2Int.zero, Vector2Int.zero) != null
            ? null : null; // tạm để placeholder, phần này mình sẽ tối ưu sau

        // Tạm trả về chính cây (bạn có thể đổi thành logic tìm ô cạnh cây)
        return treePos;
    }

    public override void Attack(Unit target)
    {
        base.Attack(target);
    }

    public override void Move(Vector2 target)
    {
        base.Move(target);
    }
}

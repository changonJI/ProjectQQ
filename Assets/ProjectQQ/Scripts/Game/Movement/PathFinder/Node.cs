using UnityEngine;

namespace QQ
{
    /// <summary>
    /// 스테이지 그리드
    /// </summary>
    public sealed class GridNode
    {
        public bool IsWalkable { get; private set; }
        public Vector2 WorldPosition { get; private set; }
        public Vector2Int GridPos { get; private set; }

        public GridNode(bool isWalkable, Vector2 worldPos, int gridX, int gridY)
        {
            IsWalkable = isWalkable;
            WorldPosition = worldPos;
            GridPos = new Vector2Int(gridX, gridY);
        }
    }

    /// <summary>
    /// 경로 계산을 위한 정보를 가진 노드
    /// </summary>
    public class PathNode : IHeapItem<PathNode>
    {
        /// 노드에 상응하는 위치의 그리드 노드
        public GridNode BaseNode { get; private set; }

        // 경로상에서 이 노드의 앞 노드
        public PathNode Parent { get; set; }

        // 경로 거리
        // fCost : 전체 거리 f(x) = g(x) + h(x)
        // gCost : 이 노드 도달하기까지의 거리
        // hCost : 이 노드에서 target까지의 거리
        public int fCost => gCost + hCost;
        public int gCost { get; set; }
        public int hCost { get; set; }

        public PathNode(GridNode baseNode = null)
        {
            Reset(baseNode);
        }

        public void Reset(GridNode baseNode = null)
        {
            BaseNode = baseNode;

            Parent = null;
            gCost = 0;
            hCost = int.MaxValue;
        }
        
        public void SetCost(int g, int h)
        {
            gCost = g;
            hCost = h;
        }

        public int HeapIndex { get; set; }

        public int CompareTo(PathNode nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (0 == compare)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }

            return -compare;
        }
    }
}
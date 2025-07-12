using UnityEngine;

namespace QQ
{
    public class Node
    {
        public bool IsWalkable { get; private set; }
        public Vector2 WorldPosition { get; private set; }
        public int GridX { get; private set; }
        public int GridY { get; private set; }

        // 경로상에서 이 노드의 앞 노드
        public Node Parent { get; set; }

        // 경로 거리
        // fCost : 전체 거리 f(x) = g(x) + h(x)
        // gCost : 이 노드 도달하기까지의 거리
        // hCost : 이 노드에서 target까지의 거리
        public int fCost => gCost + hCost;
        public int gCost { get; set; }
        public int hCost { get; set; }

        public Node(bool isWalkable, Vector2 worldPos, int gridX, int gridY)
        {
            IsWalkable = isWalkable;
            WorldPosition = worldPos;
            GridX = gridX;
            GridY = gridY;
        }
    }
}
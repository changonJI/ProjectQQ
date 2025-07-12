using System.Collections.Generic;
using UnityEngine;

namespace QQ
{
    public class GridManager : MonoBehaviour
    {
        private Node[,] grid;

        // 충돌처리 판별할 레이어
        [SerializeField] private LayerMask unwalkableLayer;

        // 노드 한 칸 반절 길이
        [SerializeField] private float nodeRadius;
        public float NodeRadius
        {
            get => nodeRadius;
            private set => nodeRadius = value;
        }

        public float NodeDiameter => NodeRadius * 2;

        // 전체 그리드 사이즈
        [SerializeField] private float worldSizeX;
        public float WorldSizeX
        {
            get => worldSizeX;
            private set => worldSizeX = value;
        }
        [SerializeField] private int worldSizeY;
        public int WorldSizeY
        {
            get => worldSizeY;
            private set => worldSizeY = value;
        }

        // 그리드 칸수
        private int GridSizeX { get; set; }
        private int GridSizeY { get; set; }

        private void Awake()
        {
            // 그리드 칸 수 구하기
            GridSizeX = Mathf.RoundToInt(WorldSizeX / NodeDiameter);
            GridSizeY = Mathf.RoundToInt(WorldSizeY / NodeDiameter);

            // 그리드 생성하기
            CreateGrid();
        }

        /// <summary>
        /// 이웃 노드 리스트 얻기
        /// </summary>
        /// <param name="node">이웃 노드를 얻을 중심 노드</param>
        /// <returns></returns>
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            // node 주변 둘레 8칸 탐색
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    // node 위치 건너뛰기
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int checkX = node.GridX + x;
                    int checkY = node.GridY + y;

                    // 전체 그리드 안에 있는지 확인
                    if (checkX >= 0 && checkX < GridSizeX && checkY >= 0 && checkY < GridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }


        /// <summary>
        /// 월드 좌표에 해당하는 노드를 반환
        /// </summary>
        /// <param name="worldPosition">노드를 얻을 월드 좌표</param>
        /// <returns></returns>
        public Node GetNodeFromWorldPos(Vector2 worldPosition)
        {
            float percentX = (worldPosition.x + WorldSizeX / 2) / WorldSizeX;
            float percentY = (worldPosition.y + WorldSizeY / 2) / WorldSizeY;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((GridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((GridSizeY - 1) * percentY);

            return grid[x, y];
        }

        private void CreateGrid()
        {
            grid = new Node[GridSizeX, GridSizeY];

            // 그리드 왼쪽 아래 끝점 (시작위치)
            Vector2 worldBottomLeft = transform.position;
            worldBottomLeft.x -= NodeDiameter * GridSizeX / 2;
            worldBottomLeft.y -= NodeDiameter * GridSizeY / 2;

            // 기본정보(위치정보, 이동 가불가 여부) 담아서 노드 생성
            for (int x = 0; x < GridSizeX; ++x)
            {
                for (int y = 0; y < GridSizeY; ++y)
                {
                    // 위치 구하기
                    Vector2 pos = worldBottomLeft + Vector2.right * (NodeDiameter * x + NodeRadius) + Vector2.up * (NodeDiameter * y + NodeRadius);

                    // 경로로 사용할 수 있는 노드인지 판별
                    bool isWalkableNode = !Physics2D.OverlapBox(pos, Vector2.one * NodeDiameter, 0f, unwalkableLayer);

                    // 노드 생성
                    grid[x, y] = new Node(isWalkableNode, pos, x, y);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(WorldSizeX, WorldSizeY, 0f));

            if (null != grid)
            {
                foreach (Node node in grid)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawCube(node.WorldPosition, Vector3.one * (NodeDiameter - 0.1f));

                }
            }
        }
    }
}
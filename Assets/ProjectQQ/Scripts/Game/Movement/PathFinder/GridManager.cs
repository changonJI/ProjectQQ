using System.Collections.Generic;
using UnityEngine;

namespace QQ
{
    public class GridManager : MonoBehaviour
    {
        private GridNode[,] grid;
        public bool isOnGizmo;

        // 충돌처리 판별할 레이어
        [SerializeField] private LayerMask unwalkableLayer;

        // 노드 한 칸 반절 길이
        [SerializeField] private float nodeRadius;
        public float NodeRadius { get => nodeRadius; private set => nodeRadius = value; }

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
        public Vector2Int GridSize { get; private set; }

        private void Awake()
        {
            // 그리드 칸 수 구하기
            int gridSizeX = Mathf.RoundToInt(WorldSizeX / NodeDiameter);
            int gridSizeY = Mathf.RoundToInt(WorldSizeY / NodeDiameter);

            GridSize = new Vector2Int(gridSizeX, gridSizeY);

            // 그리드 생성하기
            CreateGrid();
        }

        /// <summary>
        /// 그리드 노드 얻기 (지정한 좌표가 그리드에 없으면 null)
        /// </summary>
        /// <param name="gridPosX">그리드 x위치</param>
        /// <param name="gridPosY">그리드 y위치</param>
        /// <returns></returns>
        public GridNode GetNode(int gridPosX, int gridPosY)
        {
            // 전체 그리드 안에 있는지 확인
            if (gridPosX < 0 || gridPosX >= GridSize.x
                || gridPosY < 0 && gridPosY >= GridSize.y)
            {
                return null;
            }

            return grid[gridPosX, gridPosY];
        }

        /// <summary>
        /// 월드 좌표에 해당하는 노드를 반환
        /// </summary>
        /// <param name="worldPosition">노드를 얻을 월드 좌표</param>
        /// <returns></returns>
        public GridNode GetNodeFromWorldPos(Vector2 worldPosition)
        {
            float percentX = (worldPosition.x + WorldSizeX / 2) / WorldSizeX;
            float percentY = (worldPosition.y + WorldSizeY / 2) / WorldSizeY;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((GridSize.x - 1) * percentX);
            int y = Mathf.RoundToInt((GridSize.y - 1) * percentY);

            return grid[x, y];
        }

        private void CreateGrid()
        {
            grid = new GridNode[GridSize.x, GridSize.y];

            // 그리드 왼쪽 아래 끝점 (시작위치)
            Vector2 worldBottomLeft = transform.position;
            worldBottomLeft.x -= NodeDiameter * GridSize.x / 2;
            worldBottomLeft.y -= NodeDiameter * GridSize.y / 2;

            // 기본정보(위치정보, 이동 가불가 여부) 담아서 노드 생성
            for (int x = 0; x < GridSize.x; ++x)
            {
                for (int y = 0; y < GridSize.y; ++y)
                {
                    // 위치 구하기
                    Vector2 pos = worldBottomLeft + Vector2.right * (NodeDiameter * x + NodeRadius) + Vector2.up * (NodeDiameter * y + NodeRadius);

                    // 경로로 사용할 수 있는 노드인지 판별
                    bool isWalkableNode = !Physics2D.OverlapBox(pos, Vector2.one * NodeDiameter, 0f, unwalkableLayer);

                    // 노드 생성
                    grid[x, y] = new GridNode(isWalkableNode, pos, x, y);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (false == isOnGizmo)
            {
                return;
            }

            Gizmos.DrawWireCube(transform.position, new Vector3(WorldSizeX, WorldSizeY, 0f));

            if (null != grid)
            {
                foreach (GridNode node in grid)
                {
                    Color color = Color.white;
                    
                    if (!node.IsWalkable)
                    {
                        color = Color.red;
                    }

                    color.a = 0.25f;
                    Gizmos.color = color;

                    Gizmos.DrawCube(node.WorldPosition, Vector3.one * (NodeDiameter - nodeRadius / 5));
                }
            }
        }
    }
}
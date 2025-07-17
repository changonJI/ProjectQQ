using Cysharp.Threading.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace QQ
{
    public class Pathfinder : Singleton<Pathfinder>
    {
        public GridManager Grid { get; set; }
        private PathContextPool pathContextPool = new PathContextPool();

        public UniTask FindPathAsync(Vector2Int start, Vector2Int end, List<GridNode> resultPath)
        {
            return UniTask.RunOnThreadPool(() =>
            {
                // 컨텍스트 Get
                PathContext context = pathContextPool.Get(Grid);
                try
                {
                    // 길찾기
                    FindPath(start, end, context, resultPath);
                }
                finally
                {
                    // 컨텍스트 반납
                    pathContextPool.Release(context);
                }
            });
        }

        void FindPath(Vector2 startPos, Vector2 targetPos, PathContext context, List<GridNode> resultPath)
        {
            // 간단한 표기를 위한..
            PathNode[,] pathNodes = context.pathNodes;
            bool[,] opened = context.opened;
            bool[,] closed = context.closed;
            Heap<PathNode> openSet = context.openSet;

            Array.Clear(opened, 0, opened.Length);
            Array.Clear(closed, 0, closed.Length);
            openSet.Clear();

            GridNode startNode = Grid.GetNodeFromWorldPos(startPos);
            GridNode targetNode = Grid.GetNodeFromWorldPos(targetPos);

            opened[startNode.GridPos.x, startNode.GridPos.y] = true;
            openSet.Add(pathNodes[startNode.GridPos.x, startNode.GridPos.y]);

            // 탐색 전 노드가 있을 때만 동작
            while (0 < openSet.Count)
            {
                PathNode currentPathNode = openSet.Pop();
                GridNode currentNode = currentPathNode.BaseNode;

                for (int i = 0; i < openSet.Count; ++i)
                {
                    // 탐색 완료한 노드 체크
                    closed[currentNode.GridPos.x, currentNode.GridPos.y] = true;

                    // 이번 노드 == 종착점인 경우 탐색 종료
                    if (currentNode == targetNode)
                    {
                        PathNode startPathNode = pathNodes[startNode.GridPos.x, startNode.GridPos.y];
                        // 경로 추적
                        RetracePath(startPathNode, currentPathNode, resultPath);

                        return;
                    }

                    // curNode 주변 8칸 탐색
                    foreach (Vector2Int dir in _dirs)
                    {
                        Vector2Int dirPos = currentNode.GridPos + dir;
                        GridNode neighbour = Grid.GetNode(dirPos.x, dirPos.y);
                        if (null == neighbour || false == neighbour.IsWalkable
                            || true == closed[neighbour.GridPos.x, neighbour.GridPos.y])
                        {
                            continue;
                        }

                        PathNode neighbourPath = pathNodes[neighbour.GridPos.x, neighbour.GridPos.y];
                        int costToNeighbour = currentPathNode.gCost + GetHeuristicDistance(currentNode, neighbour);
                        neighbourPath.SetCost(costToNeighbour, GetHeuristicDistance(neighbour, targetNode));
                        neighbourPath.Parent = currentPathNode;

                        if (!openSet.Contains(neighbourPath))
                        {
                            openSet.Add(neighbourPath);
                        }
                    }
                }//END for openSet
            }//END while
        }

        /// <summary>
        /// 찾은 길 역방향으로 parent 추적하여 길 만들기
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        private void RetracePath(PathNode startNode, PathNode endNode, List<GridNode> resultPath)
        {
            PathNode currentNode = endNode;

            while (currentNode != startNode)
            {
                resultPath.Add(currentNode.BaseNode);
                currentNode = currentNode.Parent;
            }
            resultPath.Reverse();
        }

        private int GetHeuristicDistance(GridNode nodeA, GridNode nodeB)
        {
            int distX = Mathf.Abs(nodeA.GridPos.x - nodeB.GridPos.x);
            int distY = Mathf.Abs(nodeA.GridPos.y - nodeB.GridPos.y);

            // 대각선 = 루트2 근사값 1.4, 정수계산을 위해 *10
            if (distX > distY)
            {
                return 14 * distY + 10 * (distX - distY);
            }
            return 14 * distX + 10 * (distY - distX);
        }

        // 8방향 이동 (대각선 포함)
        private readonly Vector2Int[] _dirs = new Vector2Int[]
        {
        new Vector2Int( 1,  0),
        new Vector2Int(-1,  0),
        new Vector2Int( 0,  1),
        new Vector2Int( 0, -1),
        new Vector2Int( 1,  1),
        new Vector2Int( 1, -1),
        new Vector2Int(-1,  1),
        new Vector2Int(-1, -1)
        };
    }


    class PathContext
    {
        public PathNode[,] pathNodes;
        public bool[,] opened;
        public bool[,] closed;
        public Heap<PathNode> openSet;

        public PathContext(int gridSizeX, int gridSizeY)
        {
            pathNodes = new PathNode[gridSizeX, gridSizeY];
            opened = new bool[gridSizeX, gridSizeY];
            closed = new bool[gridSizeX, gridSizeY];
            openSet = new Heap<PathNode>(gridSizeX * gridSizeY);
        }

        public void Reset(GridManager grid)
        {
            // 배열은 재할당하지 않고 Clear만
            for (int x = 0; x < grid.GridSize.x; x++)
            {
                for (int y = 0; y < grid.GridSize.y; y++)
                {
                    pathNodes[x, y].Reset(grid.GetNode(x, y));
                }
            }

            Array.Clear(opened, 0, opened.Length);
            Array.Clear(closed, 0, closed.Length);
            openSet.Clear();
        }
    }

    class PathContextPool
    {
        readonly ConcurrentStack<PathContext> stack = new ConcurrentStack<PathContext>();

        public PathContext Get(GridManager grid)
        {
            if (stack.TryPop(out var context))
            {
                context.Reset(grid);
                return context;
            }

            return new PathContext(grid.GridSize.x, grid.GridSize.y);
        }

        public void Release(PathContext ctx)
        {
            stack.Push(ctx);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace QQ
{
    public class Pathfinder : MonoBehaviour
    {
        GridManager grid;
        void Awake()
        {
            grid = GetComponent<GridManager>();

            if (null == grid)
            {
                gameObject.AddComponent<GridManager>();
            }
        }

        void Update()
        {

        }

        void FindPath(BaseGameObject startObject, BaseGameObject targetPos)
        {
            FindPath(startObject.transform.position, targetPos.transform.position);
        }

        void FindPath(Vector2 startPos, Vector2 targetPos)
        {
            Node startNode = grid.GetNodeFromWorldPos(startPos);
            Node targetNode = grid.GetNodeFromWorldPos(targetPos);

            // 탐색 전 노드
            // TODO. PriorityQueue로 변경 필요
            List<Node> openSet = new List<Node>();
            // 탐색이 끝난 노드
            HashSet<Node> closeSet = new HashSet<Node>();
            openSet.Add(startNode);

            // 탐색 전 노드가 있을 때만 동작
            while (0 < openSet.Count)
            {
                Node currentNode = openSet[0];
                for (int i = 0; i < openSet.Count; ++i)
                {
                    // 거리 비용 비교하여 현재 탐색 노드 변경
                    if (openSet[i].fCost < currentNode.fCost
                        || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                    {
                        currentNode = openSet[i];
                    }

                    // 이번에 탐색할 노드 탐색 끝 노드로 옮기기
                    openSet.Remove(currentNode);
                    closeSet.Add(currentNode);

                    // 이번 노드 == 종착점인 경우 탐색 종료
                    if (currentNode == targetNode)
                    {
                        // 경로 추적
                        RetracePath(startNode, targetNode);
                        return;
                    }

                    List<Node> neighbours = grid.GetNeighbours(currentNode);
                    foreach (Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.IsWalkable || closeSet.Contains(neighbour))
                        {
                            continue;
                        }
                        int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 찾은 길 역방향으로 parent 추적하여 길 만들기
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        void RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
            int distY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

            // 대각선 = 루트2 근사값 1.4, 정수계산을 위해 *10
            if (distX > distY)
            {
                return 14 * distY + 10 * (distX - distY);
            }
            return 14 * distX + 10 * (distY - distX);
        }
    }
}
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QQ.FSM
{
    public class MonsterMovement : MovementBase
    {
        private Vector3 DestPos { get; set; }
        private GridNode DestNode { get; set; }
        private GridNode Node { get; set; }
        private bool isRequireFindPath = true;

        public void SetDestination(Vector3 destPos)
        {
            // 추적해야 할 노드가 바뀌었을 경우에만 Path를 다시 탐색한다.
            if (DestPos != destPos)         // 좌표가 달라짐
            {
                GridNode destNode = Pathfinder.Instance.Grid.GetNodeFromWorldPos(destPos);
                if (DestNode != destNode)   // 노드도 달라짐
                {
                    isRequireFindPath = true; // 경로 재탐색
                    DestNode = destNode;
                }// 목표 노드가 같은 경우엔 경로 재탐색은 필요 없음

                DestPos = destPos;
            }
        }

        protected override void OnInit()
        {
            
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnDestroyed()
        {
            
        }

        protected override void OnUpdate()
        {
            Node = Pathfinder.Instance.Grid.GetNodeFromWorldPos(gameObject.transform.position);

            if (null == Node)
            {
                LogHelper.LogWarning($"몬스터 '{name}'의 현재 위치 노드 파악 불가");
                return;
            }

            SetMoveDirectionByPath();

            // 길찾기..
            if (false == findingPath && true == isRequireFindPath)
            {
                FindPathToTarget().Forget();
            }
        }

        protected override void OnFixedUpdate()
        {
            
        }

        private async UniTask FindPathToTarget()
        {
            isRequireFindPath = false;
            findingPath = true;
            path.Clear();

            await Pathfinder.Instance.FindPathAsync(gameObject.transform.position, DestPos, path);

            findingPath = false;
        }

        private void SetMoveDirectionByPath()
        {
            if (path.TryPeek(out var nextNode)) // path가 있는 경우
            {
                if (nextNode == Node)    // 다음 노드에 도달한 경우
                {
                    path.TryPop(out _); // 꺼내고 결과값 버리기

                    if (path.TryPeek(out nextNode))
                    {
                        MoveDirection = nextNode.WorldPosition - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
                        MoveDirection = MoveDirection.normalized;
                    }
                    else   // 목표 노드에 도달했을 때
                    {
                        MoveDirection = Vector2.zero;
                    }
                }
                else if (Mathf.Abs(Node.GridPos.x - nextNode.GridPos.x) > 1
                    || Mathf.Abs(Node.GridPos.y - nextNode.GridPos.y) > 1)
                {   // nextNode가 인접노드가 아닐 때 경로 재탐색
                    isRequireFindPath = true;
                }
            }
            else // path가 없는 경우
            {
                MoveDirection = Vector2.zero;

                // DestNode가 내 Node가 아니면 길찾기
                if (Node != DestNode)
                {
                    isRequireFindPath = true;
                }
            }
        }
    }
}
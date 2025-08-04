using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace QQ
{
    /// <summary>
    /// 몬스터 소환기
    /// 주기마다 List<SpawnRuntimeData>를 순회,
    /// 타임트리거에 MonsterSpawnData에 해당하는 소환 실행
    /// </summary>
    public class MonsterSpawner : MonoBehaviour
    {
        public const int chapterSegmentDivider = 10000;
        public const int stageSegmentDivider = 100;

        private List<SpawnRuntimeData> spawnRuntimeDatas = new List<SpawnRuntimeData>();
        public int Chapter { get; private set; }
        public int Stage { get; private set; }
        private GridManager StageGrid { get; set; }

        private float spawnCheckTimestamp;
        public int spawnMonsterCount = 0;   // 인스펙터에 소환 마리수 표시하기위한 테스트 변수

        CameraBouond camBound;
        List<GridNode> spawnAreaNodes = new List<GridNode>();

        [SerializeField] private bool isOnGizmo;

        void Start()
        {

        }

        void Update()
        {
            if (true == TryAdvanceTick(ref spawnCheckTimestamp, 1.0f))
            {
                // 카메라 영역과 소환 범위 연산
                CalcCameraAndSpawnArea();

                for (int i = spawnRuntimeDatas.Count - 1; i >= 0; --i)
                {
                    SpawnRuntimeData runtimeData = spawnRuntimeDatas[i];

                    // 소환타이밍 도래함?
                    if (runtimeData.nextSpawnTime <= spawnCheckTimestamp)
                    {
                        // 소환
                        SpawnMonsters(runtimeData.id).Forget();

                        // 이번 소환으로 남은 소환 횟수 더 없으면 List에서 지우기
                        if (0 >= --runtimeData.remainingSetCount)
                        {
                            spawnRuntimeDatas.RemoveAt(i);
                            continue;
                        }

                        // 다음 소환시간 갱신
                        runtimeData.nextSpawnTime += runtimeData.spawnLoopTime;
                        // 데이터 갱신
                        spawnRuntimeDatas[i] = runtimeData;
                    }
                }//END for spawnRuntimeDatas
            }//END if TryAdvanceTick
        }

        /// <summary>
        /// id에 해당하는 몬스터스폰데이터의 몬스터들 소환하기
        /// </summary>
        /// <param name="id">MonsterSpawnData의 id</param>
        private async UniTaskVoid SpawnMonsters(int id)
        {
            MonsterSpawnData spawnData = MonsterSpawnDataManager.Instance.Get(id);
            if (spawnData.id != id)
            {
                return;
            }

            for (int i = 0; i < spawnData.monsterCnt1; ++i)
            {
                GameObject monster = await PoolManager.Instance.GetObject(GameObjectType.Monster, "TestMonster", spawnData.monsterId1);

                // 위치 지정
                if (null != monster)
                {
                    Vector3 spawnPos = GetRandomSpawnPos();
                    monster.transform.position = spawnPos;

                    ++spawnMonsterCount;
                }
            }

            for (int i = 0; i < spawnData.monsterCnt2; ++i)
            {
                GameObject monster = await PoolManager.Instance.GetObject(GameObjectType.Monster, "TestMonster", spawnData.monsterId2);

                // 위치 지정
                if (null != monster)
                {
                    Vector3 spawnPos = GetRandomSpawnPos();
                    monster.transform.position = spawnPos;

                    ++spawnMonsterCount;
                }
            }
        }

        /// <summary>
        /// 그리드 기반 랜덤 소환 위치 지정
        /// </summary>
        /// <returns></returns>
        private Vector3 GetRandomSpawnPos()
        {
            Vector3 resultPos = Vector3.zero;

            // 그리드 없으면 그냥 원점소환
            if (null == StageGrid)
            {
                return resultPos;
            }

            // 랜덤 추출 범위 : 그리드로 지정된 영역
            if (true == camBound.IsValid() && 0 < spawnAreaNodes.Count)
            {
                int randomNode = -1;

                int i = 0;
                do
                {
                    randomNode = Random.Range(0, spawnAreaNodes.Count);
                    i++;
                } while (true == spawnAreaNodes[randomNode].IsWalkable && i < 500);

                resultPos = spawnAreaNodes[randomNode].WorldPosition;
            }
            else
            {
                // 랜덤 노드 1개 추출
                int x = Random.Range(0, StageGrid.GridSize.x);
                int y = Random.Range(0, StageGrid.GridSize.y);

                resultPos = StageGrid.GetNode(x, y).WorldPosition;
            }

            return resultPos;
        }

        /// <summary>
        /// 시간이 특정 간격만큼 증가했으면 기준틱 간격만큼 증가 후 return true;
        /// </summary>
        /// <param name="anchorTick"></param>
        /// <param name="intervalTick"></param>
        /// <returns></returns>
        private bool TryAdvanceTick(ref float anchorTick, float intervalTick)
        {
            if (anchorTick + intervalTick <= Time.time)
            {
                anchorTick += intervalTick;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 특정 스테이지의 몬스터 스폰 데이터 기준으로 스포너 세팅
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="stage"></param>
        public void SetStage(int chapter, int stage, GridManager stageGrid, float playerCameraHalfW, float playerCameraHalfH)
        {
            Chapter = chapter;
            Stage = stage;
            StageGrid = stageGrid;

            spawnRuntimeDatas.Clear();

            // id : 000000 = chapter00 stage00 wave00
            // wave는 1부터 연속된 숫자로 존재함을 전제로 함
            int chapterSegment = chapter * chapterSegmentDivider;
            int stageSegment = stage * stageSegmentDivider;
            int idBase = chapterSegment + stageSegment;

            for (int i = 1; i < stageSegmentDivider; ++i)
            {
                int id = idBase + i;
                MonsterSpawnData spawnData = MonsterSpawnDataManager.Instance.Get(id);
                if (spawnData.id != id) // 다른 id가 나왔다 == 이 id는 없다
                {
                    break; // wave 연번이므로 없으면 break;
                }

                SpawnRuntimeData runtimeData = new SpawnRuntimeData();
                runtimeData.id = id;
                runtimeData.nextSpawnTime = spawnData.spawnStart + Time.time;
                runtimeData.spawnLoopTime = spawnData.spawnLoop;
                runtimeData.remainingSetCount = spawnData.spawnCount;

                spawnRuntimeDatas.Add(runtimeData);
            }

            // 카메라 사이즈
            camBound.SetCameraHalfSize(playerCameraHalfW, playerCameraHalfH);
        }

        private void CalcCameraAndSpawnArea()
        {
            if (0 >= spawnRuntimeDatas.Count)
            {
                return;
            }

            Vector3 actorPosition = PoolManager.Instance.actor.transform.position;
            GridNode actorNode = StageGrid.GetNodeFromWorldPos(actorPosition);

            if (null == actorNode)
            {
                camBound.Reset();
            }

            // center가 똑같으면 갱신하지 않아도 됨
            if (camBound.centerNode != actorNode)
            {
                int camLeftGridX = Mathf.Max(0, StageGrid.GetGridX(actorPosition.x - camBound.cameraSizeHalf.x));
                int camRightGridX = Mathf.Min(StageGrid.GridSize.x - 1, StageGrid.GetGridX(actorPosition.x + camBound.cameraSizeHalf.x));
                int camTopGridY = Mathf.Min(StageGrid.GridSize.y - 1, StageGrid.GetGridY(actorPosition.y + camBound.cameraSizeHalf.y));
                int camBottomGridY = Mathf.Max(0, StageGrid.GetGridY(actorPosition.y - camBound.cameraSizeHalf.y));

                camBound.SetEdgeGridIndex(actorNode, camTopGridY, camRightGridX, camBottomGridY, camLeftGridX);

                CalcSpawnArea();
            }
        }

        private void CalcSpawnArea()
        {
            spawnAreaNodes.Clear();

            int gridW = StageGrid.GridSize.x;
            int gridH = StageGrid.GridSize.y;

            // 소환 영역 박스
            int leftGridX = camBound.leftGridX - 1;
            int rightGridX = camBound.rightGridX + 1;
            int topGridY = camBound.topGridY + 1;
            int bottomGridY = camBound.bottomGridY - 1;

            if (leftGridX >= 0)
            {
                for (int y = Mathf.Max(bottomGridY, 0); y < topGridY; ++y)
                {
                    GridNode node = StageGrid.GetNode(leftGridX, y);
                    if (null == node)
                    {
                        break;
                    }
                    if (false == node.IsWalkable)
                    {
                        continue;
                    }

                    spawnAreaNodes.Add(node);
                }
            }
            if (topGridY < gridH)
            {
                for (int x = Mathf.Max(leftGridX, 0); x < rightGridX; ++x)
                {
                    GridNode node = StageGrid.GetNode(x, topGridY);
                    if (null == node)
                    {
                        break;
                    }
                    if (false == node.IsWalkable)
                    {
                        continue;
                    }

                    spawnAreaNodes.Add(node);
                }
            }
            if (rightGridX < gridW)
            {
                for (int y = Mathf.Max(bottomGridY, 0) + 1; y < topGridY + 1; ++y)
                {
                    GridNode node = StageGrid.GetNode(rightGridX, y);
                    if (null == node)
                    {
                        break;
                    }
                    if (false == node.IsWalkable)
                    {
                        continue;
                    }

                    spawnAreaNodes.Add(node);
                }
            }
            if (bottomGridY >= 0)
            {
                for (int x = Mathf.Max(leftGridX, 0) + 1; x < rightGridX + 1; ++x)
                {
                    GridNode node = StageGrid.GetNode(x, bottomGridY);
                    if (null == node)
                    {
                        break;
                    }
                    if (false == node.IsWalkable)
                    {
                        continue;
                    }

                    spawnAreaNodes.Add(node);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (true == isOnGizmo)
            {
                foreach (var spawnArea in spawnAreaNodes)
                {
                    Color color = UnityEngine.Color.yellow;
                    color.a = 0.5f;
                    Gizmos.color = color;

                    Gizmos.DrawCube(spawnArea.WorldPosition, Vector3.one * (Pathfinder.Instance.Grid.NodeDiameter - Pathfinder.Instance.Grid.NodeRadius / 5));
                }
            }
        }

        private struct SpawnRuntimeData
        {
            public int id;                  // 테이블 데이터 id
            public float nextSpawnTime;     // 다음 소환 타이밍  // TODO. 스테이지 시작시간으로 기준으로 변경 필요
            public int spawnLoopTime;     // 소환 주기
            public int remainingSetCount;   // 남은 소환 횟수
        }

        private struct CameraBouond
        {
            // 카메라 width height 반지름
            public Vector2 cameraSizeHalf;

            // 기준, 중심위치 노드
            public GridNode centerNode;
            // 카메라 영역 모서리 인덱스
            public int topGridY;
            public int rightGridX;
            public int bottomGridY;
            public int leftGridX;

            public bool IsValid()
            {
                if (null == centerNode || 0 >= cameraSizeHalf.x || 0 >= cameraSizeHalf.y
                    || 0 >= topGridY - bottomGridY || 0 >= rightGridX - leftGridX)
                {
                    return false;
                }

                return true;
            }

            public void SetCameraHalfSize(float playerCameraHalfW, float playerCameraHalfH)
            {
                cameraSizeHalf.x = playerCameraHalfW;
                cameraSizeHalf.y = playerCameraHalfH;
            }

            public void SetEdgeGridIndex(GridNode center, int top, int right, int bottom, int left)
            {
                centerNode = center;
                topGridY = top;
                rightGridX = right;
                bottomGridY = bottom;
                leftGridX = left;
            }

            public void Reset()
            {
                centerNode = null;
                topGridY = -1;
                rightGridX = -1;
                bottomGridY = -1;
                leftGridX = -1;
            }
        }
    }
}
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
        public Vector2 test;

        void Start()
        {

        }

        void Update()
        {
            int maxCatchUp = 5;
            int loopCount = 0;

            if (null != camBound.centerNode)
                test = camBound.centerNode.WorldPosition;

            // 혹시라도 긴 틱이 밀렸을 경우를 대비하기 위해 while로 조건 잡음
            while (++loopCount < maxCatchUp && true == TryAdvanceTick(ref spawnCheckTimestamp, 1.0f))
            {
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
                    Vector3 spawnPos = RandomSpawnPos(true);
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
                    Vector3 spawnPos = RandomSpawnPos(true);
                    monster.transform.position = spawnPos;

                    ++spawnMonsterCount;
                }
            }
        }

        /// <summary>
        /// 그리드 기반 랜덤 소환 위치 지정
        /// </summary>
        /// <returns></returns>
        private Vector3 RandomSpawnPos(bool isOutsiedCamera)
        {
            Vector3 resultPos = Vector3.zero;

            // 그리드 없으면 그냥 원점소환
            if (null == StageGrid)
            {
                return resultPos;
            }

            CalcCameraArea();

            // 랜덤 추출 범위 : 그리드로 지정된 영역
            if (true == isOutsiedCamera && true == camBound.IsValid())
            {
                int gridW = StageGrid.GridSize.x;
                int gridH = StageGrid.GridSize.y;

                // 소환 제외 영역 모서리
                int leftGridX = camBound.leftGridX;
                int rightGridX = camBound.rightGridX;
                int topGridY = camBound.topGridY;
                int bottomGridY = camBound.bottomGridY;

                // 제외 영역 기준 상, 하, 좌, 우 구획 4별 노드 수
                int topNodeCount = (gridH - topGridY) * gridW;
                int bottomNodeCount = (bottomGridY + 1) * gridW;
                int leftNodeCount = leftGridX * (topGridY - bottomGridY + 1);
                int rightNodeCount = (gridW - rightGridX - 1) * (topGridY - bottomGridY + 1);

                int nodeCount = topNodeCount + bottomNodeCount + leftNodeCount + rightNodeCount;
                int randomNode = Random.Range(0, nodeCount);
                if (randomNode < topNodeCount)
                {
                    // 상단 영역 (camTopGridY+1 ~ StageGrid.GridSize.y - 1)
                    int idx = randomNode;
                    int y = topGridY + (idx / gridW) + 1;
                    int x = idx % gridW;
                    resultPos = StageGrid.GetNode(x, y).WorldPosition;
                }
                else if (randomNode < topNodeCount + bottomNodeCount)
                {
                    // 하단 영역 (0 ~ camBottomGridY)
                    int idx = randomNode - topNodeCount;
                    int y = idx / gridW;
                    int x = idx % gridW;
                    resultPos = StageGrid.GetNode(x, y).WorldPosition;
                }
                else if (randomNode < topNodeCount + bottomNodeCount + leftNodeCount)
                {
                    // 좌측 영역 (camBottomGridY ~ camTopGridY) & (0 ~ camLeftGridX-1)
                    int idx = randomNode - topNodeCount - bottomNodeCount;
                    int y = bottomGridY + (idx / leftGridX);
                    int x = idx % leftGridX;
                    resultPos = StageGrid.GetNode(x, y).WorldPosition;
                }
                else
                {
                    // 우측 영역 (camBottomGridY ~ camTopGridY) & (camRightGridX+1 ~ StageGrid.GridSize.x-1)
                    int idx = randomNode - topNodeCount - bottomNodeCount - leftNodeCount;
                    int width = gridW - rightGridX - 1;
                    int y = bottomGridY + (idx / width);
                    int x = rightGridX + 1 + (idx % width);
                    resultPos = StageGrid.GetNode(x, y).WorldPosition;
                }
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

        private void CalcCameraArea()
        {
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
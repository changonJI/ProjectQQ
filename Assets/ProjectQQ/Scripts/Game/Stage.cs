using UnityEngine;

namespace QQ
{
    public class Stage : BaseGameObject
    {
        public override GameObjectType ObjType => GameObjectType.Stage;
        private StageData stageData;
        private MonsterSpawner monsterSpawner;
        private GridManager grid;

        // 값 들어간거 확인용, 안정적 동작 확인 이후 삭제
        [SerializeField] int chapter;
        [SerializeField] int stage;

        protected override void OnDestroyed() { }

        protected override void OnFixedUpdate() { }

        protected override void OnFocus() { }

        protected override void OnInit()
        {
            monsterSpawner = gameObject.AddComponent<MonsterSpawner>(this);
            grid = gameObject.GetComponent<GridManager>();
        }

        protected override void OnLateUpdate() { }

        protected override void OnLostFocus() { }

        protected override void OnStart()
        {
            Pathfinder.Instance.Grid = GetComponent<GridManager>();
        }

        protected override void OnTriggerEnter2Ded(Collider2D other) { }

        protected override void OnCollisionEnter2Ded(Collision2D other) { }

        protected override void OnUpdate()
        {
        }

        public void SetChapterStage(int chapterGroup, int stageNumber)
        {
            chapter = chapterGroup;
            stage = stageNumber;

            if (0 == chapter || 0 == stage)
            {
                LogHelper.LogError($"{gameObject.name} 스테이지 chapter, stage 설정 안됨");
            }

            stageData = StageDataManager.Instance.Get(chapter, stage);
            if (stageData.chapterGroup != chapterGroup || stageData.stageNumber != stageNumber)
            {
                LogHelper.LogError($"{gameObject.name} chapter, stage에 매치되는 데이터 없음");
            }
            tableID = stageData.id;
        }

        // SetChapterStage 먼저 하고 호출해야 함
        public void SetMonsterSpawner(float camHalfW, float camHalfH)
        {
            // 몬스터 스폰 세팅
            if (null != monsterSpawner)
            {
                monsterSpawner.SetStage(chapter, stage, grid, camHalfW, camHalfH);
            }
        }

        
    }
}
using System.Collections;
using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class WildBoarEntity : TargetableEntity
    {
        public WildBoarData wildBoarData;

        // 属性访问器
        public Animator Animator => animator;
        public float MoveSpeed => moveSpeed;
        public bool entityReady = false;
        public bool inDeathProcess = false;
        public bool inAttackProcess = false;
        public CharacterEntity character = null;

        private Animator animator;
        private float moveSpeed;

        private IFsm<WildBoarEntity> wildBoarFsm;

        private Coroutine coroutine;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            // 初始化数据
            wildBoarData = userData as WildBoarData;
            // 获取组件引用
            animator = GetComponentInChildren<Animator>();
            // 初始化速度
            moveSpeed = wildBoarData.Speed;
            // 初始化主角的引用
            int _id = DataNodeExtension.GetCharacterEntityId();
            Entity _entity = GameEntry.Entity.GetGameEntity(_id);
            character = _entity as CharacterEntity;
            // 初始化状态机
            InitStateMachine();
        }

        private void InitStateMachine()
        {
            // 创建状态机
            FsmState<WildBoarEntity>[] states =
            {
                new WildBoarIdleState(),
                new WildBoarMoveState(),
                new WildBoarAttackState(),
                new WildBoarDeathState()
            };

            wildBoarFsm = GameEntry.Fsm.CreateFsm(
                "WildBoarFSM",
                this,
                states
            );

            // 启动状态机，默认进入Idle状态
            wildBoarFsm.Start<WildBoarIdleState>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            // 延迟更新所在层，防止过早被攻击判定
            coroutine = StartCoroutine(SetLayerRecursivelyInDelay(userData));
        }

        private void OnDestory()
        {
            StopCoroutine(coroutine);
            coroutine = null;

            GameEntry.Fsm.DestroyFsm(wildBoarFsm);
        }

        private IEnumerator SetLayerRecursivelyInDelay(object userData)
        {
            yield return new WaitForSeconds(.1f);
            gameObject.SetLayerRecursively(Constant.Layer.EnemyLayerId);
            // 这里显示血条
            var targetableObjectData = userData as TargetableObjectData;
            GameEntry.HPBar.ShowHPBar(this, targetableObjectData.HPRatio, targetableObjectData.HPRatio);
            // 实体初始化完成
            entityReady = true;
        }

        public void ResetData()
        {
            entityReady = false;
            inDeathProcess = false;
            moveSpeed = 0f;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        public override ImpactData GetImpactData()
        {
            return new ImpactData(wildBoarData.Camp, wildBoarData.HP, wildBoarData.Attack, wildBoarData.Defense);
        }
    }
}

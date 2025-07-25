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
        public bool inAttackProcess = false;
        public CharacterEntity character = null;
        public float resetTime = -1f;
        public IFsm<WildBoarEntity> wildBoarFsm;

        private Animator animator;
        private float moveSpeed;

        public bool isHide;

        private Coroutine coroutine;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            // 初始化数据
            wildBoarData = userData as WildBoarData;
            // 获取组件引用
            animator = GetComponentInChildren<Animator>();
            // 初始化主角的引用
            int _id = DataNodeExtension.GetCharacterEntityId();
            Entity _entity = GameEntry.Entity.GetGameEntity(_id);
            character = _entity as CharacterEntity;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            // 延迟更新所在层，防止过早被攻击判定
            coroutine = StartCoroutine(SetLayerRecursivelyInDelay(userData));

            // 初始化状态机
            InitStateMachine();

            // 初始化速度
            moveSpeed = wildBoarData.Speed;
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();

            // 重置数据
            ResetData();
            // 隐藏血条
            GameEntry.HPBar.HideHPBar(this);
            // 摧毁状态机
            GameEntry.Fsm.DestroyFsm(wildBoarFsm);
            wildBoarFsm = null;
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
                $"WildBoarFSM-{wildBoarData.Id}",
                this,
                states
            );

            // 启动状态机，默认进入Idle状态
            wildBoarFsm.Start<WildBoarIdleState>();
        }

        private IEnumerator SetLayerRecursivelyInDelay(object userData)
        {
            yield return new WaitForSeconds(.1f);
            gameObject.SetLayerRecursively(Constant.Layer.EnemyLayerId);
            // 这里显示血条
            var targetableObjectData = userData as TargetableObjectData;
            GameEntry.HPBar.ShowHPBar(this, targetableObjectData.HPRatio, targetableObjectData.HPRatio);
        }

        public void ResetData()
        {
            inAttackProcess = false;
            moveSpeed = 0f;
            resetTime = -1;
            isHide = false;
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

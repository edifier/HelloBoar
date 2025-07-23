using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class CharacterEntity : TargetableEntity
    {
        public Animator animator;
        private float moveSpeed = 4f;
        private float attackRange = 2f;

        private IFsm<CharacterEntity> characterFsm;

        public CharacterData characterData = null;

        // 属性访问器
        public Animator Animator => animator;
        public float MoveSpeed => moveSpeed;
        public float AttackRange => attackRange;

        public List<Entity> hitEntityList = new();
        public bool inAttackProcess = false;
        public bool inDeathProcess = false;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            // 初始化数据
            characterData = userData as CharacterData;
            // 获取组件引用
            animator = GetComponentInChildren<Animator>();
            // 初始化状态机
            InitStateMachine();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            characterData = userData as CharacterData;
            Name = $"{characterData.Name} {Id}";

            // 显示血条
            var targetableObjectData = userData as TargetableObjectData;
            GameEntry.HPBar.ShowHPBar(this, targetableObjectData.HPRatio, targetableObjectData.HPRatio);
        }

        private void InitStateMachine()
        {
            // 创建状态机
            FsmState<CharacterEntity>[] states =
            {
                new CharacterIdleState(),
                new CharacterMoveState(),
                new CharacterAttackState(),
                new CharacterDeathState()
            };

            characterFsm = GameEntry.Fsm.CreateFsm(
                "CharacterFSM",
                this,
                states
            );

            // 启动状态机，默认进入Idle状态
            characterFsm.Start<CharacterIdleState>();
        }

        public override ImpactData GetImpactData()
        {
            return new ImpactData(characterData.Camp, characterData.HP, characterData.Attack, characterData.Defense);
        }

        public void ResetData()
        {
            hitEntityList.Clear();
            inAttackProcess = false;
            inDeathProcess = false;
        }
    }
}

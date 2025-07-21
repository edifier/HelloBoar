using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class CharacterEntity : TargetableEntity
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float attackRange = 2f;

        private IFsm<CharacterEntity> characterFsm;

        public CharacterData characterData = null;

        // 属性访问器
        public Vector2 MoveDirection { get; private set; }
        public Animator Animator => animator;
        public float MoveSpeed => moveSpeed;
        public float AttackRange => attackRange;

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

            Name = Utility.Text.Format("Aircraft ({0})", Id);
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

        public void ChangeState(ICharacterStateType newState)
        {
            switch (newState)
            {
                case ICharacterStateType.Idle:
                    characterFsm.Start<CharacterIdleState>();
                    break;
                case ICharacterStateType.Move:
                    characterFsm.Start<CharacterMoveState>();
                    break;
                case ICharacterStateType.Attack:
                    characterFsm.Start<CharacterAttackState>();
                    break;
                case ICharacterStateType.Death:
                    characterFsm.Start<CharacterDeathState>();
                    break;
            }
        }
        
        public override ImpactData GetImpactData()
        {
            return new ImpactData(characterData.Camp, characterData.HP, 0, characterData.Defense);
        }
    }
}

using System.Collections.Generic;
using System.Threading;
using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public abstract class CharacterAttackDecision : CharacterSurvivalState
    {
        private bool moveAttackHandle;
        private bool hasPerformDamage;

        private static readonly int moveAttackHash = Animator.StringToHash("MoveAttack");
        private static readonly int emptyHash = Animator.StringToHash("Empty");

        protected override void OnInit(IFsm<CharacterEntity> _fsm)
        {
            base.OnInit(_fsm);
        }

        protected override void OnLeave(IFsm<CharacterEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);

            // 移动状态下OnLeave时, 取消移动攻击状态
            if (moveAttackHandle)
            {
                EndMoveAttackHandle();
                character.inAttackProcess = false;
            }
        }

        protected override void OnUpdate(IFsm<CharacterEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            if (character.inEscapeProcess)
            {
                timer += elapseSeconds;
                if (timer >= .5f) character.inEscapeProcess = false;
                return;
            }

            if (moveAttackHandle)
            {
                AnimatorStateInfo currentStateInfo = character.Animator.GetCurrentAnimatorStateInfo(1);

                if (currentStateInfo.IsName("MoveAttack") && currentStateInfo.normalizedTime >= .5f && !hasPerformDamage)
                {
                    hasPerformDamage = true;
                    // 攻击进行到中段
                    // 中段是斧子落下的时机，判断现在角色面前是否有人，并造成伤害
                    // 受伤害的怪物，不一定是起手时面前的怪物
                    List<Entity> targets = DetectTargetsInAttackRange();
                    if (targets.Count != 0)
                    {
                        // 造成伤害
                        PerformDamage(targets);
                    }
                }

                if (currentStateInfo.IsName("MoveAttack") && currentStateInfo.normalizedTime >= .95f)
                {
                    EndMoveAttackHandle();
                    character.inAttackProcess = false;
                }

                // 一次移动攻击执行中，不再执行下面的判定
                return;
            }

            List<Entity> res = DetectTargetsInAttackRange();
            if (res.Count != 0 && !character.inAttackProcess)
            {
                // 如果有被攻击实体，进入攻击状态
                if (res.Count == 0) return;
                character.inAttackProcess = true;
                // 站立攻击
                if (fsm.CurrentState is CharacterIdleState)
                    ChangeState<CharacterAttackState>(fsm);
                // 奔跑攻击
                else if (fsm.CurrentState is CharacterMoveState)
                    StartMoveAttackHandle();
            }
        }

        private void StartMoveAttackHandle()
        {
            character.Animator.SetLayerWeight(1, 0.8f);
            character.Animator.SetTrigger(moveAttackHash);
            hasPerformDamage = false;
            moveAttackHandle = true;
        }

        private void EndMoveAttackHandle()
        {
            character.Animator.ResetTrigger(moveAttackHash);
            character.Animator.SetTrigger(emptyHash);
            character.Animator.SetLayerWeight(1, 0f);
            moveAttackHandle = false;
        }
    }
}


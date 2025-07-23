using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class CharacterAttackState : CharacterBaseState
    {
        private static readonly int attackHash = Animator.StringToHash("Attack");
        private bool hasPerformDamage = false;

        protected override void OnInit(IFsm<CharacterEntity> _fsm)
        {
            base.OnInit(_fsm);
        }

        protected override void OnEnter(IFsm<CharacterEntity> _fsm)
        {
            base.OnEnter(_fsm);

            // 播放攻击动画
            character.Animator.SetTrigger(attackHash);
        }

        protected override void OnUpdate(IFsm<CharacterEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            // 动画进行50%，结算伤害
            AnimatorStateInfo currentStateInfo = character.Animator.GetCurrentAnimatorStateInfo(0);
            if (currentStateInfo.normalizedTime >= .55f && !hasPerformDamage)
            {
                hasPerformDamage = true;
                PerformDamage();
            }

            // 动画结束，进入等待状态
            if (currentStateInfo.normalizedTime >= .95f)
                ChangeState<CharacterIdleState>(_fsm);
        }

        protected override void OnLeave(IFsm<CharacterEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);

            hasPerformDamage = false;
            character.inAttackProcess = false;
            character.Animator.ResetTrigger(attackHash);
        }

        private void PerformDamage()
        {
            foreach (Entity hitEntity in fsm.Owner.hitEntityList)
                AIUtility.PerformDamage(fsm.Owner, hitEntity);
        }
    }
}

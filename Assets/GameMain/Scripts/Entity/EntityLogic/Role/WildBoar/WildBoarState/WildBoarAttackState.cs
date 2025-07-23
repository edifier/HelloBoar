using UnityEngine;
using GameFramework.Fsm;

namespace GoodbyeWildBoar
{
    public class WildBoarAttackState : WildBoarBaseState
    {
        private static readonly int attackHash = Animator.StringToHash("Attack");
        private bool hasPerformDamage = false;
        protected override void OnInit(IFsm<WildBoarEntity> _fsm)
        {
            base.OnInit(_fsm);
        }

        protected override void OnEnter(IFsm<WildBoarEntity> _fsm)
        {
            base.OnEnter(_fsm);

            // 播放待机动画
            wildBoar.Animator.SetTrigger(attackHash);
        }

        protected override void OnUpdate(IFsm<WildBoarEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            // 动画进行50%，结算伤害
            AnimatorStateInfo currentStateInfo = wildBoar.Animator.GetCurrentAnimatorStateInfo(0);

            if (currentStateInfo.normalizedTime >= .55f && !hasPerformDamage)
            {
                hasPerformDamage = true;
                // character伤害
                // 检测攻击范围，在范围内攻击主角
                Vector3 rayOrigin = ownerTs.localPosition + rayOffset;
                int hitCount = Physics.SphereCastNonAlloc(rayOrigin, rayRadius, ownerTs.forward, hitInfo, rayDistance, 1 << attackableLayers.value);
                if (hitCount != 0 && hitInfo[0].collider.gameObject == wildBoar.character.gameObject)
                    AIUtility.PerformDamage(fsm.Owner, wildBoar.character);
            }

            // 动画结束，进入等待状态
            if (currentStateInfo.normalizedTime >= .95f)
            {
                // 攻击间隔
                wildBoar.resetTime = .7f;
                ChangeState<WildBoarIdleState>(_fsm);
            }
        }

        protected override void OnLeave(IFsm<WildBoarEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);

            wildBoar.Animator.ResetTrigger(attackHash);
            hasPerformDamage = false;
            wildBoar.inAttackProcess = false;
        }
    }
}

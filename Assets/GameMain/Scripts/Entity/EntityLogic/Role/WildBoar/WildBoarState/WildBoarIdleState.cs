using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class WildBoarIdleState : WildBoarAlertState
    {
        private static readonly int IdleHash = Animator.StringToHash("Idle");
        private Rigidbody rb;

        protected override void OnInit(IFsm<WildBoarEntity> _fsm)
        {
            base.OnInit(_fsm);

            rb = ownerTs.GetComponent<Rigidbody>();
        }

        protected override void OnEnter(IFsm<WildBoarEntity> _fsm)
        {
            base.OnEnter(_fsm);

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            // 播放待机动画
            wildBoar.Animator.SetTrigger(IdleHash);
        }

        protected override void OnUpdate(IFsm<WildBoarEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            // 休息时间，输出的好机会
            if (wildBoar.resetTime > 0)
            {
                timer += elapseSeconds;
                if (timer < wildBoar.resetTime) return;
                else wildBoar.resetTime = 0f;
            }

            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<WildBoarEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);

            wildBoar.Animator.ResetTrigger(IdleHash);
        }
    }
}

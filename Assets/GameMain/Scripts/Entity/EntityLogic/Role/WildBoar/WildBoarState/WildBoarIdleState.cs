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
            // 死亡进程中，说明是播完死亡动画来到idle状态
            if (wildBoar.IsDead && !wildBoar.isHide)
            {
                // 实体回收
                wildBoar.isHide = true;
                // 隐藏实体
                GameEntry.Entity.HideEntity(wildBoar);
            }
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

            if (wildBoar != null)
                wildBoar.Animator.ResetTrigger(IdleHash);
        }
    }
}

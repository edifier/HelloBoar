using UnityEngine;
using GameFramework.Fsm;

namespace GoodbyeWildBoar
{
    public class WildBoarMoveState : WildBoarAlertState
    {
        private static readonly int runHash = Animator.StringToHash("Run");

        protected override void OnInit(IFsm<WildBoarEntity> _fsm)
        {
            base.OnInit(_fsm);
        }

        protected override void OnEnter(IFsm<WildBoarEntity> _fsm)
        {
            base.OnEnter(_fsm);

            // 播放待机动画
            wildBoar.Animator.SetTrigger(runHash);
        }

        protected override void OnUpdate(IFsm<WildBoarEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<WildBoarEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);

            wildBoar.Animator.ResetTrigger(runHash);
        }
    }
}


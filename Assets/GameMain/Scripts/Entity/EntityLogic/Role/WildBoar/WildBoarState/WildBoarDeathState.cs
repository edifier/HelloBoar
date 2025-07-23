using UnityEngine;
using GameFramework.Fsm;

namespace GoodbyeWildBoar
{
    public class WildBoarDeathState : WildBoarBaseState
    {
        private static readonly int deathHash = Animator.StringToHash("Death");

        protected override void OnEnter(IFsm<WildBoarEntity> _fsm)
        {
            base.OnEnter(_fsm);

            // 播放待机动画
            wildBoar.Animator.SetTrigger(deathHash);
        }

        protected override void OnUpdate(IFsm<WildBoarEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            AnimatorStateInfo currentStateInfo = wildBoar.Animator.GetCurrentAnimatorStateInfo(0);
            // 死亡后的处理
            if (currentStateInfo.normalizedTime >= .95f && wildBoar.inDeathProcess)
            {
                // 重置数据
                wildBoar.ResetData();
                // 隐藏血条
                GameEntry.HPBar.HideHPBar(wildBoar);
                // 隐藏实体
                GameEntry.Entity.HideEntity(wildBoar);
                // 进入Idle状态
                ChangeState<WildBoarIdleState>(_fsm);
            }
        }

        protected override void OnLeave(IFsm<WildBoarEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);

            // 重置动画trigger
            wildBoar.Animator.ResetTrigger(deathHash);
        }
    }
}

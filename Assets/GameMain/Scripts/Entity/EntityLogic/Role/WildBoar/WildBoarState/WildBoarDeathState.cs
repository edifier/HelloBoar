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
            // 播放死亡音乐
            GameEntry.Sound.PlayMusic(wildBoar.wildBoarData.DeadSoundId);
        }

        protected override void OnUpdate(IFsm<WildBoarEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            if (wildBoar == null) return;

            // 防止异常处理
            if (!wildBoar.IsDead)
            {
                ChangeState<WildBoarIdleState>(_fsm);
                return;
            }

            AnimatorStateInfo currentStateInfo = wildBoar.Animator.GetCurrentAnimatorStateInfo(0);
            // 死亡后的处理
            if (currentStateInfo.normalizedTime >= .95f)
                // 进入站立状态
                ChangeState<WildBoarIdleState>(_fsm);
        }

        protected override void OnLeave(IFsm<WildBoarEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);

            // 重置动画trigger
            wildBoar.Animator.ResetTrigger(deathHash);
        }
    }
}

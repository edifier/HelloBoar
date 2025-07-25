using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class CharacterDeathState : CharacterBaseState
    {
        private static readonly int deathHash = Animator.StringToHash("Death");

        protected override void OnInit(IFsm<CharacterEntity> _fsm)
        {
            base.OnInit(_fsm);
        }

        protected override void OnEnter(IFsm<CharacterEntity> _fsm)
        {
            base.OnEnter(_fsm);

            // 播放死亡动画
            character.Animator.SetTrigger(deathHash);
            // 播放死亡音乐
            GameEntry.Sound.PlayMusic(character.characterData.DeadSoundId);
        }

        protected override void OnUpdate(IFsm<CharacterEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            AnimatorStateInfo currentStateInfo = character.Animator.GetCurrentAnimatorStateInfo(0);
            if (currentStateInfo.normalizedTime >= .95f)
                // 进入idle状态
                ChangeState<CharacterIdleState>(_fsm);
        }

        protected override void OnLeave(IFsm<CharacterEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);

            character.Animator.ResetTrigger(deathHash);
        }
    }
}

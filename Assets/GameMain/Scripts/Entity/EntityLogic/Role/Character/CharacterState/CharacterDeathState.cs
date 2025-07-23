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
        }

        protected override void OnUpdate(IFsm<CharacterEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            // 切换场景回来会出现非死亡状态下进入当前state下的情况
            if (character.inSwitchSceneProcess && !character.IsDead)
            {
                character.inSwitchSceneProcess = false;
                ChangeState<CharacterIdleState>(_fsm);
            }

            AnimatorStateInfo currentStateInfo = character.Animator.GetCurrentAnimatorStateInfo(0);
            if (currentStateInfo.normalizedTime >= .95f && character.inDeathProcess)
            {
                // 数据重置
                character.ResetData();
                // 隐藏血条
                GameEntry.HPBar.HideHPBar(character);
                // 隐藏实体
                GameEntry.Entity.HideEntity(character);
                // 进入idle状态
                ChangeState<CharacterIdleState>(_fsm);
            }
        }

        protected override void OnLeave(IFsm<CharacterEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);

            character.Animator.ResetTrigger(deathHash);
        }
    }
}

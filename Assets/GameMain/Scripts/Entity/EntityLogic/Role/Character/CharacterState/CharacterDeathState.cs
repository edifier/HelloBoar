using System.Collections;
using System.Collections.Generic;
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

            AnimatorStateInfo currentStateInfo = character.Animator.GetCurrentAnimatorStateInfo(0);
            if (currentStateInfo.normalizedTime >= .95f && character.inDeathProcess)
            {
                character.Animator.ResetTrigger(deathHash);
                // 数据重置
                character.ResetData();
                // 隐藏实体
                GameEntry.Entity.HideEntity(character);
                // todo: 是否需要且回到menu场景
            }
        }

        protected override void OnLeave(IFsm<CharacterEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);
        }
    }
}

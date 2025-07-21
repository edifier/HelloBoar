using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class CharacterAttackState : CharacterBaseState
    {
        private static readonly int attackHash = Animator.StringToHash("Attack");

        protected override void OnInit(IFsm<CharacterEntity> _fsm)
        {
            base.OnInit(_fsm);
        }

        protected override void OnEnter(IFsm<CharacterEntity> _fsm)
        {
            base.OnEnter(_fsm);

            // 播放待机动画
            _fsm.Owner.Animator.SetTrigger(attackHash);
            Debug.Log("进入攻击状态");
        }
        
        protected override void OnUpdate(IFsm<CharacterEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<CharacterEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);
            Debug.Log("离开攻击状态");
        }
    }
}

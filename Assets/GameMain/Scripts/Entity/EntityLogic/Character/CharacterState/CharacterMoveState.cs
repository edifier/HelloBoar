using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class CharacterMoveState : FsmState<CharacterEntity>
    {
        private static readonly int runHash = Animator.StringToHash("Run");

        protected override void OnInit(IFsm<CharacterEntity> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<CharacterEntity> fsm)
        {
            base.OnEnter(fsm);

            // 播放待机动画
            fsm.Owner.Animator.SetTrigger(runHash);
            Debug.Log("进入移动状态");
        }
        
        protected override void OnUpdate(IFsm<CharacterEntity> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<CharacterEntity> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            Debug.Log("离开移动状态");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class CharacterIdleState : FsmState<CharacterEntity>
    {
        private static readonly int speedHash = Animator.StringToHash("Idle");

        protected override void OnInit(IFsm<CharacterEntity> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<CharacterEntity> fsm)
        {
            base.OnEnter(fsm);

            // 播放待机动画
            fsm.Owner.Animator.SetTrigger(speedHash);
            Debug.Log("进入待机状态");
        }
        
        protected override void OnUpdate(IFsm<CharacterEntity> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<CharacterEntity> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            Debug.Log("离开待机状态");
        }
    }
}

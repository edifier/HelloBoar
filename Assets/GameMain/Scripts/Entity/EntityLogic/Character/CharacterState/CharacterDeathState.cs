using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class CharacterDeathState : FsmState<CharacterEntity>
    {
        private static readonly int deathHash = Animator.StringToHash("Death");

        protected override void OnInit(IFsm<CharacterEntity> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<CharacterEntity> fsm)
        {
            base.OnEnter(fsm);

            // 播放待机动画
            fsm.Owner.Animator.SetTrigger(deathHash);
            Debug.Log("进入死亡状态");
        }
        
        protected override void OnUpdate(IFsm<CharacterEntity> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<CharacterEntity> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            Debug.Log("离开死亡状态");
        }
    }
}

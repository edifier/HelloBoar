using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class CharacterIdleState : CharacterAttackDecision
    {
        private static readonly int IdleHash = Animator.StringToHash("Idle");

        protected override void OnInit(IFsm<CharacterEntity> _fsm)
        {
            base.OnInit(_fsm);
        }

        protected override void OnEnter(IFsm<CharacterEntity> _fsm)
        {
            base.OnEnter(_fsm);

            // 播放待机动画
            character.Animator.SetTrigger(IdleHash);
            // 监听摇杆激活事件
            GameEntry.Event.Subscribe(JoystickEventArgs.EventId, JoystickEvtStartHandle);
            if (character.IsDead && !character.isHide)
            {
                character.isHide = true;
                // 隐藏实体
                GameEntry.Entity.HideEntity(character);
            }
        }

        protected override void OnUpdate(IFsm<CharacterEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<CharacterEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);

            character.Animator.ResetTrigger(IdleHash);
            GameEntry.Event.Unsubscribe(JoystickEventArgs.EventId, JoystickEvtStartHandle);
        }

        private void JoystickEvtStartHandle(object sender, GameEventArgs e)
        {
            JoystickEventArgs ne = (JoystickEventArgs)e;
            // 获取摇杆控件状态
            if (ne.EventType == IJoystickEventType.Activated)
                ChangeState<CharacterMoveState>(fsm);
        }
    }
}

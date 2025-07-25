using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class CharacterMoveState : CharacterAttackDecision
    {
        private static readonly int runHash = Animator.StringToHash("Run");

        protected override void OnInit(IFsm<CharacterEntity> _fsm)
        {
            base.OnInit(_fsm);
        }

        protected override void OnEnter(IFsm<CharacterEntity> _fsm)
        {
            base.OnEnter(_fsm);

            // 播放待机动画
            _fsm.Owner.Animator.SetTrigger(runHash);
            // 监听摇杆闲置事件
            GameEntry.Event.Subscribe(JoystickEventArgs.EventId, JoystickEvtEndHandle);
        }

        protected override void OnUpdate(IFsm<CharacterEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            Vector2 direction = DataNodeExtension.GetInputJoystickDirection();
            // 转头&&向前走
            MoveHandle(direction);
        }

        protected override void OnLeave(IFsm<CharacterEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);

            character.Animator.ResetTrigger(runHash);
            GameEntry.Event.Unsubscribe(JoystickEventArgs.EventId, JoystickEvtEndHandle);
        }

        private void JoystickEvtEndHandle(object sender, GameEventArgs e)
        {
            JoystickEventArgs ne = (JoystickEventArgs)e;
            // 获取摇杆控件状态
            if (ne.EventType == IJoystickEventType.Deactivated)
                ChangeState<CharacterIdleState>(fsm);
        }
        
        public void MoveHandle(Vector2 _direction)
        {
            // 转头
            Vector3 _rotation = new Vector3(_direction.x, 0, _direction.y);
            character.transform.rotation = Quaternion.LookRotation(_rotation);
            // 向前走
            character.transform.Translate(Vector3.forward * character.MoveSpeed * Time.fixedDeltaTime);
        }
    }
}

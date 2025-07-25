using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    /// <summary>
    /// attack状态只在站立攻击时触发
    /// </summary>
    public class CharacterAttackState : CharacterSurvivalState
    {
        private static readonly int attackHash = Animator.StringToHash("Attack");
        private bool hasPerformDamage = false;

        protected override void OnEnter(IFsm<CharacterEntity> _fsm)
        {
            base.OnEnter(_fsm);

            // 播放攻击动画
            character.Animator.SetTrigger(attackHash);
            // 监听摇杆激活事件
            GameEntry.Event.Subscribe(JoystickEventArgs.EventId, JoystickEvtStartHandle);
        }

        protected override void OnUpdate(IFsm<CharacterEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            // 动画进行50%，结算伤害
            AnimatorStateInfo currentStateInfo = character.Animator.GetCurrentAnimatorStateInfo(0);
            if (currentStateInfo.normalizedTime >= .55f && !hasPerformDamage)
            {
                hasPerformDamage = true;
                // 这里受到伤害的实体是在WildBoarAlertState状态里添加好的
                PerformDamage(DetectTargetsInAttackRange());
            }

            // 动画结束，进入等待状态
            if (currentStateInfo.normalizedTime >= .95f)
                ChangeState<CharacterIdleState>(_fsm);
        }

        protected override void OnLeave(IFsm<CharacterEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);

            hasPerformDamage = false;
            character.inAttackProcess = false;
            character.Animator.ResetTrigger(attackHash);
            GameEntry.Event.Unsubscribe(JoystickEventArgs.EventId, JoystickEvtStartHandle);
        }

        private void JoystickEvtStartHandle(object sender, GameEventArgs e)
        {
            JoystickEventArgs ne = (JoystickEventArgs)e;
            // 获取摇杆控件状态
            if (ne.EventType == IJoystickEventType.Activated)
            {
                // 进入逃跑状态
                // 0.5秒内不进行攻击判定
                character.inEscapeProcess = true;
                ChangeState<CharacterMoveState>(fsm);
            }
        }
    }
}

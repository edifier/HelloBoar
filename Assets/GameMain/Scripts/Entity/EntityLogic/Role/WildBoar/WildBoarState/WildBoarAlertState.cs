using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class WildBoarAlertState : WildBoarBaseState
    {
        protected override void OnInit(IFsm<WildBoarEntity> _fsm)
        {
            base.OnInit(_fsm);
        }

        protected override void OnUpdate(IFsm<WildBoarEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            // 判断和character的距离
            var _distance = (ownerTs.position - wildBoar.character.transform.position).magnitude;
            // 和character的距离过远，不做后面的判断
            if (_distance > 20 || wildBoar.character.inDeathProcess || wildBoar.character.IsDead) return;

            // 检测攻击范围，在范围内攻击主角
            Vector3 rayOrigin = ownerTs.localPosition + rayOffset;
            int hitCount = Physics.SphereCastNonAlloc(rayOrigin, rayRadius, ownerTs.forward, hitInfo, rayDistance, 1 << attackableLayers.value);
            if (hitCount != 0 && !wildBoar.inAttackProcess)
            {
                wildBoar.inAttackProcess = true;
                if (!wildBoar.character.inDeathProcess)
                    // 进入攻击状态
                    ChangeState<WildBoarAttackState>(_fsm);
                else
                {
                    wildBoar.inAttackProcess = false;
                    // 进入站立状态
                    ChangeState<WildBoarIdleState>(_fsm);
                }
                // 不再做警戒校验
                return;
            }

            // 警戒中发现主角，进入移动状态跑向主角
            if (_distance < alertDistance && _distance > rayDistance)
            {
                bool isMoveState = _fsm.CurrentState is WildBoarMoveState;
                if (!isMoveState) ChangeState<WildBoarMoveState>(_fsm);
            }
        }
    }
}

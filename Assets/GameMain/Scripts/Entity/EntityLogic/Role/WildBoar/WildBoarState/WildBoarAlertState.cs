using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace GoodbyeWildBoar
{
    public class WildBoarAlertState : WildBoarBaseState
    {
        // 警戒距离
        private readonly static int alertDistance = 6;
        // 射线检测距离
        private float rayDistance = 0.6f;
        // 射线起点偏移（基于角色位置）
        private Vector3 rayOffset = new Vector3(0, 0.85f, 0);
        // 射线半径（球形检测） 
        private float rayRadius = 0.7f;
        private LayerMask attackableLayers;
        private RaycastHit[] hitInfo = new RaycastHit[1];
        private Transform ownerTs;

        protected override void OnInit(IFsm<WildBoarEntity> _fsm)
        {
            base.OnInit(_fsm);

            attackableLayers = Constant.Layer.CharacterLayerId;
            ownerTs = wildBoar.transform;
        }

        protected override void OnUpdate(IFsm<WildBoarEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            // 检测攻击范围，在范围内攻击主角
            Vector3 rayOrigin = ownerTs.localPosition + rayOffset;
            int hitCount = Physics.SphereCastNonAlloc(rayOrigin, rayRadius, ownerTs.forward, hitInfo, rayDistance, 1 << attackableLayers.value);
            if (hitCount != 0 && !wildBoar.inAttackProcess)
            {
                if (!wildBoar.character.inDeathProcess)
                    // 进入攻击状态
                    ChangeState<WildBoarAttackState>(_fsm);
                else
                    // 进入站立状态
                    ChangeState<WildBoarIdleState>(_fsm);
                // 不再做警戒校验
                return;
            }

            // 判断和character的距离
            var _distance = (ownerTs.position - wildBoar.character.transform.position).magnitude;
            // 警戒中发现主角，进入移动状态跑向主角
            if (_distance < alertDistance)
                ChangeState<WildBoarMoveState>(_fsm);
        }
    }
}

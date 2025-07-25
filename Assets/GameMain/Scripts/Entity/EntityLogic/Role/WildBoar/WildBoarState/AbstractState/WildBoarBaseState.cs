using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public abstract class WildBoarBaseState : FsmState<WildBoarEntity>
    {
        /// <summary>
        /// 提供给自定义方法内使用
        /// </summary>
        protected IFsm<WildBoarEntity> fsm;
        protected WildBoarEntity wildBoar;
        protected float timer = 0;
        protected Transform ownerTs;

        // 警戒距离
        protected readonly static float alertDistance = 5f;
        // 射线检测距离
        protected float rayDistance = 1.2f;
        // 射线起点偏移（基于角色位置）
        protected Vector3 rayOffset = new Vector3(0, 0.85f, 0);
        // 射线半径（球形检测） 
        protected float rayRadius = 0.7f;
        protected LayerMask attackableLayers;
        protected RaycastHit[] hitInfo = new RaycastHit[1];

        protected override void OnInit(IFsm<WildBoarEntity> _fsm)
        {
            base.OnInit(_fsm);
            fsm = _fsm;
            wildBoar = _fsm.Owner;

            attackableLayers = Constant.Layer.CharacterLayerId;
            ownerTs = wildBoar.transform;
        }

        protected override void OnEnter(IFsm<WildBoarEntity> _fsm)
        {
            base.OnEnter(_fsm);

            timer = 0;
        }

        protected override void OnUpdate(IFsm<WildBoarEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);
        }

        protected int DetectTargetsInSphereCast()
        {
            Vector3 rayOrigin = ownerTs.localPosition + rayOffset;
            return Physics.SphereCastNonAlloc(rayOrigin, rayRadius, ownerTs.forward, hitInfo, rayDistance, 1 << attackableLayers.value);
        }
    }
}

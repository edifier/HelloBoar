using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class CharacterAttackDecision : CharacterBaseState
    {
        // 射线检测距离
        private float rayDistance = 0.6f;
        // 射线起点偏移（基于角色位置）
        private Vector3 rayOffset = new Vector3(0, 0.85f, 0);
        // 射线半径（球形检测） 
        private float rayRadius = 0.7f;
        // 可攻击对象的层级
        private LayerMask attackableLayers;
        private Transform ownerTs;
        private RaycastHit[] hitInfo = new RaycastHit[Constant.SurvivalGame.WildBoarMaxNum];

        protected override void OnInit(IFsm<CharacterEntity> _fsm)
        {
            base.OnInit(_fsm);

            attackableLayers = Constant.Layer.EnemyLayerId;
            ownerTs = character.transform;
        }
        protected override void OnUpdate(IFsm<CharacterEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            // 进行攻击判定
            // 计算射线起点和方向
            Vector3 rayOrigin = ownerTs.localPosition + rayOffset;
            int hitCount = Physics.SphereCastNonAlloc(rayOrigin, rayRadius, ownerTs.forward, hitInfo, rayDistance, 1 << attackableLayers.value);
            if (hitCount != 0 && !character.inAttackProcess)
            {
                // 被攻击实体列表赋值
                character.SetHitEntityList(hitInfo);
                // 如果有被攻击实体，进入攻击状态
                if (character.hitEntityList.Count > 0)
                    // 进入攻击状态
                    ChangeState<CharacterAttackState>(_fsm);
            }
        }
    }
}


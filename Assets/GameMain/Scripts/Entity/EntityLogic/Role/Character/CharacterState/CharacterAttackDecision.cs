using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class CharacterAttackDecision : CharacterBaseState
    {
        // 射线检测距离
        private float rayDistance = 0.6f;
        // 射线起点偏移（基于角色位置）
        private Vector3 rayOffset = new Vector3(0, 0.85f, 0f);
        // 射线半径（球形检测） 
        private float rayRadius = 0.7f;
        // 攻击角度判断
        private float attackAngleThreshold = 60f;
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
                character.inAttackProcess = true;
                // 被攻击实体列表赋值
                character.hitEntityList.Clear();
                for (int i = 0; i < hitInfo.Length; i++)
                {
                    RaycastHit item = hitInfo[i];
                    if (item.collider != null)
                    {
                        WildBoarEntity wildBoar = item.collider.GetComponent<WildBoarEntity>();
                        // 攻击判断
                        if (IsTargetInFront(wildBoar.transform) && !wildBoar.inDeathProcess)
                            character.hitEntityList.Add(item.collider.GetComponent<Entity>());
                        else character.inAttackProcess = false;
                    }
                }

                // 如果有被攻击实体，进入攻击状态
                if (character.hitEntityList.Count > 0)
                    // 进入攻击状态
                    ChangeState<CharacterAttackState>(_fsm);
            }
        }

        private bool IsTargetInFront(Transform target)
        {
            // 计算到目标的向量
            Vector3 toTarget = target.position - ownerTs.position;
            toTarget.y = 0; // 忽略Y轴差异
            
            // 标准化向量
            Vector3 directionToTarget = toTarget.normalized;
            Vector3 ownerForward = ownerTs.forward;
            ownerForward.y = 0;
            ownerForward.Normalize();
            
            // 计算点积（cosθ）
            float dotProduct = Vector3.Dot(ownerForward, directionToTarget);
            
            // 计算角度阈值（cos(attackAngleThreshold/2)）
            float angleThreshold = Mathf.Cos(attackAngleThreshold * 0.5f * Mathf.Deg2Rad);
            
            // 检查目标是否在攻击角度范围内
            return dotProduct > angleThreshold;
        }
    }
}


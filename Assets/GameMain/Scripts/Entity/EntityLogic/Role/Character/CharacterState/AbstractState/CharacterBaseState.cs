using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public abstract class CharacterBaseState : FsmState<CharacterEntity>
    {
        // 提供给自定义方法内使用
        public IFsm<CharacterEntity> fsm;
        protected CharacterEntity character;
        protected Transform ownerTs;
        protected float timer;

        // 射线检测距离
        protected readonly static float rayDistance = 0.6f;
        // 射线起点偏移（基于角色位置）
        private Vector3 rayOffset = new Vector3(0, 0.85f, 0f);
        // 射线半径（球形检测） 
        private readonly static float rayRadius = 0.7f;
        // 可攻击对象的层级
        private LayerMask attackableLayers;
        
        protected RaycastHit[] hitInfo = new RaycastHit[Constant.SurvivalGame.WildBoarMaxCount];

        // 攻击角度判断
        private readonly static float attackAngleThreshold = 120f;

        protected override void OnInit(IFsm<CharacterEntity> _fsm)
        {
            base.OnInit(_fsm);
            fsm = _fsm;
            character = _fsm.Owner;
            ownerTs = character.transform;
            attackableLayers = Constant.Layer.EnemyLayerId;
        }

        protected override void OnEnter(IFsm<CharacterEntity> _fsm)
        {
            base.OnEnter(_fsm);

            timer = 0;
        }

        protected void PerformDamage(List<Entity> _hitEntityList)
        {
            if (_hitEntityList.Count == 0) return;

            foreach (Entity hitEntity in _hitEntityList)
                AIUtility.PerformDamage(character, hitEntity);
        }
        
        /// <summary>
        /// 校验攻击目标，并保存在character.hitEntityList中
        /// 调用之前先调用DetectTargetsInAttackRange
        /// </summary>
        protected List<Entity> DetectTargetsInAttackRange()
        {
            List<Entity> res = new();

            float _rayDistance = fsm.CurrentState is CharacterMoveState ? 1.2f : rayDistance;
            Vector3 rayOrigin = ownerTs.localPosition + rayOffset;
            int count = Physics.SphereCastNonAlloc(rayOrigin, rayRadius, ownerTs.forward, hitInfo, _rayDistance, 1 << attackableLayers.value);
            if (count == 0)
            {
                return res;
            }

            // 被攻击实体列表赋值
            // 这里要循环count
            for (int i = 0; i < count; i++)
            {
                RaycastHit item = hitInfo[i];
                if (item.collider == null) continue;
                WildBoarEntity wildBoar = item.collider.GetComponent<WildBoarEntity>();
                // 攻击判断
                if (IsTargetInFront(wildBoar.transform) && !wildBoar.IsDead)
                    res.Add(item.collider.GetComponent<Entity>());
            }

            return res;
        }

        protected bool IsTargetInFront(Transform target)
        {
            // 计算到目标的向量
            Vector3 toTarget = target.position - ownerTs.position;
            // 忽略Y轴差异
            toTarget.y = 0; 

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

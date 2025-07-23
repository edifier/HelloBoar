using UnityEngine;
using GameFramework.Fsm;

namespace GoodbyeWildBoar
{
    public class WildBoarMoveState : WildBoarAlertState
    {
        private static readonly int runHash = Animator.StringToHash("Run");

        // 移动强度
        private float moveForce = 2f;
        private float rotationSpeed = 10f;
        private float separationForce = 5f;
        private float separationRadius = .4f;
        private readonly static float resetTimeDuration = 2.5f;
        private readonly static float resetTimeDeterminationTime = 4f;

        private Rigidbody rb;
        private Transform characterTs;
        private Collider[] overlapResults = new Collider[Constant.SurvivalGame.WildBoarMaxNum];

        protected override void OnInit(IFsm<WildBoarEntity> _fsm)
        {
            base.OnInit(_fsm);

            rb = ownerTs.GetComponent<Rigidbody>();
            characterTs = wildBoar.character.transform;
        }

        protected override void OnEnter(IFsm<WildBoarEntity> _fsm)
        {
            base.OnEnter(_fsm);

            // 播放待机动画
            wildBoar.Animator.SetTrigger(runHash);
            timer = 0f;
        }

        protected override void OnUpdate(IFsm<WildBoarEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            // 追逐一段时间后进入休息状态
            timer += elapseSeconds;
            if (timer > resetTimeDeterminationTime)
            {
                timer = 0f;
                wildBoar.resetTime = resetTimeDuration;
                ChangeState<WildBoarIdleState>(_fsm);
                return;
            }

            // 应用分离力防止重叠
            // ApplySeparationForce();

            // 调整朝向
            SmoothRotation();

            // 沿面朝方向施加力
            ApplyForwardForce();
        }

        protected override void OnLeave(IFsm<WildBoarEntity> _fsm, bool isShutdown)
        {
            base.OnLeave(_fsm, isShutdown);

            wildBoar.Animator.ResetTrigger(runHash);
            // 取消所有力
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            timer = 0;
        }

        private void ApplySeparationForce()
        {
            LayerMask layer = Constant.Layer.EnemyLayerId;
            int nearbyCount = Physics.OverlapSphereNonAlloc(ownerTs.position, separationRadius, overlapResults, 1 << layer.value);

            Vector3 repulsion = Vector3.zero;
            int count = 0;

            foreach (Collider other in overlapResults)
            {
                // 排除自己
                if (other.gameObject == ownerTs.gameObject) continue;

                Vector3 toOther = ownerTs.position - other.transform.position;
                float distance = toOther.magnitude;

                if (distance > 0)
                {
                    repulsion += toOther.normalized / distance;
                    count++;
                }
            }

            if (count > 0)
            {
                repulsion /= count;
                rb.AddForce(repulsion * separationForce, ForceMode.VelocityChange);
            }
        }

        private void ApplyForwardForce()
        {
            rb.AddForce(ownerTs.forward * moveForce, ForceMode.Impulse);
            if (rb.velocity.magnitude > wildBoar.MoveSpeed)
                rb.velocity = rb.velocity.normalized * moveForce;
        }

        private void SmoothRotation()
        {
            Vector3 targetDirection = (characterTs.position - ownerTs.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            ownerTs.rotation = Quaternion.Slerp(
                ownerTs.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }
}


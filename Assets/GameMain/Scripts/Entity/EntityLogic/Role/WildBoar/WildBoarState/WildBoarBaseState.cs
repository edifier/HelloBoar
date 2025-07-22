using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class WildBoarBaseState : FsmState<WildBoarEntity>
    {
        /// <summary>
        /// 提供给自定义方法内使用
        /// </summary>
        protected IFsm<WildBoarEntity> fsm;
        protected WildBoarEntity wildBoar;

        protected override void OnInit(IFsm<WildBoarEntity> _fsm)
        {
            base.OnInit(_fsm);
            fsm = _fsm;
            wildBoar = _fsm.Owner;
        }

        protected override void OnUpdate(IFsm<WildBoarEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            if (wildBoar.IsDead && !wildBoar.inDeathProcess)
            {
                wildBoar.inDeathProcess = true;
                ChangeState<WildBoarDeathState>(_fsm);
            }
        }
    }
}

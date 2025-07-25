using GameFramework.Fsm;

namespace GoodbyeWildBoar
{
    public abstract class WildBoarSurvivalState : WildBoarBaseState
    {
        protected override void OnUpdate(IFsm<WildBoarEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            if (wildBoar.IsDead && !wildBoar.isHide)
            {
                if (_fsm.CurrentState is WildBoarDeathState) return;
                ChangeState<WildBoarDeathState>(_fsm);
            }
        }
    }
}

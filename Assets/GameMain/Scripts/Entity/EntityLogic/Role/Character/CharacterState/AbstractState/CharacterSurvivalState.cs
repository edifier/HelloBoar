using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public abstract class CharacterSurvivalState : CharacterBaseState {
        protected override void OnUpdate(IFsm<CharacterEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            if (character.IsDead)
            {
                if (_fsm.CurrentState is CharacterDeathState) return;
                ChangeState<CharacterDeathState>(_fsm);
            }
        }
    }
}

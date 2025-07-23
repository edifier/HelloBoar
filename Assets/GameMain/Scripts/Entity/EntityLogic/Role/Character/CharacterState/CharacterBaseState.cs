using GameFramework.Fsm;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class CharacterBaseState : FsmState<CharacterEntity>
    {
        /// <summary>
        /// 提供给自定义方法内使用
        /// </summary>
        public IFsm<CharacterEntity> fsm;
        protected CharacterEntity character;

        protected override void OnInit(IFsm<CharacterEntity> _fsm)
        {
            base.OnInit(_fsm);
            fsm = _fsm;
            character = _fsm.Owner;
        }

        protected override void OnUpdate(IFsm<CharacterEntity> _fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(_fsm, elapseSeconds, realElapseSeconds);

            if (character.IsDead && !character.inDeathProcess)
            {
                if (fsm.CurrentState is CharacterDeathState) return;
                character.inDeathProcess = true;
                ChangeState<CharacterDeathState>(_fsm);
            }
        }
    }
}

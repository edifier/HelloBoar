using System.Collections;
using System.Collections.Generic;
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

        protected override void OnInit(IFsm<CharacterEntity> _fsm)
        {
            base.OnInit(_fsm);
            fsm = _fsm;
        }
    }
}

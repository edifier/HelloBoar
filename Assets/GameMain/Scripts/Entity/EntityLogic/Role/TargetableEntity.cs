//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityGameFramework.Runtime;
using UnityEngine;

namespace GoodbyeWildBoar
{
    /// <summary>
    /// 可作为目标的实体类。
    /// </summary>
    public abstract class TargetableEntity : Entity
    {
        private TargetableObjectData m_TargetableObjectData = null;

        public bool IsDead
        {
            get
            {
                return m_TargetableObjectData.HP <= 0;
            }
        }

        public abstract ImpactData GetImpactData();

        public void ApplyDamage(int damageHP)
        {
            float fromHPRatio = m_TargetableObjectData.HPRatio;
            m_TargetableObjectData.HP -= damageHP;
            float toHPRatio = m_TargetableObjectData.HPRatio;

            Debug.Log(m_TargetableObjectData.HP);
            if (fromHPRatio > toHPRatio)
                GameEntry.HPBar.ShowHPBar(this, fromHPRatio, toHPRatio);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_TargetableObjectData = userData as TargetableObjectData;
            if (m_TargetableObjectData == null)
            {
                Log.Error("Targetable object data is invalid.");
                return;
            }
        }
    }
}

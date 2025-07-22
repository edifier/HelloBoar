using System;
using GameFramework.DataTable;

using UnityEngine;

namespace GoodbyeWildBoar
{
    [Serializable]
    public class WildBoarData : TargetableObjectData
    {
        // 计算各种加成后的最大生命值
        private int m_MaxHP = 0;
        // 计算各种加成后的最终攻击力
        private int m_Attack = 0;
        // 计算各种加成后的最终护甲
        private int m_Defense = 0;
        // 移动速度
        private float m_Speed;
        // 死亡音效ID
        private int m_DeadSoundId = 0;
        // 掉落的肉
        private int m_MeatDroppedAmount = 0;
        // 掉落的金币
        private int m_GoldDroppedAmount = 0;
        // 角色名字
        private string m_Name;

        public WildBoarData(int entityId, int typeId) : base(entityId, typeId, CampType.Enemy)
        {
            IDataTable<DRWildBoar> dtWildBoar = GameEntry.DataTable.GetDataTable<DRWildBoar>();
            DRWildBoar drWildBoar = dtWildBoar.GetDataRow(typeId);
            if (drWildBoar == null) return;

            m_DeadSoundId = drWildBoar.DeadSoundId;
            m_Attack = drWildBoar.Attack;
            m_MaxHP = drWildBoar.MaxHP;
            m_Speed = drWildBoar.Speed;
            m_MeatDroppedAmount = drWildBoar.MeatDroppedAmount;
            m_GoldDroppedAmount = drWildBoar.GoldDroppedAmount;
            HP = m_MaxHP;
        }

        /// <summary>
        /// 最终攻击力
        /// </summary>
        public int Attack
        {
            get
            {
                return m_Attack;
            }
            set
            {
                m_Attack = value;
            }
        }

        /// <summary>
        /// 最终防御力
        /// </summary>
        public int Defense
        {
            get
            {
                return m_Defense;
            }
            set
            {
                m_Defense = value;
            }
        }

        /// <summary>
        /// 死亡音效ID
        /// </summary>
        public int DeadSoundId
        {
            get
            {
                return m_DeadSoundId;
            }
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public override int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        /// <summary>
        /// 野猪追击玩家时候的速度
        /// </summary>
        public float Speed
        {
            get
            {
                return m_Speed;
            }
            set
            {
                m_Speed = value;
            }
        }

        /// <summary>
        /// 掉落的肉的数量
        /// </summary>
        public int MeatDroppedAmount
        {
            get
            {
                return m_MeatDroppedAmount;
            }
            set
            {
                m_MeatDroppedAmount = value;
            }
        }

        /// <summary>
        /// 掉落的金币的数量
        /// </summary>
        public int GoldDroppedAmount
        {
            get
            {
                return m_GoldDroppedAmount;
            }
            set
            {
                m_GoldDroppedAmount = value;
            }
        }
    }
}

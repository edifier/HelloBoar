using System;
using System.Collections.Generic;
using GameFramework.DataTable;

using UnityEngine;

namespace GoodbyeWildBoar
{
    [Serializable]
    public class CharacterData : TargetableObjectData
    {
        // 武器列表
        private List<WeaponData> m_WeaponDatas = new List<WeaponData>();
        // 护甲列表
        private List<ArmorData> m_ArmorDatas = new List<ArmorData>();
        // 计算各种加成后的最大生命值
        private int m_MaxHP = 0;
        // 计算各种加成后的最终攻击力
        private int m_Attack = 0;
        // 计算各种加成后的最终护甲
        private int m_Defense = 0;
        // 死亡音效ID
        private int m_DeadSoundId = 0;
        // 角色名字
        private string m_Name;

        public CharacterData(int entityId, int typeId) : base(entityId, typeId, CampType.Player)
        {
            IDataTable<DRCharacter> dtCharacter = GameEntry.DataTable.GetDataTable<DRCharacter>();
            DRCharacter drCharacter = dtCharacter.GetDataRow(TypeId);
            if (drCharacter == null) return;

            for (int index = 0, weaponId = 0; (weaponId = drCharacter.GetWeaponIdAt(index)) > 0; index++)
            {
                AttachWeaponData(new WeaponData(GameEntry.Entity.GeneratePositiveSerialId(), weaponId, Id, CampType.Player));
            }

            for (int index = 0, armorId = 0; (armorId = drCharacter.GetArmorIdAt(index)) > 0; index++)
            {
                AttachArmorData(new ArmorData(GameEntry.Entity.GeneratePositiveSerialId(), armorId, Id, CampType.Player));
            }

            m_DeadSoundId = drCharacter.DeadSoundId;
            // 初始最大生命值
            m_MaxHP += drCharacter.HP;
            // 初始化时最大生命即当前生命
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

        public void AttachWeaponData(WeaponData weaponData)
        {
            if (weaponData == null || m_WeaponDatas.Contains(weaponData)) return;

            m_WeaponDatas.Add(weaponData);
            RefreshData();
        }

        public void DetachWeaponData(WeaponData weaponData)
        {
            if (weaponData == null) return;

            m_WeaponDatas.Remove(weaponData);
            RefreshData();
        }

        public List<ArmorData> GetAllArmorDatas()
        {
            return m_ArmorDatas;
        }

        public void AttachArmorData(ArmorData armorData)
        {
            if (armorData == null || m_ArmorDatas.Contains(armorData)) return;

            m_ArmorDatas.Add(armorData);
            RefreshData();
        }

        public void DetachArmorData(ArmorData armorData)
        {
            if (armorData == null) return;

            m_ArmorDatas.Remove(armorData);
            RefreshData();
        }

        private void RefreshData()
        {
            m_MaxHP = 0;
            m_Attack = 0;
            m_Defense = 0;
            for (int i = 0; i < m_ArmorDatas.Count; i++)
            {
                m_MaxHP += m_ArmorDatas[i].MaxHP;
                m_Attack += m_WeaponDatas[i].Attack;
                m_Defense += m_ArmorDatas[i].Defense;
            }

            if (HP > m_MaxHP)
                HP = m_MaxHP;
        }
    }
}

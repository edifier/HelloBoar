//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GoodbyeWildBoar
{
    public class HPBarComponent : GameFrameworkComponent
    {
        [SerializeField]
        private HPBarItem m_HPBarItemTemplate = null;

        [SerializeField]
        private Transform m_HPBarInstanceRoot = null;

        private int m_InstancePoolCapacity = 16;

        private IObjectPool<HPBarItemObject> m_HPBarItemObjectPool = null;
        private List<HPBarItem> m_ActiveHPBarItems = null;
        private Canvas m_CachedCanvas = null;

        private void Start()
        {
            if (m_HPBarInstanceRoot == null)
            {
                Log.Error("You must set HP bar instance root first.");
                return;
            }

            m_CachedCanvas = m_HPBarInstanceRoot.GetComponent<Canvas>();
            m_HPBarItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<HPBarItemObject>("HPBarItem", m_InstancePoolCapacity);
            m_ActiveHPBarItems = new List<HPBarItem>();
        }

        /// <summary>
        /// 显示血条并初始化血量
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fromHPRatio"></param>
        /// <param name="toHPRatio"></param>
        public void ShowHPBar(Entity entity, float fromHPRatio, float toHPRatio)
        {
            if (entity == null)
            {
                Log.Warning("Entity is invalid.");
                return;
            }

            HPBarItem hpBarItem = GetActiveHPBarItem(entity);
            if (hpBarItem == null)
            {
                hpBarItem = CreateHPBarItem(entity);
                m_ActiveHPBarItems.Add(hpBarItem);
            }

            hpBarItem.Init(entity, m_CachedCanvas, fromHPRatio, toHPRatio);
        }

        /// <summary>
        /// 通过实体引用隐藏血条
        /// </summary>
        /// <param name="entity"></param>
        public void HideHPBar(Entity entity)
        {
            HPBarItem hpBarItem = GetActiveHPBarItem(entity);
            if (hpBarItem == null) return;
            m_ActiveHPBarItems.Remove(hpBarItem);
            m_HPBarItemObjectPool.Unspawn(hpBarItem);
            hpBarItem.Reset();
        }

        /// <summary>
        /// 隐藏全部血条
        /// </summary>
        public void HideAllHPBar()
        {
            for (int i = 0; i < m_ActiveHPBarItems.Count; i++)
            {
                HPBarItem hpBarItem = m_ActiveHPBarItems[i];
                m_HPBarItemObjectPool.Unspawn(hpBarItem);
                hpBarItem.Reset();
            }
            // 清空数组
            m_ActiveHPBarItems.Clear();
        }

        private HPBarItem GetActiveHPBarItem(Entity entity)
        {
            if (entity == null) return null;

            for (int i = 0; i < m_ActiveHPBarItems.Count; i++)
            {
                if (m_ActiveHPBarItems[i].Owner == entity)
                {
                    return m_ActiveHPBarItems[i];
                }
            }

            return null;
        }

        private HPBarItem CreateHPBarItem(Entity entity)
        {
            HPBarItem hpBarItem = null;
            HPBarItemObject hpBarItemObject = m_HPBarItemObjectPool.Spawn();
            if (hpBarItemObject != null)
            {
                hpBarItem = (HPBarItem)hpBarItemObject.Target;
            }
            else
            {
                hpBarItem = Instantiate(m_HPBarItemTemplate);
                Transform transform = hpBarItem.GetComponent<Transform>();
                transform.SetParent(m_HPBarInstanceRoot);
                transform.localScale = Vector3.one;
                m_HPBarItemObjectPool.Register(HPBarItemObject.Create(hpBarItem), true);
            }

            return hpBarItem;
        }
    }
}

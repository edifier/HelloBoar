//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GoodbyeWildBoar
{
    /// <summary>
    /// 游戏模式。
    /// </summary>
    public enum GameMode : byte
    {
        /// <summary>
        /// 生存模式。
        /// </summary>
        Survival,
    }

    public abstract class GameBase
    {
        public abstract GameMode GameMode
        {
            get;
        }

        public bool GameOver
        {
            get;
            protected set;
        }

        // private MyAircraft m_MyAircraft = null;

        public virtual void Initialize()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);

            // 展示飞机
            // GameEntry.Entity.ShowMyAircraft(new MyAircraftData(GameEntry.Entity.GenerateSerialId(), 10000)
            // {
            //     Name = "My Aircraft",
            //     Position = Vector3.zero,
            // });

            GameOver = false;
        }

        public virtual void Shutdown()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        }

        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {
            // if (m_MyAircraft != null && m_MyAircraft.IsDead)
            // {
            //     GameOver = true;
            //     return;
            // }
        }

        protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            // ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            // if (ne.EntityLogicType == typeof(MyAircraft))
            // {
            //     m_MyAircraft = (MyAircraft)ne.Entity.Logic;
            // }
        }

        protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }
    }
}

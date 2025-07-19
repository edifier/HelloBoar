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

        private bool gameHasInit;
        private float timer = 0f;

        protected const float gameReadyDelayedSeconds = Constant.Layer.EnterMainSceneDuration;

        public virtual void Initialize()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);

            GameOver = false;
        }

        public virtual void Shutdown()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        }

        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (!gameHasInit)
            {
                timer += elapseSeconds;
                if (timer >= gameReadyDelayedSeconds)
                {
                    gameHasInit = true;
                    // 展示游戏内UI界面
                    GameEntry.UI.OpenUIIngameInterface(UIFormId.IngameInterface);
                    timer = 0f;
                }
            }
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

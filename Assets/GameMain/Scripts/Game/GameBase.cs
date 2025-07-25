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

        // 通用内容加载完成
        protected bool universalContentComplete;
        // 计时器
        protected float timer = 0f;

        /// <summary>
        /// 主角引用
        /// </summary>
        protected CharacterEntity m_Character = null;
        /// <summary>
        /// 目前只有主场景
        /// todo: 未来如果有多个场景，再拓展
        /// </summary>
        protected MainAssistant mainAssistant;

        protected const float gameReadyDelayedSeconds = Constant.UI.EnterMainSceneDuration;

        public virtual void Initialize()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);

            timer = 0f;
            GameOver = false;
            m_Character = null;
            universalContentComplete = false;
            // 初始化CharacterEntityId的路径，避免找不到路径问题
            DataNodeExtension.SetCharacterEntityId(-1);
            // 获取场景助手的引用
            mainAssistant = Object.FindObjectOfType<MainAssistant>();
        }

        public virtual void Shutdown()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        }

        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {
            // 游戏未初始化
            if (!universalContentComplete)
            {
                timer += elapseSeconds;
                if (timer >= gameReadyDelayedSeconds)
                {
                    timer = 0f;
                    // 初始化主游戏场景
                    InitMainGameScene();
                }
            }

            // 游戏结束
            // UI场景关闭
            if (m_Character != null && m_Character.IsDead)
            {
                ExitScene();
            }
        }

        protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(CharacterEntity))
            {
                m_Character = (CharacterEntity)ne.Entity.Logic;
                // 保存characterId，通过id可以获取Character
                DataNodeExtension.SetCharacterEntityId(m_Character.Id);
            }
        }

        protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }

        private void InitMainGameScene()
        {
            // 展示游戏内UI界面
            GameEntry.UI.OpenUIIngameInterface(UIFormId.IngameInterface);
            // 展示主角
            GameEntry.Entity.ShowCharacter(new CharacterData(GameEntry.Entity.GeneratePositiveSerialId(), 10000)
            {
                Name = "AxeKnight",
                Position = Vector3.zero,
            });
            // 主屏幕初始化
            universalContentComplete = true;
        }

        protected virtual void ExitScene()
        {
            GameOver = true;
        }
    }
}

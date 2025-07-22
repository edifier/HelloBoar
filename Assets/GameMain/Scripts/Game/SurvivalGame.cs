//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using GameFramework.Entity;
using UnityEngine;

namespace GoodbyeWildBoar
{
    public class SurvivalGame : GameBase
    {
        private float m_ElapseSeconds = 0f;
        private IEntityGroup entityGroup = null;
        private int maximum = Constant.SurvivalGame.WildBoarMaxNum;

        public override GameMode GameMode
        {
            get
            {
                return GameMode.Survival;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            // 通过组名获取实体组实例
            entityGroup = GameEntry.Entity.GetEntityGroup("WildBoar");
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            m_ElapseSeconds += elapseSeconds;
            if (m_ElapseSeconds >= Constant.SurvivalGame.InitWildBoarInterval && entityGroup.EntityCount < maximum)
            {
                m_ElapseSeconds = 0f;
                ShowWildBoar();
            }
        }

        private void ShowWildBoar()
        {
            IDataTable<DRWildBoar> dtWildBoar = GameEntry.DataTable.GetDataTable<DRWildBoar>();
            float randomPositionX = mainAssistant.enemySpawnBoundary.bounds.min.x + mainAssistant.enemySpawnBoundary.bounds.size.x * (float)Utility.Random.GetRandomDouble();
            float randomPositionZ = mainAssistant.enemySpawnBoundary.bounds.min.z + mainAssistant.enemySpawnBoundary.bounds.size.z * (float)Utility.Random.GetRandomDouble();
            GameEntry.Entity.ShowWildBoar(
                new WildBoarData(GameEntry.Entity.GeneratePositiveSerialId(), 60000 + Utility.Random.GetRandom(dtWildBoar.Count))
                {
                    Position = new Vector3(randomPositionX, 0f, randomPositionZ),
                }
            );
        }
    }
}

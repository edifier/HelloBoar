//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using UnityEngine;
using GameFramework.Entity;

namespace GoodbyeWildBoar
{
    public class SurvivalGame : GameBase
    {
        private IEntityGroup wildBoarGroup;
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

            // 配置实体池容量
            InitInstanceCapacity();
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            if (!universalContentComplete) return;

            timer += elapseSeconds;
            if (timer > Constant.SurvivalGame.GenerateWildBoarInterval && wildBoarGroup.EntityCount < Constant.SurvivalGame.WildBoarMaxCount)
            {
                timer = 0f;
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

        private void InitInstanceCapacity()
        {
            IEntityManager entityManager = GameFrameworkEntry.GetModule<IEntityManager>();
            wildBoarGroup = entityManager.GetEntityGroup("WildBoar");
            wildBoarGroup.InstanceCapacity = Constant.SurvivalGame.WildBoarMaxCount;
        }

        protected override void ExitScene()
        {
            // 关闭interface
            GameEntry.UI.CloseAllLoadedUIForms();
            // 关闭所有血条
            GameEntry.HPBar.HideAllHPBar();
            // 退出场景
            base.ExitScene();
        }
    }
}

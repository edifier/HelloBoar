using Unity.VisualScripting;

namespace GoodbyeWildBoar {
    public static partial class Constant
    {
        public static class UI
        {
            /// <summary>
            /// 进入渲染主场景的等待时间
            /// 这个时间不能比MenuSceneUI消失的时间长
            /// </summary>
            public const float EnterMainSceneDuration = 0.3f;
        }

        public static class SurvivalGame
        {
            /// <summary>
            /// Entity检查器中配置的Instance Capacity的值
            /// </summary>
            public const int WildBoarMaxCount = 12;
            /// <summary>
            /// 产生野猪的间隔时间
            /// </summary>
            public const float GenerateWildBoarInterval = 1.5f;
        }
    }
}

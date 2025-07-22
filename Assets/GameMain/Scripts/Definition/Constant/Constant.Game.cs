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
            public const int WildBoarMaxNum = 1;
            public const float InitWildBoarInterval = 2f;
        }
    }
}

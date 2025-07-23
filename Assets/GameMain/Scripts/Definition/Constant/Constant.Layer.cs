using UnityEngine;

namespace GoodbyeWildBoar
{
    public static partial class Constant
    {
        /// <summary>
        /// 层。
        /// </summary>
        public static class Layer
        {
            public const string DefaultLayerName = "Default";
            public static readonly int DefaultLayerId = LayerMask.NameToLayer(DefaultLayerName);

            public const string UILayerName = "UI";
            public static readonly int UILayerId = LayerMask.NameToLayer(UILayerName);

            public const string TargetableObjectLayerName = "Targetable Object";
            public static readonly int TargetableObjectLayerId = LayerMask.NameToLayer(TargetableObjectLayerName);

            public const string CharacterLayerName = "Character";
            public static readonly int CharacterLayerId = LayerMask.NameToLayer(CharacterLayerName);

            public const string EnemyLayerName = "Enemy";
            public static readonly int EnemyLayerId = LayerMask.NameToLayer(EnemyLayerName);
        }
    }
}

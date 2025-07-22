using UnityEngine;
using UnityGameFramework.Runtime;

namespace GoodbyeWildBoar
{
    /// <summary>
    /// 对GameEntry.DataNode的操作，统一在这里管理
    /// 方便查询node节点路径
    /// </summary>
    public static class DataNodeExtension
    {
        /// <summary>
        /// 保存characterId
        /// </summary>
        /// <param name="_id">角色实体ID</param>
        public static void SetCharacterEntityId(int _id)
        {
            GameEntry.DataNode.SetData("Character.Entity.id", new VarInt32()
            {
                Value = _id
            });
        }

        /// <summary>
        /// 获取Character的id，通过id可以获取Character
        /// </summary>
        /// <returns></returns>
        public static int GetCharacterEntityId()
        {
            return GameEntry.DataNode.GetData<VarInt32>("Character.Entity.id");
        }

        /// <summary>
        /// 保存摇杆输入方向
        /// </summary>
        /// <param name="_pos">location.normalized</param>
        public static void SetInputJoystickDirection(Vector2 _pos)
        {
            GameEntry.DataNode.SetData("Input.Joystick.Direction", new VarVector2()
            {
                Value = _pos
            });
        }

        /// <summary>
        /// 获取摇杆输入方向
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetInputJoystickDirection()
        {
            return GameEntry.DataNode.GetData<VarVector2>("Input.Joystick.Direction");
        }

        /// <summary>
        /// 保存摇杆输入角度
        /// </summary>
        /// <param name="_angle">角度</param>
        public static void SetInputJoystickDirectionAngle(float _angle)
        {
            GameEntry.DataNode.SetData("Input.Joystick.DirectionAngle", new VarSingle()
            {
                Value = _angle
            });
        }

        /// <summary>
        /// 获取摇杆输入角度
        /// </summary>
        /// <returns></returns>
        public static float GetInputJoystickDirectionAngle()
        {
            return GameEntry.DataNode.GetData<VarSingle>("Input.Joystick.DirectionAngle");
        }
    }
}

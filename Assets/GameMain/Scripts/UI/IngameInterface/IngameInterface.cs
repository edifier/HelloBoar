using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace GoodbyeWildBoar
{
    public class IngameInterface : UGuiForm, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public float R; //半径
        private RectTransform joystickPos;
        private RectTransform joystickBGPos;
        private bool dragOver;
        private Vector2 outPos;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            joystickBGPos = transform.GetChild(0) as RectTransform;
            joystickPos = joystickBGPos.GetChild(0) as RectTransform;

            // 初始值
            SetJoystickDataNode(Vector2.zero);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            // 还原音乐
            GameEntry.Sound.PlayMusic(1);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!dragOver) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickBGPos, eventData.position, eventData.enterEventCamera, out outPos
            );
            float S = Vector2.Distance(Vector2.zero, outPos);
            if (S > R)
                outPos = outPos.normalized * R;

            joystickPos.localPosition = outPos;
            // 保存在全局，供player做移动
            SetJoystickDataNode(outPos);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform as RectTransform, eventData.pressPosition, eventData.enterEventCamera, out outPos
            );

            dragOver = true;
            joystickBGPos.localPosition = outPos;
            // 触发摇杆开始拖拽事件
            GameEntry.Event.Fire(this, JoystickEventArgs.Create(IJoystickEventType.Activated));
            outPos = Vector2.zero;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            dragOver = false;
            outPos = Vector2.zero;
            ResetJoystickPos(outPos);
            // 触发摇杆结束拖拽事件
            GameEntry.Event.Fire(this, JoystickEventArgs.Create(IJoystickEventType.Deactivated));
        }

        public void OnEnable()
        {
            dragOver = false;
        }

        public void OnDisable()
        {
            dragOver = false;
            outPos = Vector2.zero;
            ResetJoystickPos(outPos);
        }

        private void ResetJoystickPos(Vector2 _pos)
        {
            joystickPos.localPosition = _pos;
            if (Screen.width > Screen.height)
                joystickBGPos.anchoredPosition = new Vector2(232, 215);
            else
                joystickBGPos.anchoredPosition = new Vector2(547, 244);
        }

        private void SetJoystickDataNode(Vector2 _pos)
        {
            // 保存[-1, 1]的二维向量
            DataNodeExtension.SetInputJoystickDirection(_pos.normalized);
            // 保存0-360度的角度，[1, 0]方向为0度，[0, 1]方向为90度
            DataNodeExtension.SetInputJoystickDirectionAngle(Vector2ToAngle(_pos.normalized));
        }

        private static float Vector2ToAngle(Vector2 direction)
        {
            // 使用Mathf.Atan2计算角度（弧度）
            float radians = Mathf.Atan2(direction.y, direction.x);

            // 将弧度转换为角度（0-360度）
            float degrees = radians * Mathf.Rad2Deg;

            // 确保角度在0-360范围内
            if (degrees < 0) degrees += 360;

            return degrees;
        }
    }
}

using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GoodbyeWildBoar
{ 
    public class JoystickEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(JoystickEventArgs).GetHashCode();
        public override int Id => EventId;
        
        public IJoystickEventType EventType { get; private set; }
        
        public static JoystickEventArgs Create(IJoystickEventType eventType)
        {
            var e = ReferencePool.Acquire<JoystickEventArgs>();
            e.EventType = eventType;
            return e;
        }

        public override void Clear()
        {
            EventType = IJoystickEventType.Deactivated;
        }
    }
}

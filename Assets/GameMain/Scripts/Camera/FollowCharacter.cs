using UnityEngine;

namespace GoodbyeWildBoar
{
    public class FollowCharacter : MonoBehaviour
    {
        private CharacterEntity character = null;

        private Vector3 cermaOffset = Vector3.zero;
        private readonly static float smoothSpeed = 0.5f;

        private void Awake()
        {
            cermaOffset = transform.localPosition;
        }

        private void Update()
        {
            if (character == null)
            {
                int _id = DataNodeExtension.GetCharacterEntityId();
                if (_id != 0)
                {
                    Entity _entity = GameEntry.Entity.GetGameEntity(_id);
                    if (_entity != null)
                        character = _entity as CharacterEntity;
                }
            }
        }

        private void LateUpdate()
        {
            if (character != null)
            {
                Vector3 characterPos = character.transform.localPosition;
                Vector3 targetPosition = new Vector3(cermaOffset.x + characterPos.x, cermaOffset.y, cermaOffset.z + characterPos.z);
                // 执行移动
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            }
        }

        private void OnDestory()
        {
            character = null;
            cermaOffset = Vector3.zero;
        }
    }
}

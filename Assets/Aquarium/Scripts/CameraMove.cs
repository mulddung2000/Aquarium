using UnityEngine;

namespace Aquarium
{
    public class CameraMove : MonoBehaviour
    {
        public Transform target;          // 캐릭터
        public Vector3 offset = new Vector3(0, 5, -7);

        private Quaternion fixedRotation;

        void Start()
        {
            // 시작 시 카메라 회전을 고정
            fixedRotation = transform.rotation;
        }

        void LateUpdate()
        {
            // 위치만 따라감
            transform.position = target.position + offset;

            // 회전은 항상 고정
            transform.rotation = fixedRotation;
        }
    }
}
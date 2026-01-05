using UnityEngine;

namespace Aquarium
{
    public class FollowMouse : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BoxCollider aquariumCollider;   // 어항
        [SerializeField] private Transform visual;               // 물고기 외형 (자식)

        [Header("Swim Speed")]
        [SerializeField] private float patrolSpeed = 1.6f;      // 랜덤 유영 속도
        [SerializeField] private float followSpeed = 2.3f;      // 클릭 추적 속도
        [SerializeField] private float swimAccel = 0.3f;        // 가속/반응
        [SerializeField] private float slowDownDistance = 0.7f; // 감속 시작 거리

        [Header("Idle Patrol")]
        [SerializeField] private float patrolChangeTime = 3f;

        [Header("Swim Motion")]
        [SerializeField] private float verticalWaveHeight = 0.1f;
        [SerializeField] private float verticalWaveSpeed = 1.0f;

        [Header("Turn")]
        [SerializeField] private float turnSpeed = 3.5f;
        [SerializeField] private float maxPitch = 18f;
        [SerializeField] private float swayAngle = 4f;
        [SerializeField] private float swaySpeed = 1.5f;

        private Vector3 velocity;
        private Vector3 targetPosition;
        private Vector3 lastPosition;
        private bool isDragging;
        private float patrolTimer;

        // 물고기 크기 보정
        private BoxCollider fishBox;
        private Vector3 fishHalfSize;

        void Start()
        {
            targetPosition = transform.position;
            lastPosition = transform.position;
            patrolTimer = patrolChangeTime;

            fishBox = GetComponent<BoxCollider>();
            Vector3 scale = transform.lossyScale;
            fishHalfSize = Vector3.Scale(fishBox.size, scale) * 0.5f;
        }

        void Update()
        {
            HandleInput();
            HandleMovement();
            HandleRotation();
        }

        // ---------------- Input ----------------
        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0)) isDragging = true;
            if (Input.GetMouseButtonUp(0)) isDragging = false;
        }

        // ---------------- Movement ----------------
        private void HandleMovement()
        {
            RaycastHit hit;

            if (isDragging && IsMouseInsideAquarium(out hit))
            {
                // 클릭 추적도 헤엄치듯
                targetPosition = ClampInsideAquarium(hit.point);
                SwimMove(targetPosition, followSpeed);
            }
            else
            {
                // 랜덤 유영
                RandomPatrol();
                SwimMove(targetPosition, patrolSpeed);
            }
        }

        // ---------------- Swim Move (공용 헤엄 이동) ----------------
        private void SwimMove(Vector3 target, float baseSpeed)
        {
            Vector3 currentPos = transform.position;
            Vector3 toTarget = target - currentPos;
            float distance = toTarget.magnitude;

            if (distance < 0.001f)
                return;

            // 감속 처리
            float speedFactor = 1f;
            if (distance < slowDownDistance)
                speedFactor = Mathf.Lerp(0.2f, 1f, distance / slowDownDistance);

            float speed = Mathf.Min(baseSpeed, distance * 2f);
            Vector3 swimVelocity = toTarget.normalized * speed * speedFactor;

            // 상하 유영
            float waveY = Mathf.Sin(Time.time * verticalWaveSpeed) * verticalWaveHeight;
            Vector3 waveOffset = Vector3.up * waveY;

            transform.position = Vector3.SmoothDamp(
                currentPos,
                currentPos + swimVelocity + waveOffset,
                ref velocity,
                swimAccel
            );
        }

        // ---------------- Rotation & Turn ----------------
        private void HandleRotation()
        {
            Vector3 moveDir = transform.position - lastPosition;
            float speed = moveDir.magnitude;

            // 거의 안 움직일 때는 안정
            if (speed < 0.001f)
            {
                visual.localRotation = Quaternion.Slerp(
                    visual.localRotation,
                    Quaternion.identity,
                    Time.deltaTime * 2f
                );
                return;
            }

            moveDir.Normalize();

            Quaternion targetRot = Quaternion.LookRotation(moveDir);

            // Pitch 제한
            Vector3 euler = targetRot.eulerAngles;
            euler.x = ClampAngle(euler.x, -maxPitch, maxPitch);
            targetRot = Quaternion.Euler(euler);

            // 부드러운 턴
            visual.rotation = Quaternion.Slerp(
                visual.rotation,
                targetRot,
                turnSpeed * Time.deltaTime
            );

            // 속도 기반 미세 흔들림
            float swayStrength = Mathf.Clamp01(speed * 2f);
            float sway = Mathf.Sin(Time.time * swaySpeed) * swayAngle * swayStrength;
            visual.rotation *= Quaternion.Euler(0f, sway, 0f);

            lastPosition = transform.position;
        }

        // ---------------- Mouse Inside Aquarium ----------------
        private bool IsMouseInsideAquarium(out RaycastHit hitInfo)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (aquariumCollider.Raycast(ray, out hitInfo, 100f))
                return true;

            hitInfo = default;
            return false;
        }

        // ---------------- Random Patrol ----------------
        private void RandomPatrol()
        {
            patrolTimer -= Time.deltaTime;

            if (patrolTimer <= 0f)
            {
                targetPosition = GetSafeRandomPointInsideAquarium();
                patrolTimer = patrolChangeTime;
            }
        }

        // ---------------- Bounds ----------------
        private Vector3 ClampInsideAquarium(Vector3 pos)
        {
            Bounds b = aquariumCollider.bounds;

            return new Vector3(
                Mathf.Clamp(pos.x, b.min.x + fishHalfSize.x, b.max.x - fishHalfSize.x),
                Mathf.Clamp(pos.y, b.min.y + fishHalfSize.y, b.max.y - fishHalfSize.y),
                Mathf.Clamp(pos.z, b.min.z + fishHalfSize.z, b.max.z - fishHalfSize.z)
            );
        }

        private Vector3 GetSafeRandomPointInsideAquarium()
        {
            Bounds b = aquariumCollider.bounds;

            return new Vector3(
                Random.Range(b.min.x + fishHalfSize.x, b.max.x - fishHalfSize.x),
                Random.Range(b.min.y + fishHalfSize.y, b.max.y - fishHalfSize.y),
                Random.Range(b.min.z + fishHalfSize.z, b.max.z - fishHalfSize.z)
            );
        }

        // ---------------- Utility ----------------
        private float ClampAngle(float angle, float min, float max)
        {
            if (angle > 180f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }
    }
}

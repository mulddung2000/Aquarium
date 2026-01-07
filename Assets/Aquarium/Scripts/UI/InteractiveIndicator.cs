using UnityEngine;

namespace Aquarium
{
    public class InteractionIndicator : MonoBehaviour
    {
        [Header("Indicator Visual Root")]
        [SerializeField] private GameObject indicatorVisual;
        // SpotLight, Quad, Mesh 등 실제 보이는 오브젝트

        private void Awake()
        {
            // 시작 시 무조건 OFF
            if (indicatorVisual != null)
                indicatorVisual.SetActive(false);

            gameObject.SetActive(false);
        }

        public void Show()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            if (indicatorVisual != null && !indicatorVisual.activeSelf)
                indicatorVisual.SetActive(true);
        }

        public void Hide()
        {
            if (indicatorVisual != null && indicatorVisual.activeSelf)
                indicatorVisual.SetActive(false);

            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }
    }
}

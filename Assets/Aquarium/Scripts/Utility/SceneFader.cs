/*using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Aquarium
{
    public class SceneFader : MonoBehaviour
    {
        public static SceneFader Instance;

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 0.6f;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void FadeOut(Action onComplete = null)
        {
            StartCoroutine(FadeRoutine(1f, onComplete));
        }

        public void FadeIn(Action onComplete = null)
        {
            StartCoroutine(FadeRoutine(0f, onComplete));
        }

        private IEnumerator FadeRoutine(float targetAlpha, Action onComplete)
        {
            float startAlpha = canvasGroup.alpha;
            float time = 0f;

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
            onComplete?.Invoke();
        }
    }
}
*/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Aquarium
{
    public class SceneFader : MonoBehaviour
    {
        public static SceneFader Instance;

        [Header("Fade")]
        [SerializeField] private Image img;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float fadeDuration = 1f;

        private bool isFading;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // 🔑 핵심: 루트로 올림
            transform.SetParent(null);

            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }


        private void OnDestroy()
        {
            if (Instance == this)
                SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Start()
        {
            // 항상 검정에서 시작
            SetAlpha(1f);
        }

        /* =========================================================
         * Scene Load Flow
         * ========================================================= */

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // 씬 로드 직후 카메라부터 강제 세팅
            if (CameraManager.Instance != null)
            {
                string location = InteractionRegistry.GetCurrentLocation();
                if (!string.IsNullOrEmpty(location))
                    CameraManager.Instance.ForceSetLocation(location);
            }

            // 그 다음 FadeIn
            StartCoroutine(FadeIn());
        }

        public void FadeToScene(string sceneName)
        {
            if (isFading) return;
            StartCoroutine(FadeOutAndLoad(sceneName));
        }

        private IEnumerator FadeOutAndLoad(string sceneName)
        {
            isFading = true;
            yield return FadeOut();
            SceneManager.LoadScene(sceneName);
            isFading = false;
        }

        /* =========================================================
         * Teleport Flow
         * ========================================================= */

        public void FadeTeleport(
            Transform player,
            Transform targetPos,
            System.Action onComplete = null
        )
        {
            if (isFading) return;
            StartCoroutine(FadeTeleportRoutine(player, targetPos, onComplete));
        }

        private IEnumerator FadeTeleportRoutine(
            Transform player,
            Transform target,
            System.Action onComplete
        )
        {
            isFading = true;

            yield return FadeOut();

            // 위치 이동
            player.position = target.position;

            // 카메라 재세팅
            if (CameraManager.Instance != null)
            {
                string location = InteractionRegistry.GetCurrentLocation();
                if (!string.IsNullOrEmpty(location))
                    CameraManager.Instance.ForceSetLocation(location);
            }

            yield return FadeIn();

            isFading = false;
            onComplete?.Invoke();
        }

        /* =========================================================
         * Fade Core
         * ========================================================= */

        private IEnumerator FadeIn()
        {
            float t = fadeDuration;
            while (t > 0f)
            {
                t -= Time.deltaTime;
                SetAlpha(curve.Evaluate(t / fadeDuration));
                yield return null;
            }
            SetAlpha(0f);
        }

        private IEnumerator FadeOut()
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                SetAlpha(curve.Evaluate(t / fadeDuration));
                yield return null;
            }
            SetAlpha(1f);
        }

        private void SetAlpha(float a)
        {
            if (img == null) return;
            img.color = new Color(0f, 0f, 0f, a);
        }
    }
}

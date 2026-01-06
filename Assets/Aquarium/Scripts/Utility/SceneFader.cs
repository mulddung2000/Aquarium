using System;
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
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
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
            if (!canvasGroup)
                yield break;

            float startAlpha = canvasGroup.alpha;
            float time = 0f;

            while (time < fadeDuration)
            {
                if (!this || !canvasGroup)
                    yield break;

                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
            onComplete?.Invoke();
        }
    }
}

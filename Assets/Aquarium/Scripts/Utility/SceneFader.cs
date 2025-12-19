using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Aquarium
{
    /// <summary>
    /// 씬 페이드인, 페이드 아웃 기능
    /// 페이드 아웃 후 씬 이동
    /// </summary>
    public class SceneFader : MonoBehaviour
    {
        #region Variables
        public Image img;
        public AnimationCurve curve;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            // 페이더 이미지를 검정색으로 시작
            img.color = new Color(0f, 0f, 0f, 1f);
        }
        #endregion

        #region Custom Method

        // 페이드인 시작
        public void FadeStart(float delayTime = 0f)
        {
            StartCoroutine(FadeIn(delayTime));
        }

        // 페이드인 : 1초동안 이미지 a: 1 -> 0
        public IEnumerator FadeIn(float delayTime = 0f)
        {
            if (delayTime > 0f)
            {
                yield return new WaitForSeconds(delayTime);
            }

            float t = 1f;

            while (t > 0f)
            {
                t -= Time.deltaTime;
                float a = curve.Evaluate(t);
                img.color = new Color(0f, 0f, 0f, a);
                yield return null;
            }
        }

        // 외부 호출용 (string)
        public void FadeTo(string sceneName)
        {
            StartCoroutine(FadeOut(sceneName));
        }

        // 외부 호출용 (build index)
        public void FadeTo(int buildIndex)
        {
            StartCoroutine(FadeOut(buildIndex));
        }

        // 페이드 아웃 : 1초동안 이미지 a: 0 -> 1
        public IEnumerator FadeOut(string sceneName)
        {
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime;
                float a = curve.Evaluate(t);
                img.color = new Color(0f, 0f, 0f, a);
                yield return null;
            }

            // ✅ 페이드 아웃 완료 후
            // 씬 이름이 있을 때만 씬 이동
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        // 페이드 아웃 (build index)
        IEnumerator FadeOut(int buildIndex)
        {
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime;
                float a = curve.Evaluate(t);
                img.color = new Color(0f, 0f, 0f, a);
                yield return null;
            }

            if (buildIndex >= 0)
            {
                SceneManager.LoadScene(buildIndex);
            }
        }

        #endregion
    }
}
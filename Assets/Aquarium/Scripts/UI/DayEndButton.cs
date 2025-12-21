using UnityEngine;

namespace Aquarium
{
    public class DayEndButton : MonoBehaviour
    {
        [SerializeField] private SceneFader sceneFader;
        [SerializeField] private string nextWeekSceneName = "Week02";

        public void OnClickEndWeek()
        {
            if (sceneFader == null)
            {
                Debug.LogError("[DayEndButton] SceneFader is not assigned.");
                return;
            }

            sceneFader.FadeTo(nextWeekSceneName);
        }
    }
}
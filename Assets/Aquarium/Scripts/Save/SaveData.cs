using UnityEngine;

namespace Aquarium
{
    [System.Serializable]
    public class SaveData
    {
        public string sceneName;
        public int currentWeek;
        public Vector3 playerPosition;
        public string nextInteractionID;

        // 슬롯 UI 표시용
        public string goalText;
        public string saveDateTime;
    }
}

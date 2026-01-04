using System;
using UnityEngine;

namespace Aquarium
{
    [Serializable]
    public class SaveData
    {
        // 로드할 씬 이름
        public string sceneName;

        // 현재 주차
        public int currentWeek;

        // 플레이어 위치
        public Vector3 playerPosition;

        // 다음에 활성화될 Interaction ID
        public string nextInteractionID;

        // 현재 장소 ID
        public string locationID;

        // 현재 목표 텍스트
        public string goalText;

        // 저장 시각 (UI 표시용)
        public string saveDateTime;
    }
}

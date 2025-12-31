using System;
using UnityEngine;

namespace Aquarium
{
    [Serializable]
    public class SaveData
    {
        public int week;
        public string sceneName;

        public string interactionID;

        public Vector3 playerPosition;

        public string locationID;
        public UIState uiState;
    }
}

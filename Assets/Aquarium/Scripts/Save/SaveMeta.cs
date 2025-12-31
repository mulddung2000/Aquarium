using System;
using UnityEngine;

namespace Aquarium
{
    [Serializable]
    public class SaveMeta
    {
        public int week;
        public string sceneName;

        public string locationID;
        public string nextGoalText;

        public string savedTime;
    }
}

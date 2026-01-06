using System;

namespace Aquarium
{
    public static class LocationEventHub
    {
        // LocationID가 변경되었을 때 발생하는 이벤트
        // 예: "W01_Room"
        public static event Action<string> OnLocationChanged;

        // Location 변경 알림
        public static void SetLocation(string locationID)
        {
            if (string.IsNullOrEmpty(locationID))
                return;

            OnLocationChanged?.Invoke(locationID);
        }
    }
}

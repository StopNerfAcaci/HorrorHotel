using UnityEngine;

namespace GlobalSettings
{
    [CreateAssetMenu(fileName = "Gameplay", menuName = "GlobalSettings/Gameplay")]
    public class Gameplay : ScriptableObject
    {
        [SerializeField] private DayPhase[] days;
        [SerializeField] private bool hasKey = false;
        public DayPhase[] Days => days;
        private const string DayPrefix = "DayKey";

        public void SaveDay(DayPhase dp)
        {
            int encoded = dp.day * 2 + (dp.isDaytime ? 0 : 1);
            PlayerPrefs.SetInt(DayPrefix, encoded);
            PlayerPrefs.Save();
        }

        public bool TryLoadDay(out DayPhase result)
        {
            if (!PlayerPrefs.HasKey(DayPrefix))
            {
                result = days[0];
                return false;
            }

            int encoded = PlayerPrefs.GetInt(DayPrefix);
            int day = encoded / 2;
            bool isDaytime = encoded % 2 == 0;

            foreach (var d in days)
            {
                if (d.day == day && d.isDaytime == isDaytime)
                {
                    result = d;
                    return true;
                }
            }

            result = days[0];
            return false;
        }
    }
}
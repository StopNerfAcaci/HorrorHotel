using Gameplay.Inventory;
using UnityEngine;

namespace GlobalSettings
{
    [CreateAssetMenu(fileName = "Gameplay", menuName = "GlobalSettings/Gameplay")]
    public class Gameplay : GlobalSettingsBase<Gameplay>
    {
        [SerializeField] private DayPhase[] days;
        [SerializeField] private bool hasKey = false;
        public static DayPhase[] Days => Get().days;
        public static bool HasKey => Get().hasKey;

        public static Gameplay Get()
        {
            return GlobalSettingsBase<Gameplay>.Get("Gameplay");
        }

        [RuntimeInitializeOnLoadMethod]
        public static void PreWarm()
        {
            GlobalSettingsBase<Gameplay>.StartPreloadAddressable("Gameplay");
        }

        public static void UnLoad()
        {
            GlobalSettingsBase<Gameplay>.StartUnload();
        }
        
    }
}
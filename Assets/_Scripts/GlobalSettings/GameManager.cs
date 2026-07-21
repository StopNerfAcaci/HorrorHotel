using UnityEngine;

namespace GlobalSettings
{
    [CreateAssetMenu(fileName = "GameManager", menuName = "GlobalSettings/GameManager")]
    public sealed class GameManager : GlobalSettingsBase<GameManager>
    {
        public void QuitLevel()
        {
            //TODO: quit logic
        }

        public void PauseGame(bool isPause)
        {
            //TODO: pause logic
        }

        public static GameManager Get()
        {
            return GlobalSettingsBase<GameManager>.Get("GameManager");
        }

        [RuntimeInitializeOnLoadMethod]
        public static void PreWarm()
        {
            GlobalSettingsBase<GameManager>.StartPreloadAddressable("GameManager");
        }

        public static void UnLoad()
        {
            GlobalSettingsBase<GameManager>.StartUnload();
        }
    }
}
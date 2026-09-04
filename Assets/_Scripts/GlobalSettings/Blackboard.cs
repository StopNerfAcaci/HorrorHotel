using GlobalSettings;
using UnityEngine;

[CreateAssetMenu(fileName = "Blackboard", menuName = "GlobalSettings/Blackboard")]
public class Blackboard : GlobalSettingsBase<Blackboard>
{
    private static Blackboard Get()
    {
        return GlobalSettingsBase<Blackboard>.Get("Blackboard");
    }

    [RuntimeInitializeOnLoadMethod]
    public static void PreWarm()
    {
        GlobalSettingsBase<Blackboard>.StartPreloadAddressable("Blackboard");
    }

    public static void UnLoad()
    {
        GlobalSettingsBase<Blackboard>.StartUnload();
    }

}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Dialogue", menuName = "Dialogue/Dialogue")]
public class DialogueConfig : ScriptableObject
{
    [SerializeField] LineConfig[] _configs;

    public IReadOnlyList<LineConfig> Configs => _configs;

    public DialogueData GetDialogue(int id)
    {
        if (_configs == null || _configs.Length == 0) return null;

        foreach (var config in _configs)
        {
            if (!config.TryGetDialogue(id, out var data)) continue;
            return data;
        }

        return null;
    }
}
using System;
using Cysharp.Threading.Tasks;
using MVP;
using TMPro;
using UnityEngine;

namespace GameCore.Presentation
{
    public class PreviewModal : Modal<PreviewViewState>
    {
        [SerializeField] TextMeshProUGUI _title;
        [SerializeField] TextMeshProUGUI _description;
        public override UniTask InitializeState(PreviewViewState state, Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        public void UpdateText(string itemName, string itemDescription)
        {
            _title.text = itemName;
            _description.text = itemDescription;
        }
    }
}
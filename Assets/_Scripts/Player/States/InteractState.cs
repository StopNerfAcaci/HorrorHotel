using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Inventory;
using UnityEngine;
using Utils.Extensions;

namespace HSM
{
    public class InteractState : State
    {
        private readonly PlayerStateDriver player;

        // private readonly GameplayManager gameplayManager;

        private bool isAbilityDone;
        private float _yaw;
        private float _pitch;

        private IItem _heldItem;
        private Vector2 prevPos;

        public InteractState(StateMachine machine, State parent, PlayerStateDriver player) : base(machine, parent)
        {
            this.player = player;
        }


        protected override void OnEnter()
        {
            player.Reader.Interact += OnInteract;
            player.Reader.Pointed += GetLastPoint;
            player.SetBusy(true);
            var root = (PlayerRoot)Parent.Parent;
            _heldItem = root.PendingInteractable as IItem;
            root.PendingInteractable = null;
            _yaw = 0f;
            _pitch = 0f;
            // ShowPreviewAsync().Forget();
        }

        // private async UniTask ShowPreviewAsync()
        // {
        //     var transition = ((PlayerRoot)Parent.Parent).TransitionService;
            // var preview = await transition.ShowPreviewModal();
            // preview.ItemPreviewCommand.Execute(_heldItem.itemData);
        // }

        protected override void OnExit()
        {
            player.Reader.Interact -= OnInteract;
            player.Reader.Pointed -= GetLastPoint;
            player.SetBusy(false);
        }

        private void GetLastPoint(Vector2 pos)
        {
            prevPos = pos;
        }

        private void OnInteract()
        {
            InteractAsync().Forget();
        }

        protected override void OnUpdate(float deltaTime)
        {
            HandleHold();
        }

        private void HandleHold()
        {
            if (!player.Reader.IsHolding) return;
            _yaw += prevPos.x * player.Data.RotationSpeed * Time.deltaTime;
            _pitch += -prevPos.y * player.Data.RotationSpeed * Time.deltaTime;
            _heldItem.Transform.localRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }

        private async UniTask InteractAsync()
        {
            // gameplayManager.UpdateDayPhase(_heldItem.itemData);
           await _heldItem.Use();
            _heldItem = null;
            isAbilityDone = true;
        }

        protected override State GetTransition() => isAbilityDone ? ((PlayerRoot)Parent.Parent).Locomotion : null;
    }
}
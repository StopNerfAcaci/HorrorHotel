using GameCore.Presentation.Shared;
using Gameplay.CoreSystem;
using Gameplay.Inventory;
using R3;
using UnityEngine;
using Utils.Helpers;

namespace HSM
{
    public class PlayerRoot : State
    {
        public readonly PlayerStateDriver player;
        public readonly Locomotion Locomotion;
        public readonly AbilityState AbilityState;
        public readonly TransitionService TransitionService;

        public PlayerRoot(StateMachine machine, PlayerStateDriver player) : base(machine, null)
        {
            this.player = player;
            core = player.Core;
            var data = player.Data;
            TransitionService = new TransitionService();
            Locomotion = new Locomotion(machine, this, player, data);
            AbilityState = new AbilityState(machine, this, player);
        }
        
        
        public IInteractable PendingInteractable { get; set; }

        protected override State GetInitialState() => Locomotion;
        
    }
}
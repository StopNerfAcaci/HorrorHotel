using GameCore.Presentation.Shared;
using Gameplay.CoreSystem;
using R3;
using UnityEngine;
using Utils.Helpers;

namespace HSM
{
    public class PlayerRoot : State
    {
        public readonly Locomotion Locomotion;
        public readonly AbilityState AbilityState;
        public readonly TransitionService TransitionService;

        public PlayerRoot(StateMachine machine, PlayerStateDriver player) : base(machine, null)
        {
            core = player.Core;
            var data = player.Data;
            TransitionService = new TransitionService();
            Locomotion = new Locomotion(machine, this, player, data);
            AbilityState = new AbilityState(machine, this, player);
        }

        public WorldItem PendingItem { get; set; }

        protected override State GetInitialState() => Locomotion;
        
    }
}
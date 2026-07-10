using Gameplay.CoreSystem;
using R3;
using UnityEngine;
using Utils.Helpers;

namespace HSM
{
    public class PlayerRoot : State
    {
        private readonly PlayerStateDriver player;
        public readonly Locomotion Locomotion;
        public readonly AbilityState AbilityState;


        public PlayerRoot(StateMachine machine, PlayerStateDriver player) : base(machine, null)
        {
            this.player = player;
            core = player.Core;
            var data = player.Data;
            Locomotion = new Locomotion(machine, this, player, data);
            AbilityState = new AbilityState(machine, this, player);
        }

        protected override State GetInitialState() => Locomotion;
        
    }
}
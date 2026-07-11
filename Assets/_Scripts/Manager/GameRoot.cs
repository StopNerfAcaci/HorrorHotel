using Managers;
using UnityEngine;

namespace HSM
{
    public class GameRoot : State
    {
        private readonly GameManager manager;
        public readonly InGameState ingameState;

        public GameRoot(StateMachine machine, GameManager manager) : base(machine, null)
        {
            this.manager = manager;
        }
    }
}
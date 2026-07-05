using System.Collections.Generic;

namespace HSM
{
    public class StateMachine
    {
        public readonly State Root;
        public readonly TransitionSequencer Sequencer;
        bool started;
        
        public StateMachine(State root)
        {
            Root = root;
            Sequencer = new TransitionSequencer(this);
        }

        public void Start()
        {
            if(started) return;
            
            started = true;
            Root.Enter();
        }

        public void Tick(float deltaTime)
        {
            if(!started) return;
            Sequencer.Tick(deltaTime);
        }

        public void FixedTick(float deltaTime) => Root.FixedUpdate(deltaTime);
        internal void InternalTick(float deltaTime) => Root.Update(deltaTime);
        public void ChangeState(State from, State to)
        {
            if (from == to || from == null || to == null) return;
            
            State lca =  TransitionSequencer.LCA(from, to);
            
            for(var s = from; s != lca; s= s.Parent) s.Exit();
            
            var stack = new Stack<State>();
            for(var s = to; s != lca; s= s.Parent) stack.Push(s);
            while(stack.Count > 0) stack.Pop().Enter();
        }
    }
    
}
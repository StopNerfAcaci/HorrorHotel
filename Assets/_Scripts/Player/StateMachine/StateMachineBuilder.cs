using System.Collections.Generic;
using System.Reflection;

namespace HSM
{
    public class StateMachineBuilder
    {
        private readonly State Root;
        
        public StateMachineBuilder(State Root)
        {
            this.Root = Root;
        }

        public StateMachine Build()
        {
            var m =  new StateMachine(Root);
            Wire(Root, m, new HashSet<State>());
            return m;
        }

        void Wire(State s, StateMachine m, HashSet<State> visited)
        {
            if (s == null) return;
            if(!visited.Add(s)) return; //State already wired
            
            var flags = BindingFlags.Instance | BindingFlags.NonPublic |  BindingFlags.Public |BindingFlags.FlattenHierarchy;
            var machineField = typeof(State).GetField("Machine", flags);
            if(machineField != null) machineField.SetValue(s, m);

            foreach (var field in s.GetType().GetFields(flags))
            {
                if(!typeof(State).IsAssignableFrom(field.FieldType)) continue; //Only consider state field
                if(field.Name == "Parent") continue; //Skip back-edge to parent
                
                var child = (State)field.GetValue(s);
                if(child == null) continue;
                if(!ReferenceEquals(child.Parent, s)) continue; //Ensure this is our direct child
                
                Wire(child, m, visited); //Recurse into the child
            }
        }
    }
}
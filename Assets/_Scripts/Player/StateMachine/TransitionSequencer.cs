using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace HSM
{
    public class TransitionSequencer
    {
        public readonly StateMachine Machine;

        ISequence sequencer; //current phase
        private Action nextPhase; //switch between phase
        private (State from, State to)? pending; //coalesce a single pending request
        CancellationTokenSource cts = new CancellationTokenSource();
        public readonly bool UseSequential = true;

        public TransitionSequencer(StateMachine machine)
        {
            Machine = machine;
        }

        //States to exit from => to but exclude lca
        static List<State> StatesToExit(State from, State lca)
        {
            var list = new List<State>();
            for (var s = from; s != null && s != lca; s = s.Parent) list.Add(s);
            return list;
        }

        static List<PhaseStep> GatherPhaseSteps(List<State> chains, bool deactivate)
        {
            var steps = new List<PhaseStep>();

            if (chains.Count == 0) return steps;
            for (int i = 0; i < chains.Count; i++)
            {
                var acts = chains[i].Activities;
                if(acts == null || acts.Count == 0) continue;
                for (int j = 0; j < acts.Count; j++)
                {
                    var act = acts[j];
                    if (deactivate)
                    {
                        if (act.Mode == ActivityMode.Active) steps.Add(ct => act.DeactivateAsync(ct));
                    }
                    else
                    {
                        if (act.Mode == ActivityMode.Inactive) steps.Add(ct => act.ActivateAsync(ct));
                    }
                }
            }

            return steps;
        }

        //State to enter from => to but exclude lca
        static List<State> StatesToEnter(State from, State lca)
        {
            var stacks = new Stack<State>();
            for (var s = from; s != null && s != lca; s = s.Parent) stacks.Push(s);
            return new List<State>(stacks);
        }

        public void RequestTransition(State from, State to)
        {
            if (to == null || from == to) return;
            if (sequencer != null)
            {
                pending = (from, to);
                return;
            }

            BeginTransition(from, to);
        }

        void BeginTransition(State from, State to)
        {
            var lca = LCA(from, to);
            var exitChain = StatesToExit(from, lca);
            var enterChain = StatesToEnter(to, lca);
            //Deactivate old branch
            var exitSteps = GatherPhaseSteps(exitChain, deactivate: true);
            sequencer = UseSequential
                ? new SequentialPhase(exitSteps, cts.Token)
                : new ParallelPhase(exitSteps, cts.Token);
            sequencer.Start();

            nextPhase = () =>
            {
                //Change state
                Machine.ChangeState(from, to);
                //Activate new branch
                var enterSteps = GatherPhaseSteps(enterChain, deactivate: false);
                sequencer = UseSequential
                    ? new SequentialPhase(enterSteps, cts.Token)
                    : new ParallelPhase(enterSteps, cts.Token);
                // sequencer = new NoopPhase();
                sequencer.Start();
            };
        }

        void EndTransition()
        {
            sequencer = null;
            if (pending.HasValue)
            {
                var p = pending.Value;
                pending = null;
                BeginTransition(p.from, p.to);
            }
        }

        public void Tick(float deltaTime)
        {
            if (sequencer != null)
            {
                if (sequencer.Update())
                {
                    if (nextPhase != null)
                    {
                        var n = nextPhase;
                        nextPhase = null;
                        n();
                    }
                    else
                    {
                        EndTransition();
                    }
                }

                return;
            }

            Machine.InternalTick(deltaTime);
        }

        //Compute lowest common ancestor of 2 states
        public static State LCA(State a, State b)
        {
            var ap = new HashSet<State>();
            for (State s = a; s != null; s = s.Parent) ap.Add(s);

            for (State s = b; s != null; s = s.Parent)
            {
                if (ap.Contains(s)) return s;
            }

            return null;
        }
    }
}

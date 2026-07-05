using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HSM
{
    public interface ISequence
    {
        bool IsDone { get; }
        void Start();
        bool Update();
    }

    //One activity operation to run in this phase
    public delegate Task PhaseStep(CancellationToken ct);

    public class NoopPhase : ISequence
    {
        public bool IsDone { get; private set; }
        public void Start() => IsDone = true;
        public bool Update() => IsDone;
    }

    public class SequentialPhase : ISequence
    {
        readonly List<PhaseStep> steps;
        readonly CancellationToken ct;
        private int index = -1;
        Task currentTask;

        public bool IsDone { get; private set; }

        public SequentialPhase(List<PhaseStep> phaseSteps, CancellationToken ct)
        {
            steps = phaseSteps;
            this.ct = ct;
        }

        public void Start() => Next();

        private void Next()
        {
            index++;
            if (steps == null || index >= steps.Count)
            {
                IsDone = true;
                return;
            }

            currentTask = steps[index](ct);
        }

        public bool Update()
        {
            if (IsDone) return true;
            if (currentTask == null || currentTask.IsCompleted) Next();

            return IsDone;
        }
    }

    public class ParallelPhase : ISequence
    {
        readonly List<PhaseStep> steps;
        readonly CancellationToken ct;
        List<Task> tasks;
        public bool IsDone { get; private set; }

        public ParallelPhase(List<PhaseStep> steps, CancellationToken ct)
        {
            this.steps = steps;
            this.ct = ct;
        }

        public void Start()
        {
            if (steps == null || steps.Count == 0)
            {
                IsDone = true;
                return;
            }

            tasks = new List<Task>(steps.Count);
            foreach (var step in steps)
            {
                tasks.Add(step(ct));
            }


        }
        public bool Update()
        {
            if (IsDone) return true;

            IsDone = tasks == null || tasks.TrueForAll(t => t.IsCompleted);
            return IsDone;
        }
    }
}

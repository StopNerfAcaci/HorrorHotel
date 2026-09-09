namespace Horror.Events
{
    public abstract class EventData
    {
        public float Cooldown { get; private set; }

        public EventData(float cooldown = 0)
        {
            Cooldown = cooldown;
        }
    }
    
    public class HideMenuEventData : EventData
    {
        public HideMenuEventData() : base(0)
        {
        }
    }

    public class DialogueEventData : EventData
    {
        public NPC NPC { get; private set; }

        public DialogueEventData(NPC npc): base(0)
        {
            NPC = npc;
        }
    }
}
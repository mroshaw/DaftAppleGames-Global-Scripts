using UnityEngine;

namespace Invector.vCharacterController.AI.FSMBehaviour
{
#if UNITY_EDITOR
    [vFSMHelpbox("Decide whether it's time for the NPC to go to sleep or wake up", UnityEditor.MessageType.Info)]
#endif
    public class SleepDecision : vStateDecision
    {
		public override string categoryName
        {
            get { return "NPC/"; }
        }

        public override string defaultName
        {
            get { return "Time to sleep?"; }
        }

        /// <summary>
        /// Make the decision
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <returns></returns>
        public override bool Decide(vIFSMBehaviourController fsmBehaviour)
        {
            int sleepHour = fsmBehaviour.aiController.sleepHour;
            int wakeHour = fsmBehaviour.aiController.wakeHour;
#if ENVIRO_3
            int enviroHour = Enviro.EnviroManager.instance.Time.hours;
            return (enviroHour >= sleepHour || enviroHour <= wakeHour);
#else
            return false;
#endif
        }
    }
}

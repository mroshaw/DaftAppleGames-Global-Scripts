using UnityEngine;

namespace Invector.vCharacterController.AI.FSMBehaviour
{
#if UNITY_EDITOR
    [vFSMHelpbox("Decide whether it's time for the NPC to go home", UnityEditor.MessageType.Info)]
#endif
    public class GoHomeDecision : vStateDecision
    {
		public override string categoryName
        {
            get { return "NPC/"; }
        }

        public override string defaultName
        {
            get { return "Time to go home"; }
        }

        /// <summary>
        /// Make the decision
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <returns></returns>
        public override bool Decide(vIFSMBehaviourController fsmBehaviour)
        {
#if ENVIRO_3
            if (!fsmBehaviour.aiController.home || !Enviro.EnviroManager.instance)
            {
                return false;
            }

            int startHour = fsmBehaviour.aiController.workStartHour;
            int endHour = fsmBehaviour.aiController.workEndHour;
            int enviroHour = Enviro.EnviroManager.instance.Time.hours;
            return (enviroHour < startHour || enviroHour >= endHour);
#else
            return false;
#endif
        }
    }
}

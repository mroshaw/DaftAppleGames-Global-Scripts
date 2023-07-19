using UnityEngine;

namespace Invector.vCharacterController.AI.FSMBehaviour
{
#if UNITY_EDITOR
    [vFSMHelpbox("Decide whether it's time for the NPC to go to work", UnityEditor.MessageType.Info)]
#endif
    public class GoToWorkDecision : vStateDecision
    {
		public override string categoryName
        {
            get { return "NPC/"; }
        }

        public override string defaultName
        {
            get { return "Time to go to work"; }
        }
        /// <summary>
        /// Make the decision
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <returns></returns>

        public override bool Decide(vIFSMBehaviourController fsmBehaviour)
        {
#if ENVIRO_3
            if (!fsmBehaviour.aiController.workplace || !Enviro.EnviroManager.instance)
            {
                return false;
            }

            int startHour = fsmBehaviour.aiController.workStartHour;
            int endHour = fsmBehaviour.aiController.workEndHour;
            int enviroHour = Enviro.EnviroManager.instance.Time.hours;
            return (enviroHour >= startHour && enviroHour <= endHour);
#else
            return false;
#endif
        }
    }
}

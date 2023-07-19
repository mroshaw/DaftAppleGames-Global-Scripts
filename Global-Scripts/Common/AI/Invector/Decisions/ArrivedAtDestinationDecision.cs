using UnityEngine;

namespace Invector.vCharacterController.AI.FSMBehaviour
{
#if UNITY_EDITOR
    [vFSMHelpbox("Decide whether NPC has arrived at their destination", UnityEditor.MessageType.Info)]
#endif
    public class ArrivedAtDestinationDecision : vStateDecision
    {
		public override string categoryName
        {
            get { return "NPC/"; }
        }

        public override string defaultName
        {
            get { return "Arrived at destination"; }
        }

        /// <summary>
        /// Make the decision
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <returns></returns>
        public override bool Decide(vIFSMBehaviourController fsmBehaviour)
        {
            // This custom decision that will verify the bool 'moveToTarget' and return if it's true or false
            return fsmBehaviour.aiController.isInDestination;
        }
    }
}

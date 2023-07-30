﻿#if INVECTOR_AI_TEMPLATE
using Invector.vCharacterController.AI.FSMBehaviour;

namespace DaftAppleGames.Common.AI.Invector.Decisions
{
#if UNITY_EDITOR
    [vFSMHelpbox("Decide whether NPC has arrived at their destination", UnityEditor.MessageType.Info)]
#endif
    public class ArrivedAtDestinationDecision : vStateDecision
    {
        /// <summary>
        /// Return the Decision Category
        /// </summary>
		public override string categoryName => "NPC/";

        /// <summary>
        /// Return the Decision name
        /// </summary>
        public override string defaultName => "Arrived at destination";

        /// <summary>
        /// Determine whether NPC has arrived at their destination
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
#endif
﻿using Invector.vCharacterController.AI;
using Invector.vCharacterController.AI.FSMBehaviour;

namespace DaftAppleGames.Common.AI.Invector.Actions
{
#if UNITY_EDITOR
    [vFSMHelpbox("This action causes the NPC to walk towards their designated workplace", UnityEditor.MessageType.Info)]
#endif
    
    // StateAction class to tell an FSM AI to go to work
    public class GoToWorkAction : vStateAction
    {
        /// <summary>
        /// Return the Action Category
        /// </summary>
        public override string categoryName => "NPC/";
        
        /// <summary>
        /// Return the Action name
        /// </summary>
        public override string defaultName => "Go to work";

        // set a custom speed to the controller
        public vAIMovementSpeed speed = vAIMovementSpeed.Walking;

        /// <summary>
        /// Tell the FSM AI to go to work
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <param name="fsmExecutionType"></param>
        public override void DoAction(vIFSMBehaviourController fsmBehaviour, vFSMComponentExecutionType fsmExecutionType = vFSMComponentExecutionType.OnStateUpdate)
        {
            fsmBehaviour.aiController.GoToWork(speed);
        }
    }
}
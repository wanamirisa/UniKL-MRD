using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UniKL
{
    [Serializable]
    public class UnityEventActionPayload : UnityEvent<ActionPayload> { }

    [Serializable]
    public class ReactionEvent
    {
        public string ActionType = "Action";
        public UnityEventActionPayload OnActionEntered;
        public UnityEventActionPayload OnActionStayed;
        public UnityEventActionPayload OnActionExited;
    }

    public class ReactionReceiver : MonoBehaviour, IReaction
    {
        [SerializeField] private List<ReactionEvent> reactionList = new();

        public void ReceiveAction(ActionPayload payload)
        {
            foreach (var reaction in reactionList)
            {
                if (reaction.ActionType == payload.ActionType)
                {
                    switch (payload.ActionState)
                    {
                        case ActionState.Enter:
                            reaction.OnActionEntered?.Invoke(payload);
                            break;
                        case ActionState.Stay:
                            reaction.OnActionStayed?.Invoke(payload);
                            break;
                        case ActionState.Exit:
                            reaction.OnActionExited?.Invoke(payload);
                            break;
                    }

                    // print($"{gameObject.name} RECEIVED: {reaction.ActionType}");
                }
            }
        }

        void OnValidate()
        {
            if (gameObject.TryGetComponent(out Collider collider))
            {
                collider.isTrigger = true;
            }
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.Events;

namespace UniKL
{
    public enum ActionState { Enter, Stay, Exit }

    public class ActionPayload
    {
        public string ActionType = "Action";
        public GameObject Source = null;
        public ActionState ActionState;
    }

    [RequireComponent(typeof(Rigidbody))]
    public class ActionEmitter : MonoBehaviour, IAction
    {
        [SerializeField] private string actionType = "Electric";

        void OnTriggerEnter(Collider other)
        {
            SendAction(other.gameObject, ActionState.Enter);
        }

        void OnTriggerStay(Collider other)
        {
            SendAction(other.gameObject, ActionState.Stay);
        }

        void OnTriggerExit(Collider other)
        {
            SendAction(other.gameObject, ActionState.Exit);
        }

        public void SendAction(GameObject target, ActionState actionState)
        {
            if (target.TryGetComponent(out IReaction reaction))
            {
                ActionPayload payload = new()
                {
                    ActionType = actionType,
                    Source = gameObject,
                    ActionState = actionState,
                };

                reaction.ReceiveAction(payload);
                // print($"{gameObject.name} SEND: {actionType}");
            }
        }

        void OnValidate()
        {
            if (gameObject.TryGetComponent(out Rigidbody rb))
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            if (gameObject.TryGetComponent(out Collider collider))
            {
                collider.isTrigger = true;
            }
        }
    }
}

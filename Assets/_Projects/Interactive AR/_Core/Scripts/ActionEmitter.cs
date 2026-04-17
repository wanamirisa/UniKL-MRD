using System;
using UnityEngine;

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
        [SerializeField] private Collider actionCollider;

        [SerializeField] private string actionType = "Electric";

        void Awake()
        {
            if (actionCollider == null)
            {
                throw new Exception($"ActionEmitter {gameObject.name} has no Collider assigned!");
            }
        }

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

                print($"{gameObject.name} SEND: {actionType}");
            }
        }

        #region EDITOR ONLY
        void OnValidate()
        {
            if (gameObject.TryGetComponent(out Rigidbody rb))
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            if (gameObject.TryGetComponent(out actionCollider))
            {
                actionCollider.isTrigger = true;
            }
        }

        void OnDrawGizmos()
        {
            if (actionCollider != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.matrix = transform.localToWorldMatrix;

                switch (actionCollider)
                {
                    case BoxCollider box:
                        Gizmos.DrawWireCube(box.center, box.size);
                        break;
                    case SphereCollider sphere:
                        Gizmos.DrawWireSphere(sphere.center, sphere.radius);
                        break;
                }
            }
        }
        #endregion
    }
}

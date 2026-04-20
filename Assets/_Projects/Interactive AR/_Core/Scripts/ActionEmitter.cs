using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniKL
{
    public enum ActionState { Enter, Stay, Exit }

    [Serializable]
    public class ActionPayload
    {
        public string ActionType = "Action";
        public GameObject Source = null;
        [HideInInspector] public ActionState ActionState;
    }

    [RequireComponent(typeof(Rigidbody))]
    public class ActionEmitter : MonoBehaviour, IAction
    {
        [SerializeField] private Collider actionCollider;

        [SerializeField] private string actionType = "Electric";
        [SerializeField] private GameObject sourceObject = null;

        // Keep track of everything we are currently touching
        private HashSet<GameObject> activeTargets = new HashSet<GameObject>();

        void Awake()
        {
            if (actionCollider == null)
            {
                throw new Exception($"ActionEmitter {gameObject.name} has no Collider assigned!");
            }
        }

        void Start()
        {
            activeTargets ??= new();

            if (sourceObject == null)
            {
                sourceObject = gameObject;
            }
        }

        void OnDisable()
        {
            // When disabled, send Exit to everything we were touching
            foreach (var target in activeTargets)
            {
                if (target != null)
                {
                    SendAction(target, ActionState.Exit);
                }
            }

            activeTargets.Clear();
        }

        void OnTriggerEnter(Collider other)
        {
            if (IsSelf(other)) return;

            activeTargets.Add(other.gameObject);
            SendAction(other.gameObject, ActionState.Enter);
        }

        void OnTriggerStay(Collider other)
        {
            if (IsSelf(other)) return;

            SendAction(other.gameObject, ActionState.Stay);
        }

        void OnTriggerExit(Collider other)
        {
            if (IsSelf(other)) return;

            activeTargets.Remove(other.gameObject);
            SendAction(other.gameObject, ActionState.Exit);
        }

        private bool IsSelf(Collider other)
        {
            // 1. If it's literally this exact same GameObject
            if (other.gameObject == gameObject) return true;

            // 2. If you assigned the root Bulb to 'sourceObject', ignore all of its children (like the Reaction object)
            if (sourceObject != null && other.transform.IsChildOf(sourceObject.transform)) return true;

            // 3. If they share the exact same parent (e.g., they are siblings under the Bulb)
            if (transform.parent != null && other.transform.parent == transform.parent) return true;

            // 4. If one is a direct child/parent of the other
            if (other.transform.IsChildOf(transform) || transform.IsChildOf(other.transform)) return true;

            return false; // It is safe to interact with!
        }

        public void SendAction(GameObject target, ActionState actionState)
        {
            if (target.TryGetComponent(out IReaction reaction))
            {
                // Create the payload
                ActionPayload payload = new()
                {
                    ActionType = actionType,
                    Source = sourceObject,
                    ActionState = actionState,
                };

                // Send the payload to the target
                reaction.ReceiveAction(payload);

                if (showDebugLogs)
                    Debug.Log($"[{actionState}] {payload.Source.name} SEND: {payload.ActionType}", payload.Source);
            }
        }

        #region EDITOR ONLY
        private bool showDebugLogs = false;
        private bool showGizmos = true;

        Color gizmosColor = new Color(1f, 0.5f, 0f);

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
            if (showGizmos == false) return;

            if (actionCollider != null)
            {
                Gizmos.color = gizmosColor;
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

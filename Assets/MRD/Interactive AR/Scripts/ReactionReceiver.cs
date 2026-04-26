using System;
using UnityEngine;
using UnityEngine.Events;

namespace UniKL
{
    [Serializable]
    public class UnityEventActionPayload : UnityEvent<ActionPayload> { }

    [Serializable]
    public class ReactionEvent
    {
        public string ActionType = "None";

        [Space(10)]

        [Tooltip("Triggered when the ActionEmitter Enter\nTrigger one time only")]
        public UnityEventActionPayload OnActionEntered;
        [Tooltip("Triggered when the ActionEmitter Stay\nTrigger every frame")]
        public UnityEventActionPayload OnActionStayed;
        [Tooltip("Triggered when the ActionEmitter Exit\nTrigger one time only")]
        public UnityEventActionPayload OnActionExited;
    }

    public class ReactionReceiver : MonoBehaviour, IReaction
    {
        [SerializeField] private Collider reactionCollider;

        [SerializeField] private ReactionEvent reactionEvent = new();

        void Awake()
        {
            if (reactionCollider == null)
            {
                throw new Exception($"ReactionReceiver {gameObject.name} has no Collider assigned!");
            }
        }

        public void ReceiveAction(ActionPayload payload)
        {
            if (reactionEvent.ActionType == payload.ActionType)
            {
                switch (payload.ActionState)
                {
                    case ActionState.Enter:
                        reactionEvent.OnActionEntered?.Invoke(payload);
                        break;
                    case ActionState.Stay:
                        reactionEvent.OnActionStayed?.Invoke(payload);
                        break;
                    case ActionState.Exit:
                        reactionEvent.OnActionExited?.Invoke(payload);
                        break;
                }

                if (showDebugLogs)
                    Debug.Log($"[{payload.ActionState}] {gameObject.name} RECEIVED: {reactionEvent.ActionType}", gameObject);
            }
        }

        #region EDITOR ONLY
        private bool showDebugLogs = false;
        private bool showGizmos = true;

        private Color gizmosColor = new Color(0.5f, 0.5f, 1f);

        void OnValidate()
        {
            if (gameObject.TryGetComponent(out reactionCollider))
            {
                reactionCollider.isTrigger = true;
            }
        }

        void OnDrawGizmos()
        {
            if (showGizmos == false) return;

            if (reactionCollider != null)
            {
                Gizmos.color = gizmosColor;
                Gizmos.matrix = transform.localToWorldMatrix;

                switch (reactionCollider)
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

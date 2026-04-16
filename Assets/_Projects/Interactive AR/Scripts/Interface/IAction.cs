using UnityEngine;

namespace UniKL
{
    public interface IAction
    {
        public void SendAction(GameObject target, ActionState actionState);
    }
}

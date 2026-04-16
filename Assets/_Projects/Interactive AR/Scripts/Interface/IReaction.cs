using System;
using UnityEngine;

namespace UniKL
{
    public interface IReaction
    {
        public void ReceiveAction(ActionPayload payload);
    }
}

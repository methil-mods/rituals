using System;

namespace Core.Dialog.Action
{
    [Serializable]
    public abstract class DialogAction
    {
        public abstract void Execute();
    }
}
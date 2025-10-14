using System;
using System.Collections.Generic;

namespace ScriptableObjects.Quest.Action
{
	[Serializable]
    public abstract class QuestAction
    {
        public abstract void Execute();
    }
}
using System;

namespace Player
{
    [Serializable]
    public class PlayerData
    {
        public int craziness = 0;
        public int maximumCraziness = 100;
        
        public float moveSpeed = 3f;
    }
}
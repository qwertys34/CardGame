using UnityEngine;

namespace Game.scripts
{
    public class Utils
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void ResetAllStatic()
        {
            if (CardManager.AllCards != null)
                CardManager.AllCards.Clear();   
        }
    }
}
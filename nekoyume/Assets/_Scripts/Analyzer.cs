using UnityEngine;

namespace Nekoyume
{
    public class Analyzer
    {
        public static Analyzer Instance => Game.Game.instance.Analyzer;

        public Analyzer()
        {
            
        }

        public Analyzer Initialize(string uniqueId = "non-unique-id")
        {
#if UNITY_EDITOR
            Debug.Log("Analyzer does not track in editor mode");
#else
#endif
            Debug.Log($"Analyzer initialized: {uniqueId}");
            return this;
        }

        public void Track(string eventName, params (string key, string value)[] properties)
        {
            if (properties.Length == 0)
            {
                return;
            }

        }

        public void Track(string eventName, object value)
        {
            
        }

        public void Flush()
        {
            
        }
    }
}

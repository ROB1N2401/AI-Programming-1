using UnityEngine;

namespace Support
{
    //Written by my friend and collaborator, imported here from his permission
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError($"There is more than one singleton of type {typeof(T)} in the scene");

                Destroy(this);
                return;
            }

            Instance = this as T;
        }
    }
}
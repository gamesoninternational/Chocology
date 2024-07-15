using UnityEngine;

    public class Singleton<Instance> : MonoBehaviour where Instance : Singleton<Instance>
    {
        public static Instance instance;
        [Header("Singleton Instance"), SerializeField, Tooltip("Dont destroy on load?")] private bool isPersistant;

        public virtual void Awake()
        {
            if (isPersistant)
            {
                if (!instance)
                {
                    instance = this as Instance;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                instance = this as Instance;
            }
        }
    }


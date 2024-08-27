using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fantasy
{
    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        public static T Instance => instance;
        protected static T instance;
        protected virtual void Awake()
        {
            //mono脚本不会有线程安全问题
                if (instance != null)
                {
                    GameObject.Destroy(this);
                    return;
                }
                instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }
    }

}


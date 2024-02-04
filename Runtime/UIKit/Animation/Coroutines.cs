using System.Collections;
using UnityEngine;

namespace CastlesTrip.UIKit
{
    public sealed class Coroutines : MonoBehaviour
    {
        private static Coroutines _instance;
        private static Coroutines instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("[COROUTINE MANAGER]");
                    _instance = go.AddComponent<Coroutines>();
                    DontDestroyOnLoad(go);
                }

                return _instance;
            }
        }

        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            return instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine routine)
        {
            instance.StopCoroutine(routine);
        }

        public static void StopRoutine(IEnumerator routine)
        {
            instance.StopCoroutine(routine);
        }
    }
}
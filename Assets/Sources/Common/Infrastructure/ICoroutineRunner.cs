using System.Collections;
using UnityEngine;

namespace Sources.Common.Infrastructure
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator enumerator);
        void StopCoroutine(Coroutine patrolCoroutine);
    }
}
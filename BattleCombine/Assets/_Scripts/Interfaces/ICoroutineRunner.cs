using System.Collections;
using UnityEngine;

namespace BattleCombine.Interfaces
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}
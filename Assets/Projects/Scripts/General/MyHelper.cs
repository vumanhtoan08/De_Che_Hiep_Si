using System.Collections;
using UnityEngine;

public static class MyHelper
{
    /// <summary>
    /// Dùng để chờ theo số frame trong Coroutine
    /// </summary>
    /// <param name="frameCount">Số frame cần chờ</param>
    /// <returns>IEnumerator</returns>
    public static IEnumerator WaitForFrames(int frameCount)
    {
        for (int i = 0; i < frameCount; i++)
        {
            yield return null;
        }
    }
}

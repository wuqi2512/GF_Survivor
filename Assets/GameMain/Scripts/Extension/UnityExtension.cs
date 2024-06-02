using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtension
{
    public static IEnumerator WaitToAction(float waitSeconds, Action action)
    {
        yield return new WaitForSeconds(waitSeconds);
        action();
    }
}
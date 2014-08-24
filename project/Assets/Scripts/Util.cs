using UnityEngine;
using System.Collections;

public static class Util
{
    public static void TweenText(GameObject target)
    {
        LeanTween.cancel(target);
        LeanTween.scale(target, 1.1f * Vector3.one, 0.1f).setEase(LeanTweenType.easeInQuad).setOnComplete(ResetMemoryScale).setOnCompleteParam(target);
    }

    private static void ResetMemoryScale(object target)
    {
        LeanTween.scale(target as GameObject, Vector3.one, 0.4f).setEase(LeanTweenType.easeOutQuad);
    }
}
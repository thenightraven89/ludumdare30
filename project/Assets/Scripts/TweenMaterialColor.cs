using UnityEngine;
using System.Collections;

public class TweenMaterialColor : MonoBehaviour
{
    public Renderer[] renderers;

    public string[] colorNames;

    private LTDescr tweenDescriptor;
    
    public void Tween(Color toColor, float time, LeanTweenType tweenEase, Color? from = null)
    {
        if (tweenDescriptor != null)
        {
            LeanTween.cancel(gameObject, tweenDescriptor.id);
        }

        if (renderers.Length > 0)
        {
            Color currentColor;

            if (from == null)
            {
                currentColor = renderers[0].material.GetColor(colorNames[0]);
            }
            else
            {
                currentColor = (Color)from;
            }
            
            tweenDescriptor = LeanTween.value(gameObject, TweenMaterialsColors, currentColor, toColor, time).setEase(tweenEase);
        }        
    }

    public void Set(Color toColor)
    {
        if (tweenDescriptor != null)
        {
            LeanTween.cancel(gameObject, tweenDescriptor.id);
        }

        TweenMaterialsColors(toColor);
    }
    
    private void TweenMaterialsColors(Color color)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.SetColor(colorNames[i], color);
        }
    }
}
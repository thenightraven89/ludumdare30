using UnityEngine;
using System.Collections;

public class TerrainUnit : MonoBehaviour
{
    public TweenMaterialColor terrainColorTweener;
    
    private Color[] colors;

    void Awake()
    {
        colors = TerrainManager.instance.colors;
        StartCoroutine(ChangeColorRandomly());
    }

    private const float UNIT_RANDOMTILETIME = 2f;

    private IEnumerator ChangeColorRandomly()
    {
        while (true)
        {
            int randomWait = Random.Range(1, 8);
            Color randomColor = colors[Random.Range(0, colors.Length)];            
            terrainColorTweener.Tween(randomColor, 0.5f, LeanTweenType.linear);
            yield return new WaitForSeconds(randomWait * UNIT_RANDOMTILETIME);
        }
    }
}
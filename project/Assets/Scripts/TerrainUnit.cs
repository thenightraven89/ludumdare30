using UnityEngine;
using System.Collections;

public class TerrainUnit : MonoBehaviour
{
    private Color[] colors;
    private Material material;

    private void ChangeTileColor(Color value)
    {
        material.SetColor("_Color", value);
    }

    void Awake()
    {
        colors = TerrainManager.instance.colors;
        material = renderer.material;

        StartCoroutine(ChangeColorRandomly());
    }

    private const float UNIT_RANDOMTILETIME = 1f;

    private IEnumerator ChangeColorRandomly()
    {
        while (true)
        {
            int randomWait = Random.Range(1, 8);
            
            Color randomColor = colors[Random.Range(0, colors.Length)];

            Color currentColor = material.GetColor("_Color");

            LeanTween.value(gameObject, ChangeTileColor, currentColor, randomColor, 0.5f).setEase(LeanTweenType.linear);

            yield return new WaitForSeconds(randomWait * UNIT_RANDOMTILETIME);
        }
    }
}
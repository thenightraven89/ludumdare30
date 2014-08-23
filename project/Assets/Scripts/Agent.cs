using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour
{
    private AgentType agentType;

    private Color mainColor;

    void Awake()
    {
        StartCoroutine(SwitchColorsRandomly());
    }

    private IEnumerator SwitchColorsRandomly()
    {
        while (true)
        {
            SwitchType(Random.Range(0, 4));    

            yield return new WaitForSeconds(4f);
        }
    }

    public void SwitchType(int index)
    {
        agentType = (AgentType)index;
        mainColor = renderer.material.GetColor("_Color");

        StartCoroutine(SwitchColor(index));
    }

    private IEnumerator SwitchColor(int colorIndex)
    {
        LeanTween.value(gameObject, TweenMaterialColor, mainColor, Spawner.instance.agentColors[colorIndex], 0.5f);
        yield return null;
    }

    public void TweenMaterialColor(Color value)
    {
        renderer.material.SetColor("_Color", value);
    }

    public enum AgentType
    {
        Citizen = 0,
        Security = 1,
        Friendly = 2,
        Corrupted = 3
    }
}

using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour
{
    public ParticleSystem particles;

    private AgentType agentType;

    private Color mainColor;

    void Awake()
    {
        StartCoroutine(SwitchColorsRandomly());
        SwitchType(0);
    }

    private IEnumerator SwitchColorsRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            SwitchType(Random.Range(0, 4));
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
        LeanTween.value(gameObject, TweenMaterialColor, mainColor, Spawner.instance.agentColors[colorIndex], 1f);
        yield return null;
    }

    public void TweenMaterialColor(Color value)
    {
        renderer.material.SetColor("_Color", value);
        particles.renderer.material.SetColor("_TintColor", value);
    }

    public enum AgentType
    {
        Citizen = 0,
        Security = 1,
        Friendly = 2,
        Corrupted = 3
    }
}

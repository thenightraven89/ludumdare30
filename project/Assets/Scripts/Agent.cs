using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour
{
    // assigned particle system
    public ParticleSystem particles;

    // the agent's type
    [HideInInspector]
    public AgentType agentType;

    // the agent's color
    private Color mainColor;

    // the complexity level of the agent - how difficult it is to hack it
    // when hacked, throw a die of values 1-to-complexity
    private int complexity;

    // the memory reward
    private int memory;

    public Velocity velocity;

    public TweenMaterialColor colorTweener;
    public TweenMaterialColor messageTweener;
    public TweenMaterialColor fizzTweener;
    public TextMesh messageText;
    private const float TIME_HACK = 2f;

    void Awake()
    {
        messageText.text = "";
        messageTweener.Set(invisibleColor);
        fizzTweener.Set(invisibleColor);

        float securityTypeChance = Random.Range(0f, 1f);

        if (securityTypeChance < Spawner.instance.securityTypeChanceMax)
        {
            SwitchType(1);
            complexity = int.MaxValue;
        }
        else
        {
            SwitchType(0);
            complexity = Dice.Roll(10);
        }

        memory = Dice.Roll(32, 32, 32, 32);
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
        colorTweener.Tween(Spawner.instance.agentColors[index], 1f, LeanTweenType.linear);
    }

    public enum AgentType
    {
        Citizen = 0,
        Security = 1,
        Friendly = 2,
        Corrupted = 3
    }

    public void Suspend()
    {
        velocity.enabled = false;
    }

    public void Resume()
    {
        velocity.enabled = true;
    }

    public Color hackColor;
    public Color escapeHackColor;
    public Color resistHackColor;
    public Color succeedHackColor;    
    public Color invisibleColor;

    int hackerSkill = 0;

    public void EscapeHack()
    {
        if (beingHacked)
        {
            StopCoroutine("ExposeToHack");
            messageText.text = "hacking_ [cancelled]";
            messageTweener.Tween(invisibleColor, 1f, LeanTweenType.easeInExpo, escapeHackColor);
            fizzTweener.Tween(invisibleColor, 0.5f, LeanTweenType.linear);
        }

        beingHacked = false;
    }

    public void ResistHack()
    {
        if (beingHacked)
        {
            messageText.text = "hacking_ [failed]";
            messageTweener.Tween(invisibleColor, 3f, LeanTweenType.easeInExpo, resistHackColor);
            fizzTweener.Tween(invisibleColor, 0.5f, LeanTweenType.linear);
        }

        Util.TweenText(messageText.gameObject);
        beingHacked = false;
    }

    public void SucceedHack()
    {
        if (beingHacked)
        {
            SwitchType(2);
            messageText.text = "hacking_ [100%]";
            messageTweener.Tween(invisibleColor, 3f, LeanTweenType.easeInExpo, succeedHackColor);
            fizzTweener.Tween(invisibleColor, 0.5f, LeanTweenType.linear);
            currentHacker.AddMemory(memory);
        }

        Util.TweenText(messageText.gameObject);
        beingHacked = false;
    }

    Hacking currentHacker = null;

    public void DefendHack(Hacking hacker)
    {
        if (!beingHacked)
        {
            beingHacked = true;
            currentHacker = hacker;
            hackerSkill = hacker.hackSkill;
            StartCoroutine("ExposeToHack");

            Util.TweenText(messageText.gameObject);
        }
    }

    private bool beingHacked = false;

    private IEnumerator ExposeToHack()
    {
        messageText.text = "hacking_";
        messageTweener.Set(hackColor);
        fizzTweener.Tween(hackColor, 0.5f, LeanTweenType.linear);
        float time = 0f;

        while (time < TIME_HACK)
        {
            messageText.text = string.Format("hacking_ [{0}%]", ((int)((time / TIME_HACK) * 100f)).ToString());
            
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        // if agent has been held for enough time, roll the hack dice

        int roll = Dice.Roll(hackerSkill);
        if (roll <= complexity)
        {
            ResistHack();
        }
        else
        {
            SucceedHack();
        }
    }
}

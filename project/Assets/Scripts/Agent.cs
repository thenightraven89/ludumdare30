using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : Entity
{
    [Tooltip("text mesh to display hacking status")]
    public TextMesh messageText;

    [Tooltip("tween hacking message color")]
    public TweenMaterialColor messageTweener;

    // the complexity level of the agent - how difficult it is to get hacked
    private int complexity;
    
    // if present, current hacker that is hacking the agent
    private Human currentHacker = null;

    // true if the agentn is currently in the process of being hacked - could be changed with currentHacker == null
    private bool beingHacked = false;
            
    // time it takes to hack agent
    private const float TIME_HACK = 2f;
    
    protected override void Awake()
    {
        base.Awake();

        // initialize aesthetics
        messageText.text = "";
        messageTweener.Set(new Color(1f, 1f, 1f, 0f));
        fizzTweener.Set(new Color(1f, 1f, 1f, 0f));

        // set agent as citizen
        SwitchType(0);

        // give agent random direction at first
        velocity = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * Vector3.forward;
    }
    
    /// <summary>
    /// switch agent type
    /// </summary>
    /// <param name="index">target type index</param>
    public void SwitchType(int index)
    {
        complexity = Dice.Roll(10);
        memory = Dice.Roll(32, 32, 32, 32);
        entityType = (EntityTypes)index;
        colorTweener.Tween(Spawner.instance.agentColors[index], 1f, LeanTweenType.linear);
    }

    /// <summary>
    /// current hacker's attack is interrupted
    /// </summary>
    public void EscapeHack()
    {
        if (beingHacked)
        {
            StopCoroutine("ExposeToHack");
            messageText.text = "hacking_ [cancelled]";
            messageTweener.Tween(currentHacker.invisibleColor, 1f, LeanTweenType.easeInExpo, currentHacker.escapeHackColor);
            fizzTweener.Tween(currentHacker.invisibleColor, 0.5f, LeanTweenType.linear);
        }

        beingHacked = false;
    }

    /// <summary>
    /// current hacker's attack has failed due to skill level
    /// </summary>
    public void ResistHack()
    {
        if (beingHacked)
        {
            messageText.text = "hacking_ [failed]";
            messageTweener.Tween(currentHacker.invisibleColor, 3f, LeanTweenType.easeInExpo, currentHacker.resistHackColor);
            fizzTweener.Tween(currentHacker.invisibleColor, 0.5f, LeanTweenType.linear);
        }

        Util.TweenText(messageText.gameObject);
        beingHacked = false;
    }

    /// <summary>
    /// current hacker's attack has succeeded
    /// </summary>
    public void SucceedHack()
    {
        if (beingHacked)
        {
            SwitchType(2);
            messageText.text = "hacking_ [100%]";
            messageTweener.Tween(currentHacker.invisibleColor, 3f, LeanTweenType.easeInExpo, currentHacker.succeedHackColor);
            fizzTweener.Tween(currentHacker.invisibleColor, 0.5f, LeanTweenType.linear);
            currentHacker.AddMemory(memory);
        }

        Util.TweenText(messageText.gameObject);
        beingHacked = false;
    }
       
    /// <summary>
    /// current hacker's attack attempt has started
    /// </summary>
    /// <param name="hacker">responsible hacker</param>
    public void DefendHack(Human hacker)
    {
        if (!beingHacked)
        {
            beingHacked = true;
            currentHacker = hacker;
            StartCoroutine("ExposeToHack");

            Util.TweenText(messageText.gameObject);
        }
    }    

    /// <summary>
    /// the getting-hacked coroutine
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExposeToHack()
    {
        messageText.text = "hacking_";
        messageTweener.Set(currentHacker.hackColor);
        fizzTweener.Tween(currentHacker.hackColor, 0.5f, LeanTweenType.linear);
        float time = 0f;

        while (time < TIME_HACK)
        {
            messageText.text = string.Format("hacking_ [{0}%]", ((int)((time / TIME_HACK) * 100f)).ToString());
            
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        // if agent has been held for enough time, roll the hack dice

        int roll = Dice.Roll(currentHacker.hackSkill);
        if (roll <= complexity)
        {
            ResistHack();
        }
        else
        {
            SucceedHack();
        }
    }
    
    protected override void Update()
    {
        base.Update();

        // add capabilities influence to velocity
        for (int i = 0; i < capabilities.Count; i++)
        {
            velocity += capabilities[i].Delta;
        }
        
        // trim velocity if needed
        if (velocity.sqrMagnitude > maxVelocity * maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        Vector3 translateValue = velocity * Time.deltaTime; // * magnitude?

        // limit to movement area
        if ((currentTransform.position + translateValue).x < movementArea.xMin ||
            (currentTransform.position + translateValue).x > movementArea.xMax)
        {
            velocity += new Vector3(-2f * velocity.x, 0f, 0f);
        }

        if ((currentTransform.position + translateValue).z < movementArea.yMin ||
            (currentTransform.position + translateValue).z > movementArea.yMax)
        {
            velocity += new Vector3(0f, 0f, -2f * velocity.z);
        }

        // exclude weaker movement axis - this gives the flocks the 90-degree urban-ish movement
        if (velocity.x * velocity.x > velocity.z * velocity.z)
        {
            velocity = new Vector3(velocity.x, 0f, 0f);
        }
        else
        {
            velocity = new Vector3(0f, 0f, velocity.z);
        }

        // finally, transalte agent with computed velocity
        currentTransform.Translate(velocity * Time.deltaTime);
    }
}
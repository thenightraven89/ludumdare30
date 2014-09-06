using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Human : Entity
{
    private List<Agent> hackableAgents;

    [Range(0, 10)]
    public int hackSkill;

    public TweenMaterialColor ringColorTweener;

    public Color hackColor;
    public Color escapeHackColor;
    public Color resistHackColor;
    public Color succeedHackColor;
    public Color invisibleColor;

    private const float VELOCITY_CANCELHACK = 0.5f;

    protected override void Awake()
    {
        base.Awake();

        velocity = Vector3.zero;

        memory = 0;

        hackableAgents = new List<Agent>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        ProcessInput();

        ProcessHacking();

        ElasticCamera.instance.UpdatePosition();        
    }

    private void ProcessHacking()
    {
        for (int i = 0; i < hackableAgents.Count; i++)
        {
            bool isPresent = false;

            for (int j = 0; j < hits.Count; j++)
            {
                if (hackableAgents[i].Equals(hits[j].GetComponent<Agent>()))
                {
                    isPresent = true;
                }
            }

            if (!isPresent)
            {
                Debug.Log("escaping due to fail processhacking");
                hackableAgents[i].EscapeHack();
                hackableAgents.RemoveAt(i);
            }

            if (isPresent && hackableAgents[i].EntityType != EntityTypes.Citizen)
            {
                hackableAgents.RemoveAt(i);
            }

            if (hackableAgents.Count == 0)
            {
                ringColorTweener.Set(invisibleColor);
            }
        }
    }

    private void ProcessInput()
    {
        float horizontalVelocity = Input.GetAxis("Horizontal") * maxVelocity;
        float verticalVelocity = Input.GetAxis("Vertical") * maxVelocity;

        if (Mathf.Abs(horizontalVelocity) > Mathf.Abs(verticalVelocity))
        {
            currentTransform.Translate(Vector3.right * horizontalVelocity * Time.deltaTime);
        }
        else
        {
            currentTransform.Translate(Vector3.forward * verticalVelocity * Time.deltaTime);
        }

        if (Mathf.Abs(horizontalVelocity) + Mathf.Abs(verticalVelocity) > VELOCITY_CANCELHACK * maxVelocity && hackableAgents.Count > 0)
        {
            Cancel();
        }

        //currentTransform.Translate((Vector3.right * horizontalVelocity + Vector3.forward * verticalVelocity) * magnitude * Time.fixedDeltaTime);

        if (Input.GetButtonDown("Hack"))
        {
            Activate();
        }

        if (Input.GetButtonUp("Hack"))
        {
            Cancel();
        }
    }

    internal void Activate()
    {
        fizzTweener.Tween(mainColor, 0.5f, LeanTweenType.linear, invisibleColor);

        hackableAgents.Clear();

        LeanTween.cancel(gameObject);
        ringColorTweener.Set(Color.white);
             
        for (int i = 0; i < hits.Count; i++)
        {
            Agent agent = hits[i].GetComponent<Agent>();

            // if target is an agent add it to the dictinary
            if (agent != null && agent.EntityType == EntityTypes.Citizen)
            {
                hackableAgents.Add(agent);
                agent.DefendHack(this);
            }
        }
    }
    
    internal void Cancel()
    {
        // cancel all hacks
        for (int i = 0; i < hackableAgents.Count; i++)
        {
            hackableAgents[i].EscapeHack();
        }

        hackableAgents.Clear();

        // aesthetics
        fizzTweener.Tween(invisibleColor, 0.5f, LeanTweenType.linear);
        ringColorTweener.Set(invisibleColor);
    }

    internal void AddMemory(int memoryAmount)
    {
        memory += memoryAmount;
        ElasticCamera.instance.AddMemory(memoryAmount);

        StageManager.instance.AnnounceMemory(this);
        audio.PlayOneShot(audio.clip);
    }

    internal int GetHackedMemory()
    {
        return memory;
    }

    internal void ResetMemory()
    {
        memory = 0;
    }
}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hacking : MonoBehaviour
{
    public GameObject[] rings;

    public Sight sight;

    private List<Agent> hackableAgents;

    [Range(0, 10)]
    public int hackSkill;

    public TweenMaterialColor ringColorTweener;
    public TweenMaterialColor psColorTweener;

    void Awake()
    {
        hackableAgents = new List<Agent>();
        
    }
    
    internal void Activate()
    {
        psColorTweener.Tween(new Color(1f, 1f, 1f, 1f), 0.5f, LeanTweenType.linear, new Color(1f, 1f, 1f, 0f));

        transform.localScale = Vector3.one * (sight.radius + 3f);

        // keep all hacked agents here
        hackableAgents.Clear();

        //ChangeRingsAlpha(Color.white);

        //Debug.Log("hack activated");
        LeanTween.cancel(gameObject);
        ringColorTweener.Set(Color.white);

        //Color currentColor = rings[0].renderer.material.GetColor("_TintColor");
        //ringColorTweener.Tween(new Color(currentColor.r, currentColor.g, currentColor.b, 1f), 0.5f, LeanTweenType.easeOutSine);

        //process sight to compute list of visible targets
        sight.Process();

        //fetch list of targets
        List<Collider> hits = sight.hits;

        for (int i = 0; i < hits.Count; i++)
        {
            Agent agent = hits[i].GetComponent<Agent>();

            // if target is an agent add it to the dictinary
            if (agent != null && agent.agentType == Agent.AgentType.Citizen)
            {
                hackableAgents.Add(agent);
                agent.DefendHack(this);
            }
        }
    }

    private int hackedMemory = 0;
    public string title;

    void Update()
    {
        List<Collider> hits = sight.hits;

        for (int i = 0; i < hackableAgents.Count; i++)
        {
            bool isPresent = false;

            for (int j = 0; j < hits.Count; j++)
            {
                if (hackableAgents[i] == hits[j].GetComponent<Agent>())
                {
                    isPresent = true;
                }
            }

            if (!isPresent)
            {
                hackableAgents[i].EscapeHack();
                hackableAgents.RemoveAt(i);
            }

            if (isPresent && hackableAgents[i].agentType != Agent.AgentType.Citizen)
            {
                hackableAgents.RemoveAt(i);
            }

            if (hackableAgents.Count == 0)
            {
                ringColorTweener.Set(new Color(1f, 1f, 1f, 0f));
            }
        }
    }

    internal void Cancel()
    {
        for (int i = 0; i < hackableAgents.Count; i++)
        {
            hackableAgents[i].EscapeHack();
        }

        psColorTweener.Tween(new Color(1f, 1f, 1f, 0f), 0.5f, LeanTweenType.linear);

        hackableAgents.Clear(); //?????
        //Color currentColor = rings[0].renderer.material.GetColor("_TintColor");
        //ringColorTweener.Tween(new Color(currentColor.r, currentColor.g, currentColor.b, 0f), 0.5f, LeanTweenType.easeInSine);

        ringColorTweener.Set(new Color(1f, 1f, 1f, 0f));
    }

    internal void AddMemory(int memory)
    {
        hackedMemory += memory;
        ElasticCamera.instance.AddMemory(memory);

        StageManager.instance.AnnounceMemory(this);
        audio.PlayOneShot(audio.clip);
    }

    internal int GetHackedMemory()
    {
        return hackedMemory;
    }

    internal void ResetMemory()
    {
        hackedMemory = 0;
    }
}
using UnityEngine;
using System.Collections;

public class ElasticCamera : MonoBehaviour
{
    public static ElasticCamera instance;

    public GameObject target;

    public TextMesh announcer;
    public TextMesh memory;
    public TextMesh time;

    public TweenMaterialColor announcerTweener;

    private int currentMemory;

    [Range(0, 2)]
    public float tweenTime;

    public LeanTweenType easeType;

    private Transform targetTransform;

    void Awake()
    {
        instance = this;
        targetTransform = target.transform;

        announcer.text = "";
        memory.text = "";
        time.text = "";

        currentMemory = 0;
    }

    public void UpdatePosition()
    {        
        transform.position = Vector3.Lerp(transform.position, target.transform.position, 0.25f);
    }

    public void Announce(string text)
    {
        announcer.text = text;
        Util.TweenText(announcer.gameObject);
    }

    internal void AddMemory(int memoryAmount)
    {        
        currentMemory += memoryAmount;
        memory.text = string.Format("memory required_ {0}/{1}kB", currentMemory, StageManager.instance.targetMemory);
        Util.TweenText(memory.gameObject);
    }

    internal void SetTime(int timeAmount)
    {
        time.text = string.Format("time left_ {0}s", timeAmount);
        Util.TweenText(time.gameObject);
    }

    internal void SetMemory(int memoryAmount)
    {
        currentMemory = memoryAmount;
        memory.text = string.Format("memory required_ {0}/{1}kB", currentMemory, StageManager.instance.targetMemory);
        Util.TweenText(memory.gameObject);
    }
}
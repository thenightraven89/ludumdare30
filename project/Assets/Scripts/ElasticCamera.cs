using UnityEngine;
using System.Collections;

public class ElasticCamera : MonoBehaviour
{
    public static ElasticCamera instance;

    public GameObject target;

    [Range(0, 2)]
    public float tweenTime;

    public LeanTweenType easeType;

    private Transform targetTransform;

    void Awake()
    {
        instance = this;
        targetTransform = target.transform;
    }

    public void UpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, 0.25f);
        //LeanTween.move(gameObject, target.transform.position, tweenTime).setEase(easeType);
    }
}
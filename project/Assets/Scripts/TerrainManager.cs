using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;

    public Color[] colors;

    void Awake()
    {
        instance = this;

        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    //transform.GetChild(i).localEulerAngles = new Vector3(90f, Random.Range(-1, 2) * 90f, 0f);
        //    materials.Add(transform.GetChild(i).renderer.material);
        //}
    }
}
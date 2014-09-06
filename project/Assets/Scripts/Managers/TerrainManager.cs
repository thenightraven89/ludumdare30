using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;

    public Color[] colors;

    public int terrainWidth;
    public int terrainLength;

    public GameObject terrainPiece;
    public float unitScale = 5f;

    public Vector3 terrainOffset;

    void Awake()
    {
        instance = this;

        for (int i = 0; i < terrainWidth; i++)
        {
            for (int j = 0; j < terrainLength; j++)
            {
                GameObject newTerrainPiece = Instantiate(terrainPiece, new Vector3(i * unitScale, -0.7f, j * unitScale) + terrainOffset, Quaternion.Euler(90f, 0f, 0f)) as GameObject;
                newTerrainPiece.transform.parent = transform;                
            }
        }
    }
}
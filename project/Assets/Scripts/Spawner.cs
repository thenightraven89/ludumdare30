using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    // amount of times object will be spawned
    [Range(1, 1000)]
    public int spawnCount;

    // object that will be spawned
    public GameObject sourceObject;

    // location of spawning pool
    public Rect spawnArea;

    public Color[] agentColors;

    void Awake()
    {
        instance = this;

        for (int i = 0; i < spawnCount; i++)
        {
            // spawn units and give them random direction

            GameObject newObject = GameObject.Instantiate(
                sourceObject,
                new Vector3(
                    Random.Range(spawnArea.xMin, spawnArea.xMax),
                    0f,
                    Random.Range(spawnArea.yMin, spawnArea.yMax)),
                Quaternion.identity) as GameObject;

            newObject.transform.parent = transform;
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(spawnArea.xMin, 0, spawnArea.yMin), new Vector3(spawnArea.xMax, 0, spawnArea.yMin));
        Gizmos.DrawLine(new Vector3(spawnArea.xMax, 0, spawnArea.yMin), new Vector3(spawnArea.xMax, 0, spawnArea.yMax));
        Gizmos.DrawLine(new Vector3(spawnArea.xMax, 0, spawnArea.yMax), new Vector3(spawnArea.xMin, 0, spawnArea.yMax));
        Gizmos.DrawLine(new Vector3(spawnArea.xMin, 0, spawnArea.yMax), new Vector3(spawnArea.xMin, 0, spawnArea.yMin));
    }
}
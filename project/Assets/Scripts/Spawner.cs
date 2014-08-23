using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    // amount of times object will be spawned
    [Range(1, 100)]
    public int spawnCount;

    // object that will be spawned
    public GameObject sourceObject;

    // location of spawning pool
    public Rect spawnArea;

    void Awake()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // spawn units and give them random direction

            GameObject newObject = GameObject.Instantiate(
                sourceObject,
                new Vector3(
                    Random.Range(spawnArea.xMin, spawnArea.xMax),
                    0f,
                    Random.Range(spawnArea.yMin, spawnArea.yMax)),
                Quaternion.Euler(0f, Random.Range(0f, 360f), 0f)) as GameObject;

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
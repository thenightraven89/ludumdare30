using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    // amount of times object will be spawned
    [Range(1, 1000)]
    public int spawnCount;
    
    [Range(0f, 1f)]
    public float securityTypeChanceMax;

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

    internal void RestoreUnits()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Agent agent = transform.GetChild(i).GetComponent<Agent>();


            float securityTypeChance = Random.Range(0f, 1f);

            if (securityTypeChance < securityTypeChanceMax)
            {
                agent.SwitchType((int)Agent.EntityTypes.Security);
                agent.GetComponent<Cohesion>().magnitude = 0f;
            }
            else
            {
                // set agent as citizen
                agent.SwitchType((int)Agent.EntityTypes.Citizen);
                agent.GetComponent<Cohesion>().magnitude = sourceObject.GetComponent<Cohesion>().magnitude;
            }

            // give agent random direction at first
            agent.SetRandomVelocity();
        }
    }
}
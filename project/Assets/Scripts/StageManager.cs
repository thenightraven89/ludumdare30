using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    //time until stage changes (seconds)
    public int stageTime;

    //required memory to finish stage
    public int targetMemory;

    void Awake()
    {
        instance = this;

        StartCoroutine(AnnounceStage(3));
    }

    private enum StageState
    {
        Announcing,
        Starting,
        Running,
        TimedOut
    }

    private IEnumerator RunStage(int time)
    {
        while (time > -1 && winner == null)
        {
            ElasticCamera.instance.SetTime(time);
            time--;
            yield return new WaitForSeconds(1f);
        }

        if (winner == null)
        {
            StartCoroutine(LoseStage());
        }
        else
        {
            StartCoroutine(WinStage());
        }

        yield return null;
    }

    private IEnumerator WinStage()
    {
        ElasticCamera.instance.Announce(string.Format("tophacker_ {0}", winner.title));

        yield return new WaitForSeconds(3f);

        StartCoroutine(AnnounceStage(3));

        yield return null;
    }

    private IEnumerator LoseStage()
    {
        ElasticCamera.instance.Announce("timed out_");

        yield return new WaitForSeconds(3f);

        StartCoroutine(AnnounceStage(3));

        yield return null;
    }

    private IEnumerator AnnounceStage(int cooldown)
    {
        ElasticCamera.instance.SetMemory(0);
        ElasticCamera.instance.SetTime(stageTime);


        while (cooldown > -1)
        {
            ElasticCamera.instance.Announce(string.Format("round starting in_ {0}", cooldown));
            cooldown--;
            yield return new WaitForSeconds(1f);
        }

        ElasticCamera.instance.Announce("");

        winner = null;
        
        for (int i = 0; i < hackers.Length; i++)
        {
            hackers[i].ResetMemory();
        }

        StartCoroutine(RunStage(stageTime));
        
        yield return null;
    }

    public Hacking[] hackers;

    Hacking winner = null;

    public void AnnounceWinner(Hacking hacker)
    {
        if (winner == null)
        {
            winner = hacker;
        }
    }

    internal void AnnounceMemory(Hacking hacker)
    {
        if (hacker.GetHackedMemory() >= targetMemory && winner == null)
        {
            winner = hacker;
        }
    }
}
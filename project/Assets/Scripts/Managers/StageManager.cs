using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public int TargetMemory
    {
        get { return targetMemories[currentStage]; }
    }

    public int StageTime
    {
        get { return stageTimes[currentStage]; }
    }


    public int[] stageTimes;

    public int[] targetMemories;

    private int currentStage;

    public GameObject mainScreen;

    public AudioSource music;
    public AudioClip winSound;
    public AudioClip loseSound;

    void Awake()
    {
        instance = this;

        currentStage = 0;
        StartCoroutine(ExplainStage());
    }

    private enum StageState
    {
        Announcing,
        Starting,
        Running,
        TimedOut,
        Intercepted
    }

    private IEnumerator RunStage(int time)
    {
        while (time > -1 && winner == null && !intercepted)
        {
            ElasticCamera.instance.SetTime(time);
            time--;
            yield return new WaitForSeconds(1f);
        }

        if (intercepted)
        {
            StartCoroutine(LoseStageIntercept());
        }
        else if (winner == null)
        {
            StartCoroutine(LoseStageTimeOut());
        }
        else
        {
            StartCoroutine(WinStage());
        }

        yield return null;
    }

    private bool nothingPressed;

    public Human controller;

    private IEnumerator ExplainStage()
    {
        mainScreen.SetActive(true);

        nothingPressed = true;

        while (nothingPressed)
        {
            if (Input.GetButton("Hack")) nothingPressed = false;

            yield return new WaitForEndOfFrame();
        }
        controller.enabled = true;

        mainScreen.SetActive(false);
        music.Play();
        StartCoroutine(AnnounceStage(3));
        yield return null;
    }

    private IEnumerator WinStage()
    {
        music.PlayOneShot(winSound);

        ElasticCamera.instance.Announce("download complete_");
        //ElasticCamera.instance.Announce(string.Format("tophacker_ {0}", winner.title));

        yield return new WaitForSeconds(3f);

        currentStage++;

        if (currentStage >= stageTimes.Length)
        {
            StartCoroutine(FinishGame());
        }
        else
        {
            StartCoroutine(AnnounceStage(3));
        }

        yield return null;
    }

    private IEnumerator FinishGame()
    {
        ElasticCamera.instance.Announce("well, it looks like you're done\nthank you for playing!\npress [x] to start anew_");
        nothingPressed = true;
        controller.enabled = false;

        music.Stop();
        ElasticCamera.instance.ClearTime();
        ElasticCamera.instance.ClearMemory();

        while (nothingPressed)
        {
            if (Input.GetButton("Hack")) nothingPressed = false;

            yield return new WaitForEndOfFrame();
        }

        Application.LoadLevel(0);

        yield return null;
    }

    private IEnumerator LoseStageTimeOut()
    {
        music.PlayOneShot(loseSound);

        ElasticCamera.instance.Announce("timed out_");

        yield return new WaitForSeconds(3f);

        StartCoroutine(AnnounceStage(3));

        yield return null;
    }

    private IEnumerator LoseStageIntercept()
    {
        music.PlayOneShot(loseSound);

        ElasticCamera.instance.Announce("intercepted_");

        yield return new WaitForSeconds(3f);

        StartCoroutine(AnnounceStage(3));

        yield return null;
    }

    private IEnumerator AnnounceStage(int cooldown)
    {
        Spawner.instance.RestoreUnits();

        ElasticCamera.instance.SetMemory(0);
        ElasticCamera.instance.SetTime(StageTime);
        
        while (cooldown > -1)
        {
            ElasticCamera.instance.Announce(string.Format("round {0} starting in_ {1}", currentStage, cooldown));
            cooldown--;
            yield return new WaitForSeconds(1f);
        }

        ElasticCamera.instance.Announce("");

        winner = null;
        intercepted = false;
        
        for (int i = 0; i < hackers.Length; i++)
        {
            hackers[i].ResetMemory();
        }

        StartCoroutine(RunStage(StageTime));
        
        yield return null;
    }

    public Human[] hackers;

    private Human winner = null;
    private bool intercepted = false;

    public void AnnounceWinner(Human hacker)
    {
        if (winner == null)
        {
            winner = hacker;
        }
    }

    public void AnnounceIntercept()
    {
        intercepted = true;
    }

    internal void AnnounceMemory(Human hacker)
    {
        if (hacker.GetHackedMemory() >= TargetMemory && winner == null)
        {
            winner = hacker;
        }
    }
}
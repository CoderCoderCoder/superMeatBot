using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class LoadExternal : MonoBehaviour
{
    public static LoadExternal instance;

    public GameObject academyPrefab;

    public byte[][] iterationData;
    public Academy[] academies;
    public Agent agent;

    private ThreadedJob job;
    public int maxIteration;
    private static int toLoad;
    public static int maxIterations = 50;
    internal static long lastWaitedTicks;
    public static readonly string path = "C:\\UnityML\\python_allInOne\\models\\123\\";


    string path1 = "C:\\UnityML\\python\\models\\123\\";
    string path2 = "C:\\UnityML\\python2\\models\\123\\";
    private int maxCheckpoint = 0;
    private static int lastLoadedCheckpoint = 0;
    public static bool runningCheckpointThread = false;

    // Use this for initialization
    void Start()
    {
        instance = this;
        iterationData = new byte[maxIterations][];
        academies = new Academy[maxIterations];
    }

    // Update is called once per frame
    void Update()
    {
        if (toLoad != maxIteration)
        {
            StartCoroutine(LoadModelInternal(toLoad));
        }

        if(!runningCheckpointThread && maxCheckpoint != lastLoadedCheckpoint)
        {
            runningCheckpointThread = true;
            lastLoadedCheckpoint = maxCheckpoint;
            CopyCheckpointAndRun(maxCheckpoint);
        }
    }

    private void OnDestroy()
    {
        if (job != null)
            job.Abort();
    }

    public void CallLearner()
    {
        LoadingBar.instance.StartLoading();
        maxIteration = 0;
        toLoad = 0;
        CleanModelFolders();
        StartCoroutine(WaitForCheckpoints());
        StartCoroutine(CreateCheckpoints());
        //job = new ThreadedJob();
        //job.Start();
    }

    internal static void NotifyCheckpointModelCreated()
    {
        runningCheckpointThread = false;
        toLoad = lastLoadedCheckpoint;
    }

    private IEnumerator CreateCheckpoints()
    {
        ExecuteCommand("C:\\UnityML\\python\\masterScript.bat", true);
        yield break;
    }

    public IEnumerator WaitForCheckpoints()
    {
        int maxIteration = 50;
        for (int i = 1; i <= maxIteration; i++)
        {
            do
            {
                if (File.Exists(path1 + "model-" + i + "000.cptk.index"))
                {
                    maxCheckpoint = i;
                    break;
                }
                yield return new WaitForSeconds(1);
            } while (true);
        }
    }

    public void CopyCheckpointAndRun(int iteration)
    {
        DirectoryInfo di = new DirectoryInfo(path1);
        foreach (FileInfo file in di.GetFiles())
        {
            if (file.Name.StartsWith("model-" + iteration + "000.cptk"))
                System.IO.File.Copy(file.FullName, path2 + file.Name, true);
        }

        string checkpointFile = path2 + "checkpoint";
        using (StreamWriter sw = File.CreateText(checkpointFile))
        {
            sw.WriteLine("model_checkpoint_path: \"model-" + iteration + "000.cptk\"");
            sw.WriteLine("all_model_checkpoint_paths: \"model-" + iteration + "000.cptk\"");
            sw.WriteLine();


        }
        ExecuteCommand("C:\\UnityML\\python2\\masterScript.bat", true, true);

        UnityEngine.Debug.Log("iteration " + iteration + " done!");
    }

    public void ExecuteCommand(string command, bool thread = false, bool resetRunningCheckpointThreadOnEnd = false)
    {

        if (thread)
        {
            new ThreadedJobCheckpoint(command, resetRunningCheckpointThreadOnEnd).Start();
        }
        else
        {
            int ExitCode;
            ProcessStartInfo ProcessInfo;
            Process Process;

            ProcessInfo = new ProcessStartInfo("cmd.exe", "/c " + command);

            ProcessInfo.CreateNoWindow = false;//Change that

            ProcessInfo.UseShellExecute = false;

            Process = Process.Start(ProcessInfo);

            Process.WaitForExit();

            ExitCode = Process.ExitCode;
            Process.Close();

            UnityEngine.Debug.Log("ExitCode: " + ExitCode.ToString());

            if (resetRunningCheckpointThreadOnEnd)
                NotifyCheckpointModelCreated();
        }

    }



    public void CleanModelFolders()
    {
        DirectoryInfo di = new DirectoryInfo(path);
        DirectoryInfo di2 = new DirectoryInfo(path1);
        DirectoryInfo di3 = new DirectoryInfo(path2);

        foreach (FileInfo file in di.GetFiles())
        {
            if (file.Name.StartsWith("model") || file.Name.StartsWith("checkpoint"))
                file.Delete();
        }
        foreach (FileInfo file in di2.GetFiles())
        {
            if (file.Name.StartsWith("model") || file.Name.StartsWith("checkpoint"))
                file.Delete();
        }
        foreach (FileInfo file in di3.GetFiles())
        {
            if (file.Name.StartsWith("model") || file.Name.StartsWith("checkpoint"))
                file.Delete();
        }
    }

    public void LoadModel(int iteration)
    {
        toLoad = iteration;
    }

    public IEnumerator LoadModelInternal(int iteration)
    {
        PlayerPrefs.SetString("lastIterationTime", lastWaitedTicks + "");
        LoadingBar.instance.StartLoading();
        maxIteration = iteration;
        string path_byte = path2 + "testGame_123.bytes";

        WWW graphModel = new WWW(path_byte);

        yield return graphModel;

        iterationData[iteration - 1] = graphModel.bytes;

        CreateAcademy(iteration);
        //CoreBrain Will load this static graphModel !!
        //gameToActivateOnLearned.SetActive(true);



    }

    private void CreateAcademy(int iteration)
    {
        academies[iteration-1] = Instantiate(academyPrefab, transform).GetComponent<Academy>();
        agent.brain = academies[iteration - 1].transform.GetChild(0).GetComponent<Brain>();
    }

    internal void SetMaxIteration(int i)
    {
        maxIteration = i;
    }
}

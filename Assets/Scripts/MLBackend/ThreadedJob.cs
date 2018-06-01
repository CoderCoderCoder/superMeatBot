using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class ThreadedJob
{
    private bool m_IsDone = false;
    private object m_Handle = new object();
    private System.Threading.Thread m_Thread = null;

    private readonly string trainer_config = "default:\n" +
"    trainer: ppo\n" +
"    batch_size: 1024\n" +
"    beta: 5.0e-3\n" +
"    buffer_size: 10240\n" +
"    epsilon: 0.2\n" +
"    gamma: 0.99\n" +
"    hidden_units: 128\n" +
"    lambd: 0.95\n" +
"    learning_rate: 3.0e-4\n" +
"    max_steps: [STEPS]\n" +
"    memory_size: 256\n" +
"    normalize: false\n" +
"    num_epoch: 3\n" +
"    num_layers: 2\n" +
"    time_horizon: 64\n" +
"    sequence_length: 64\n" +
"    summary_freq: 1000\n" +
"    checkpoint_freq: 111\n" +
"    use_recurrent: false\n" +
"\n" +
"Ball3DBrain:\n" +
"    normalize: true\n" +
"    batch_size: 1200\n" +
"    buffer_size: 12000\n" +
"    summary_freq: 1000\n" +
"    time_horizon: 1000\n" +
"    gamma: 0.995\n" +
"    beta: 0.001";

    
    public bool IsDone {
        get {
            bool tmp;
            lock (m_Handle)
            {
                tmp = m_IsDone;
            }
            return tmp;
        }
        set {
            lock (m_Handle)
            {
                m_IsDone = value;
            }
        }
    }

    public virtual void Start()
    {
        m_Thread = new System.Threading.Thread(Run);
        m_Thread.Start();
    }
    public virtual void Abort()
    {
        m_Thread.Abort();
    }

    protected virtual void ThreadFunction() {
        for (int i = 1; i <= LoadExternal.maxIterations; i++)
        {
            PrepareIteration(i);
            if (i == 1)
                ExecuteCommand("C:\\UnityML\\python_allInOne\\masterScript.bat");
            else
                ExecuteCommand("C:\\UnityML\\python_allInOne\\masterScript2.bat");

            UnityEngine.Debug.Log("Iteration done: " + i);

            LoadExternal.instance.LoadModel(i);
        }

    }


    private void PrepareIteration(int i)
    {
        using (System.IO.StreamWriter sw = File.CreateText("C:\\UnityML\\python_allInOne\\trainer_config.yaml"))
        {
            int steps = 5000 * i;
            sw.WriteLine(trainer_config.Replace("[STEPS]", steps + ""));
        }
    }


    public void ExecuteCommand(string command, bool thread = false)
    {
        
        int ExitCode;
        ProcessStartInfo ProcessInfo;
        Process Process;

        ProcessInfo = new ProcessStartInfo("cmd.exe", "/c " + command);

        ProcessInfo.CreateNoWindow = true;//Change that

        ProcessInfo.UseShellExecute = false;

        Process = Process.Start(ProcessInfo);
        long startTicks = System.DateTime.Now.Ticks;
        Process.WaitForExit();
        System.DateTime startTime = System.DateTime.Now;
        long waitingTicks = System.DateTime.Now.Ticks - startTicks;
        LoadExternal.lastWaitedTicks = waitingTicks;
        UnityEngine.Debug.Log("Waited Ticks: " + waitingTicks);
        ExitCode = Process.ExitCode;
        Process.Close();

        UnityEngine.Debug.Log("ExitCode: " + ExitCode.ToString());

    }
    protected virtual void OnFinished() { }

    public virtual bool Update()
    {
        if (IsDone)
        {
            OnFinished();
            return true;
        }
        return false;
    }
    public IEnumerator WaitFor()
    {
        while (!Update())
        {
            yield return null;
        }
    }
    private void Run()
    {
        ThreadFunction();
        IsDone = true;
    }
}
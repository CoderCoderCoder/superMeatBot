using System.Collections;
using System.Diagnostics;

public class ThreadedJobCheckpoint
{
    private bool m_IsDone = false;
    private object m_Handle = new object();
    private System.Threading.Thread m_Thread = null;


    private string command;
    private bool resetRunningCheckpointThreadOnEnd;

    public ThreadedJobCheckpoint(string command, bool resetRunningCheckpointThreadOnEnd)
    {
        this.command = command;
        this.resetRunningCheckpointThreadOnEnd = resetRunningCheckpointThreadOnEnd;
    }

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

    protected virtual void ThreadFunction()
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
        {
            LoadExternal.NotifyCheckpointModelCreated();
        }
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
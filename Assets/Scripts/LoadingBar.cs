using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {

    private Image image;
    private long maxTicks, startTick;
    private bool running = false;

    public static LoadingBar instance;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        instance = this;
    }

    public void StartLoading()
    {
        string lastTicksStr = PlayerPrefs.GetString("lastIterationTime", "729449090");
        maxTicks = long.Parse(lastTicksStr);
        image.fillAmount = 0;
        startTick = System.DateTime.Now.Ticks;
        running = true;
    }

    // Update is called once per frame
    void Update () {
        if (running)
        {
            long ticksAlready = System.DateTime.Now.Ticks - startTick;
            image.fillAmount = Mathf.Clamp01((float)ticksAlready / maxTicks);
        }
	}
}

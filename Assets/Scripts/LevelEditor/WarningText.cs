using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningText : MonoBehaviour {

    [SerializeField]
    private Text warningTextUi;

    [SerializeField]
    private AnimationCurve warningAlpha;

    private float timeSinceWarningEmitted = float.PositiveInfinity;
	
	// Update is called once per frame
	void Update () {
        UpdateTime(Time.deltaTime);
        ApplyColorToUi();
	}

    private void UpdateTime(float deltaTime)
    {
        timeSinceWarningEmitted += deltaTime;
    }

    private void ApplyColorToUi()
    {
        var color = warningTextUi.color;
        color.a = warningAlpha.Evaluate(timeSinceWarningEmitted);
        warningTextUi.color = color;
    }

    internal void EmitWarning(string warningText)
    {
        warningTextUi.text = warningText;
        timeSinceWarningEmitted = 0f;
    }
}

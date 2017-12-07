using System.Collections.Generic;
using UnityEngine;

using AirSig;

public class DeveloperDefinedTest : BasedGestureHandle
{

    // Callback for receiving signature/gesture progression or identification results
    AirSigManager.OnDeveloperDefinedMatch developerDefined;
    RightController rc;

    // Handling developer defined gesture match callback - This is invoked when the Mode is set to Mode.DeveloperDefined and a gesture is recorded.
    // gestureId - a serial number
    // gesture - gesture matched or null if no match. Only guesture in SetDeveloperDefinedTarget range will be verified against
    // score - the confidence level of this identification. Above 1 is generally considered a match
    void HandleOnDeveloperDefinedMatch(long gestureId, string gesture, float score)
    {
        textToUpdate = string.Format("<color=cyan>Gesture Match: {0} Score: {1}</color>", gesture.Trim(), score);
        if (gesture.Equals("Square"))
        {
            Debug.Log("Drew a square!");
            rc.setSpawn(gesture.Trim());
        }
        else if (gesture.Equals("Circle"))
        {
            Debug.Log("Drew a circle!");
            rc.setSpawn(gesture.Trim());
        }
    }

    // Use this for initialization
    void Awake()
    {
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

        // Update the display text
        //textMode.text = string.Format("Mode: {0}", AirSigManager.Mode.DeveloperDefined.ToString());
        // textResult.text = defaultResultText = "Pressing trigger and write symbol in the air\nReleasing trigger when finish";
        //textResult.alignment = TextAnchor.UpperCenter;
        instruction.SetActive(false);
        ToggleGestureImage("All");

        // Configure AirSig by specifying target 
        developerDefined = new AirSigManager.OnDeveloperDefinedMatch(HandleOnDeveloperDefinedMatch);
        airsigManager.onDeveloperDefinedMatch += developerDefined;
        airsigManager.SetMode(AirSigManager.Mode.DeveloperDefined);
        airsigManager.SetDeveloperDefinedTarget(new List<string> { "Circle", "Square" });
        airsigManager.SetClassifier("SquareCircle", "");

        checkDbExist();

        airsigManager.SetTriggerStartKeys(
            AirSigManager.Controller.RIGHT_HAND,
            SteamVR_Controller.ButtonMask.Trigger,
            AirSigManager.PressOrTouch.PRESS);


        //        airsigManager.SetTriggerStartKeys(
        //            AirSigManager.Controller.LEFT_HAND,
        //			SteamVR_Controller.ButtonMask.Trigger,
        //            AirSigManager.PressOrTouch.PRESS);

        rc = GameObject.Find("Controller (right)").GetComponent<RightController>();

    }


    void OnDestroy()
    {
        // Unregistering callback
        airsigManager.onDeveloperDefinedMatch -= developerDefined;
    }

    void Update()
    {
        UpdateUIandHandleControl();
    }
}

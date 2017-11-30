using System.Collections.Generic;
using UnityEngine;

using AirSig;

public class PlayerSignature : BasedGestureHandle {

    // Gesture index to use for training and verifying player signature. Valid index is 100 only
    readonly int PLAYER_SIGNATURE_INDEX = 100;

    // Callback for receiving signature/gesture progression or identification results
    AirSigManager.OnPlayerSignatureTrained signatureTrained;
    AirSigManager.OnPlayerSignatureMatch signatureMatch;

    // Handling player signature match callback - This is invoked when the Mode is set to Mode.IdentifyPlayerSignature and a gesture is recorded.
    // gestureId - a serial number
    // match - true/false indicates that whether a gesture recorded match the gesture trained
    // targetIndex - one of the index in the SetTarget range.
    void HandleOnPlayerSignatureMatch(long gestureId, bool match, int targetIndex) {
        string result = "<color=red>Player signature failed to match</color>";
        if (PLAYER_SIGNATURE_INDEX == targetIndex && match) {
            result = string.Format("<color=cyan>Player signature match ^_^</color>");
        }
        textToUpdate = result;
    }

    // Handling player signature training callback - This is invoked when the Mode is set to Mode.TrainPlayerSignature and a gesture is recorded.
    // gestureId - a serial number
    // error - error while training for this signature
    // progress - progress of training. 1 indicates the training is completed
    // securityLevel - the strength of this player sinature
    void HandleOnPlayerSignatureTrained(long gestureId, AirSigManager.Error error, float progress, AirSigManager.SecurityLevel securityLevel) {
        if (null == error) {
            if (progress < 1.0f) {
                textToUpdate = string.Format("Player signature training\nunder progress {0}%", Mathf.RoundToInt(progress * 100));
            } else {
                // The training has completed, switch to the identification mode
                nextUiAction = () => {
                    StopCoroutine(uiFeedback);
                    SwitchToIdentify();
                };
            }
        } else {
            textToUpdate = string.Format("<color=red>This attempt of training failed\ndue to {0} (see error code document),\ntry again</color>", error.code);
        }
    }

    void SwitchToIdentify() {
        textResult.text = defaultResultText = string.Format("Try write the trained signature to identify\nPress the Application key to reset");
        textMode.text = string.Format("Mode: {0}", AirSigManager.Mode.IdentifyPlayerSignature.ToString());
        airsigManager.SetMode(AirSigManager.Mode.IdentifyPlayerSignature);
        airsigManager.SetTarget(new List<int> { PLAYER_SIGNATURE_INDEX });
    }

    void ResetSignature() {
        airsigManager.DeletePlayerRecord(PLAYER_SIGNATURE_INDEX);
        textResult.text = defaultResultText = "Pressing trigger and write a signature in the air\nReleasing trigger when finish\nUse the Application key to reset progress";
        textMode.text = string.Format("Mode: {0}", AirSigManager.Mode.TrainPlayerSignature.ToString());
        airsigManager.SetMode(AirSigManager.Mode.TrainPlayerSignature);
        airsigManager.SetTarget(new List<int> { PLAYER_SIGNATURE_INDEX });
    }

    // Use this for initialization
    void Awake() {
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

        // Update the display text
        textResult.alignment = TextAnchor.UpperCenter;
        instruction.SetActive(false);
        ToggleGestureImage("");

        // Configure AirSig by specifying target 
        signatureTrained = new AirSigManager.OnPlayerSignatureTrained(HandleOnPlayerSignatureTrained);
        airsigManager.onPlayerSignatureTrained += signatureTrained;
        signatureMatch = new AirSigManager.OnPlayerSignatureMatch(HandleOnPlayerSignatureMatch);
        airsigManager.onPlayerSignatureMatch += signatureMatch;

        ResetSignature();

        airsigManager.SetTriggerStartKeys(
            AirSigManager.Controller.RIGHT_HAND,
            SteamVR_Controller.ButtonMask.Trigger,
            AirSigManager.PressOrTouch.PRESS);


        airsigManager.SetTriggerStartKeys(
            AirSigManager.Controller.LEFT_HAND,
            SteamVR_Controller.ButtonMask.Touchpad,
            AirSigManager.PressOrTouch.PRESS);

    }


    void OnDestroy() {
        // Unregistering callback
        airsigManager.onPlayerSignatureTrained -= signatureTrained;
        airsigManager.onPlayerSignatureMatch -= signatureMatch;
    }

    void Update() {
        UpdateUIandHandleControl();

        if (-1 != (int)rightHandControl.index) {
            var device = SteamVR_Controller.Input((int)rightHandControl.index);
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu)) {
                ResetSignature();
            }
        }
    }
}
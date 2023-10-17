using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using bear.j.easy_dialog;
using UnityEngine.SceneManagement;

public class Device_check : MonoBehaviour
{
    [SerializeField] ARSession m_Session;

    IEnumerator Start()
    {
        ARSession.stateChanged += ARSession_stateChanged;

        if ((ARSession.state == ARSessionState.None) || (ARSession.state == ARSessionState.CheckingAvailability))
        {
            yield return ARSession.CheckAvailability();
        }

        if (ARSession.state == ARSessionState.Unsupported)
        {
            //todo Start some fallback experience for unsupported devices
            //Debug.Log("设备不支持AR");
            Canvas_confirm_box.confirm_box
            (
                 "Hint",
                 "Sorry, your device does not support AR function, please replace other devices",
                 "Cancel",
                 "Confirm",
                 true,
                 delegate () { },
                 delegate () 
                 {
                     SceneManager.LoadSceneAsync("main_ui");
                 }
           );
        }
        else
        {
            // Start the AR session
            m_Session.enabled = true;
        }
    }
    private void ARSession_stateChanged(ARSessionStateChangedEventArgs obj)
    {
        //throw new System.NotImplementedException();
    }
}

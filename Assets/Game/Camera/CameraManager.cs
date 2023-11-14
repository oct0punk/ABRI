using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{    
    CinemachineBrain brain;
    public static CameraManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        brain = GetComponent<CinemachineBrain>();
    }


    public static void Possess(ICinemachineCamera newCam)
    {
        Instance.brain.ActiveVirtualCamera.Priority = 0;
        newCam.Priority = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.ComponentModel;

public class CameraManager : MonoBehaviour
{    
    CinemachineBrain brain;
    public static CameraManager Instance { get; private set; } 

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        brain = GetComponent<CinemachineBrain>();
    }

    public static float Possess(ICinemachineCamera newCam)
    {
        ICinemachineCamera from = Instance.brain.ActiveVirtualCamera;
        Instance.brain.ActiveVirtualCamera.Priority = 0;
        newCam.Priority = 1;
        return Instance.brain.m_CustomBlends.GetBlendForVirtualCameras(from.Name, newCam.Name, new CinemachineBlendDefinition()).m_Time;
    }
}

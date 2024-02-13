using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{    
    CinemachineBrain brain;
    public static CameraManager Instance { get { return GameManager.instance.GetComponent<CameraManager>(); } } 

    // Start is called before the first frame update
    void Awake()
    {
        brain = GetComponent<CinemachineBrain>();
    }

    public static float Possess(ICinemachineCamera newCam)
    {
        ICinemachineCamera from = Instance.brain.ActiveVirtualCamera;
        Instance.brain.ActiveVirtualCamera.Priority = 0;
        newCam.Priority = 1;
        float res = Instance.brain.m_CustomBlends.GetBlendForVirtualCameras(from.Name, newCam.Name, new CinemachineBlendDefinition()).m_Time;
        if (res < .1f) return Instance.brain.m_DefaultBlend.BlendTime;
        return res;
    }
}

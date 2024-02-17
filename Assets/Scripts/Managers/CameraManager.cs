using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{    
    CinemachineBrain brain;
    public static CameraManager Instance;
    public Volume volume;

    // Start is called before the first frame update
    void Awake()
    {        
        Instance = this;
        brain = GetComponent<CinemachineBrain>();

        DepthOfField dof;
        volume.profile.TryGet(out dof);
        if (dof != null)
            dof.focalLength.Override(Screen.dpi / 4);
    }

    public static float Possess(ICinemachineCamera newCam)
    {
        float res = Instance.brain.m_DefaultBlend.BlendTime;
        if (Instance.brain.ActiveVirtualCamera != null)
        {
            ICinemachineCamera from = Instance.brain.ActiveVirtualCamera;
            res = Instance.brain.m_CustomBlends.GetBlendForVirtualCameras(from.Name, newCam.Name, new CinemachineBlendDefinition()).m_Time;
            Instance.brain.ActiveVirtualCamera.Priority = 0;
        }
        newCam.Priority = 1;
        return res;
    }
}

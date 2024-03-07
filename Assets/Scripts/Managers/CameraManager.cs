using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    CinemachineBrain brain;
    public static CameraManager Instance;
    [SerializeField] Volume volume;
    [SerializeField] ParticleSystem feathers;
    int noticeFeathers = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        brain = GetComponent<CinemachineBrain>();

        //DepthOfField dof;
        //volume.profile.TryGet(out dof);
        //if (dof != null)
        //    dof.focalLength.Override(Screen.dpi / 4);
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

    public void EmitFeathers()
    {
        if (GameManager.instance.gameState == GameState.Indoor) return; 


        if (noticeFeathers == 0)
            Lumberjack.Instance.Message("Ce sont les plumes de l'oiseau que je cherche!", 1.0f, feathers.Play, true);

        else if (noticeFeathers == 4)
            Lumberjack.Instance.Message("Encore des plumes, j'esp�re qu'il n'est plus tr�s loin.", 1.0f, feathers.Play, true);

        noticeFeathers++;
    }
}

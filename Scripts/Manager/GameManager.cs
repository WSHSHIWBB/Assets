using UnityEngine;
using VRFrameWork;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        ModuleManager.Instance.RegisterAllModules();
    }

    private void OnDestroy()
    {
        ModuleManager.Instance.UnRegisterAllModule();
    }



}

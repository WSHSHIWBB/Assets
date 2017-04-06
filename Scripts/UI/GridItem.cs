using UnityEngine;
using VRFrameWork;
using VRTK;

public class GridItem : MonoBehaviour
{
    [HideInInspector]
    public int Index;
    [HideInInspector]
    public Tips Tips;

    private ObjectsConfigModule _objectsConfigModule;
    private BagModule _bagModule;
    private ObjectInfo _objectInfo;
    private float _grabDelay = 1f;
    private float _grabTimer = 0;

    private void Awake()
    {
        _objectsConfigModule = ModuleManager.Instance.Get<ObjectsConfigModule>();
        _bagModule = ModuleManager.Instance.Get<BagModule>();
    }

    private void Start()
    {
        int id = _bagModule.GetIDByIndex(Index);
        if (id != -1)
        {
            _objectInfo = _objectsConfigModule.GetObjectInfoByID(id);
            string Name = _objectInfo.EnglishName;
            GameObject gridItem3D = Instantiate(Resources.Load<GameObject>("Prefabs/3DUIObject/" + Name), transform, false);
            if (gridItem3D == null)
            {
                Debug.LogError("Can't find gridItem3D at parth Prefabs/3DUIObject/" + Name);
                return;
            }
        }
        OnScrollMove(0);
    }

    public void OnScrollMove(int currentPage)
    {

        if (transform.childCount > 1)
        {
            Transform GridItem3D = transform.GetChild(1);
            if (Index >= currentPage * 9 && Index < (currentPage + 1) * 9)
            {
                GridItem3D.gameObject.SetActive(true);
            }
            else
            {
                GridItem3D.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            ShowTips();
            GrabObject(other);
        }
    }

    private void GrabObject(Collider other)
    {
        if(_objectInfo==null)
        {
            return;
        }

        VRTK_InteractGrab grab = (other.GetComponent<VRTK_InteractGrab>() ? other.GetComponent<VRTK_InteractGrab>() : other.GetComponentInParent<VRTK_InteractGrab>());
        if(grab && grab.GetGrabbedObject()==null && grab.gameObject.GetComponent<VRTK_ControllerEvents>().grabPressed && Time.time >= _grabTimer)
        {
            string Name = _objectInfo.EnglishName;
            VRTK_InteractTouch touch = grab.gameObject.GetComponent<VRTK_InteractTouch>();
            GameObject prefab = Instantiate(Resources.Load<GameObject>("Prefabs/LabObjects/" + Name),null,false);
            if(prefab==null)
            {
                Debug.LogError("The labObjects of name: " + Name + " is null!");
                return;
            }

            touch.ForceTouch(prefab);
            grab.AttemptGrab();
            _grabTimer = Time.time + _grabDelay;
        }
    }

    private void ShowTips()
    {
        if (!Tips.gameObject.activeInHierarchy)
        {
            SetTips();
            Tips.RefreshFrame(Time.frameCount);
        }
        else
        {
            if (!Tips.Judge1Frame(Time.frameCount))
            {
                SetTips();
            }
            Tips.RefreshFrame(Time.frameCount);
        }
    }

    private void SetTips()
    {
        if (!Tips)
        {
            Debug.Log("Tips is null!");
            return;
        }
        if (transform.childCount < 2)
        {
            Debug.Log("ChildCout < 2!");
            return;
        }
        if (_objectInfo == null)
        {
            Debug.Log("ObjectInfo is null!");
            return;
        }

        Tips.gameObject.SetActive(true);
        string describ = _objectInfo.Description;
        describ = System.Text.RegularExpressions.Regex.Replace(describ, @"(\w{6})", "$0\n");
        Tips.SetTips(transform.position, describ, Index % 9);
        Tips.currentShow = transform;
    }



}

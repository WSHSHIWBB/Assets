using UnityEngine;
using VRFrameWork;

public class VRToggle : MonoBehaviour
{
    private bool _isOn = false;
    public bool IsOn
    {
        get
        {
            return _isOn;
        }
        set
        {
            _isOn = value;
        }
    }

    public VRToggleGroup Group;
    private Animator animator;

    void Start()
    {
        if (Group)
        {
            Group.Add(this);
        }
        animator = GetComponent<Animator>();
        UIEventListener.AddUIListener(gameObject).SetEventHandler(EnumUIinputType.OnEnter, new UIEventHandler(OnEnter), null);
        UIEventListener.AddUIListener(gameObject).SetEventHandler(EnumUIinputType.OnExit, new UIEventHandler(OnExit), null);
        UIEventListener.AddUIListener(gameObject).SetEventHandler(EnumUIinputType.OnDown, new UIEventHandler(OnDown), null);
    }

    public void OnNormal()
    {
        animator.SetTrigger("Normal");
    }

    public void OnHightLighted()
    {
        animator.SetTrigger("Highlighted");
        //tobe hightlighted select
    }

    public void OnSelected()
    {
        //tobe selected handler
        animator.SetTrigger("Pressed");
        //tobe sound select
    }

    private void OnEnter(GameObject linster, object _arg, object[] _params)
    {
        if (!_isOn)
        {
            OnHightLighted();
        }
    }

    private void OnExit(GameObject linster, object _arg, object[] _params)
    {
        if (!_isOn)
        {
            OnNormal();
        }
    }

    private void OnDown(GameObject linster, object _arg, object[] _params)
    {
        if (Group)
        {
            Group.SetOn(this);
        }
        else
        {
            _isOn = !_isOn;
            if(_isOn)
            {
                OnSelected();
            }
            else
            {
                OnHightLighted();
            }
        }
    }

}

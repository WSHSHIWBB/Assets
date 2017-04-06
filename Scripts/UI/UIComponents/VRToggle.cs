using UnityEngine;
using UnityEngine.UI;
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

    private VRToggleGroup _group;
    private Animator _animator;
    private Text _text;

    public void SetIndex(string text)
    {
        _text.text = text;
    }

    private void Awake()
    {
        _group = GetComponentInParent<VRToggleGroup>();
        _animator = GetComponent<Animator>();
        _text = transform.Find("Text").GetComponent<Text>();
        if (_group)
        {
            _group.Add(this);
        }
    }

    void Start()
    {
        UIEventListener.AddUIListener(gameObject).SetEventHandler(EnumUIinputType.OnEnter, new UIEventHandler(OnEnter), null);
        UIEventListener.AddUIListener(gameObject).SetEventHandler(EnumUIinputType.OnExit, new UIEventHandler(OnExit), null);
        UIEventListener.AddUIListener(gameObject).SetEventHandler(EnumUIinputType.OnDown, new UIEventHandler(OnDown), null);
    }

    public void OnNormal()
    {
        _animator.SetTrigger("Normal");
    }

    public void OnHightLighted()
    {
        _animator.SetTrigger("Highlighted");
        //tobe hightlighted select
    }

    public void OnSelected()
    {
        _animator.SetTrigger("Pressed");
        //tobe selected handler
        //tobe sound select
    }

    private void OnExit(GameObject linster, object _arg, object[] _params)
    {
        if (!_isOn)
        {
            OnNormal();
        }
    }

    private void OnEnter(GameObject linster, object _arg, object[] _params)
    {
        if (!_isOn)
        {
            OnHightLighted();
        }
    }

    private void OnDown(GameObject linster, object _arg, object[] _params)
    {
        if (_group)
        {
            _group.SetOn(this);
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

using System.Collections;
using UnityEngine;
using VRFrameWork;
using VRTK;
using DG.Tweening;

public class BagPanel : BaseUI
{
    private BagModule _bagModule;
    private Transform _gridLayoutGroup;
    private Transform _hintLayoutGroup;
    private int _totalPage=0;
    private int _currentPage=0;
    private float _xAxis=0f;
    private float _timeCounter = 0f;
    private Transform _tipsTrans;

    private VRTK_ControllerEvents _leftControllerEvents;
    private VRTK_ControllerEvents _rightControllerEvents;


    public override EnumUIType GetUIType()
    {
        return EnumUIType.Play_Bag;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        _bagModule = ModuleManager.Instance.Get<BagModule>();
        int count = _bagModule.GetBagSize();
        _totalPage = count/9+1;
        _gridLayoutGroup = transform.Find("Mask/GridLayoutGroup");
        _hintLayoutGroup = transform.Find("Hint");
        _tipsTrans = transform.Find("Tips");
        _leftControllerEvents = VRTK_SDK_Bridge.GetControllerLeftHand(false).GetComponent<VRTK_ControllerEvents>();
        _rightControllerEvents = VRTK_SDK_Bridge.GetControllerRightHand(false).GetComponent<VRTK_ControllerEvents>();
    }

    protected override void Enabled()
    {
        _leftControllerEvents.TouchpadAxisChanged += new ControllerInteractionEventHandler(TouchPadAxisChangedHandler);
        _rightControllerEvents.TouchpadAxisChanged += new ControllerInteractionEventHandler(TouchPadAxisChangedHandler);
        _leftControllerEvents.TouchpadTouchEnd += new ControllerInteractionEventHandler(TouchPadTouchEndHandler);
        _rightControllerEvents.TouchpadTouchEnd += new ControllerInteractionEventHandler(TouchPadTouchEndHandler);
    }

    protected override void Disable()
    {
        _leftControllerEvents.TouchpadAxisChanged -= new ControllerInteractionEventHandler(TouchPadAxisChangedHandler);
       _rightControllerEvents.TouchpadAxisChanged -= new ControllerInteractionEventHandler(TouchPadAxisChangedHandler);
        _leftControllerEvents.TouchpadTouchEnd -= new ControllerInteractionEventHandler(TouchPadTouchEndHandler);
        _rightControllerEvents.TouchpadTouchEnd -= new ControllerInteractionEventHandler(TouchPadTouchEndHandler);
    }

    private void TouchPadAxisChangedHandler(object sender, ControllerInteractionEventArgs e)
    {
        _xAxis = e.touchpadAxis.x;
    }

    private void TouchPadTouchEndHandler(object sender, ControllerInteractionEventArgs e)
    {
        _xAxis = 0;
        _timeCounter = 0;
    }

    protected override void OnUpdate(float deltaTime)
    {
        if(_xAxis!=0)
        {
            if (_timeCounter == 0)
            {
                MoveOnePage(_xAxis);
            }
            else
            {
                if (_timeCounter > 0.5f)
                {
                    MoveOnePage(_xAxis);
                    _timeCounter = 0;
                }
            }
            _timeCounter += deltaTime;
        }
    }

    private void MoveOnePage(float xAxis)
    {
        if(xAxis==0)
        {
            Debug.LogError("The xAxis is 0!!");
            return;
        }
        else if(xAxis>0)
        {
            if (_currentPage < _totalPage - 1)
                ++_currentPage;
        }
        else if(xAxis<0)
        {
            if (_currentPage > 0)
                  --_currentPage;
        }
        _gridLayoutGroup.localPosition = new Vector3(((float)(_totalPage-1)/2)*51-_currentPage* 51, 0, 0);
        for(int i=0;i< _totalPage*9;++i)
        {
            _gridLayoutGroup.GetChild(i).GetComponent<GridItem>().OnScrollMove(_currentPage);
        }
    }

    private void IntilalGridItem(int toutalPage)
    {
        RectTransform rectTrans = _gridLayoutGroup.GetComponent<RectTransform>();
        rectTrans.sizeDelta = new Vector2(toutalPage * 51, 51);
        rectTrans.localPosition = new Vector3((rectTrans.rect.width - 51) / 2, 0, 0);
        for (int i=0;i< toutalPage*9; ++i)
        {
            GameObject prefab = Instantiate(Resources.Load<GameObject>("UI/GridItem"),_gridLayoutGroup,false);
            GridItem  gridItem= prefab.GetComponent<GridItem>();
            gridItem.Index = i;
            gridItem.TipsTrans = _tipsTrans;
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        IntilalGridItem(_totalPage);
    }

    protected override void OnPlayOpenUIAnimaton()
    {
        transform.DOScale(0.01f, 0.3f);
    }

    protected override void OnPlayCloseUIAnimation()
    {
        transform.localScale = Vector3.zero;
    }



}

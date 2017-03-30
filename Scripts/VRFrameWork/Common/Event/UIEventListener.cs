using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

namespace VRFrameWork
{

    public delegate void UIEventHandler(GameObject linster, object _arg, params object[] _params);
    public enum EnumUIinputType
    {
        OnClick,
        OnDown,
        OnUp,
        OnEnter,
        OnExit,
        OnSelect,
        OnUpdateSelect,
        OnDeselect,
        OnDrag,
        OnEndDrag,
        OnDrop,
        OnScroll,
        OnMove,
    }


    /// <summary>
    /// This Class is a capsulation for UIEventHandler,The "_arg" is for implements of interface,The "_params" is for
    /// User to transfer parameter,Set "null" if don't need
    /// </summary>
    public class UIHandler
    {
        private UIEventHandler eventHandler = null;
        private object[] handlerparams;

        public void CallHandler(GameObject linster, object _arg)
        {
            if (null != eventHandler)
            {
                eventHandler(linster, _arg, handlerparams);
            }
        }

        public UIHandler()
        {
        }

        public UIHandler(UIEventHandler _handler, params object[] _params)
        {
            SetUIHandler(_handler, _params);
        }

        /// <summary>
        /// You can set a handler for the implements of the UnityEngile.System and transfer some params
        /// </summary>
        /// <param name="_handler"></param>
        /// <param name="_params"></param>
        public void SetUIHandler(UIEventHandler _handler, params object[] _params)
        {
            ClearHandler();
            this.eventHandler += _handler;
            this.handlerparams = _params;
        }

        public void ClearHandler()
        {
            if (null != eventHandler)
            {
                eventHandler -= eventHandler;
                eventHandler = null;
            }
        }
    }

    public class UIEventListener :
    MonoBehaviour,
    IPointerClickHandler,                            //Called when a pointer is pressed and released on the same object
    IPointerDownHandler,                          //Called when a pointer is pressed on the object
    IPointerUpHandler,                               // Called when a pointer is pressed on the object
    IPointerEnterHandler,                          //Called when a pointer enters the object
    IPointerExitHandler,                             //Called when a pointer exits the object
    ISelectHandler,                                      //Called when the object becomes the selected object
    IUpdateSelectedHandler,                   //Called on the selected object each tick
    IDeselectHandler,                                 //Called on the selected object becomes deselected
    IDragHandler,                                        //Called on the selected object becomes deselected
    IEndDragHandler,                                 //Called on the drag object when a drag finishes
    IDropHandler,                                       //Called on the drag object when a drag finishes
    IScrollHandler,                                      //Called when a mouse wheel scrolls
    IMoveHandler                                       //Called when a move event occurs (left, right, up, down, ect)
    {
        public UIHandler onClick;
        public UIHandler onDown;
        public UIHandler onUp;
        public UIHandler onEnter;
        public UIHandler onExit;
        public UIHandler onSelect;
        public UIHandler onUpdateSelect;
        public UIHandler onDeSelect;
        public UIHandler onDrag;
        public UIHandler onEndDrag;
        public UIHandler onDrop;
        public UIHandler onScroll;
        public UIHandler onMove;

        /// <summary>
        /// Add and return  a UIEventListener Component to a UI
        /// </summary>
        /// <param name="UItoAdd"></param>
        /// <returns></returns>
        public static UIEventListener AddUIListener(GameObject UItoAdd)
        {
            return UItoAdd.GetOrAddComponent<UIEventListener>();
        }

        /// <summary>
        /// Set a event handler to the callback of a  specific  interface implemention
        /// </summary>
        /// <param name="_inputType"></param>
        /// <param name="_handler"></param>
        /// <param name="_params"></param>
        public void SetEventHandler(EnumUIinputType _inputType, UIEventHandler _handler, params object[] _params)
        {
            switch (_inputType)
            {
                case EnumUIinputType.OnClick:
                    if (null == onClick)
                    {
                        onClick = new UIHandler();
                        onClick.SetUIHandler(_handler, _params);
                    }
                    break;
                case EnumUIinputType.OnDown:
                    if (null == onDown)
                    {
                        onDown = new UIHandler();
                        onDown.SetUIHandler(_handler, _params);
                    }
                    break;
                case EnumUIinputType.OnUp:
                    if (null == onUp)
                    {
                        onUp = new UIHandler();
                        onUp.SetUIHandler(_handler, _params);
                    }
                    break;
                case EnumUIinputType.OnEnter:
                    if (null == onEnter)
                    {
                        onEnter = new UIHandler();
                        onEnter.SetUIHandler(_handler, _params);
                    }
                    break;
                case EnumUIinputType.OnExit:
                    if(null==onExit)
                    {
                        onExit = new UIHandler();
                        onExit.SetUIHandler(_handler, _params);
                    }
                    break;
                case EnumUIinputType.OnSelect:
                    if (null == onSelect)
                    {
                        onSelect = new UIHandler();
                        onSelect.SetUIHandler(_handler, _params);
                    }
                    break;
                case EnumUIinputType.OnUpdateSelect:
                    if (null == onUpdateSelect)
                    {
                        onUpdateSelect = new UIHandler();
                        onUpdateSelect.SetUIHandler(_handler, _params);
                    }
                    break;
                case EnumUIinputType.OnDeselect:
                    if (null == onDeSelect)
                    {
                        onDeSelect = new UIHandler();
                        onDeSelect.SetUIHandler(_handler, _params);
                    }
                    break;
                case EnumUIinputType.OnDrag:
                    if (null == onDrag)
                    {
                        onDrag = new UIHandler();
                        onDrag.SetUIHandler(_handler, _params);
                    }
                    break;
                case EnumUIinputType.OnEndDrag:
                    if (null == onEndDrag)
                    {
                        onEndDrag = new UIHandler();
                        onEndDrag.SetUIHandler(_handler, _params);
                    }
                    break;
                case EnumUIinputType.OnDrop:
                    if (null == onDrop)
                    {
                        onDrop = new UIHandler();
                        onDrop.SetUIHandler(_handler, _params);
                    }
                    break;
                case EnumUIinputType.OnScroll:
                    if (null == onScroll)
                    {
                        onScroll = new UIHandler();
                        onScroll.SetUIHandler(_handler, _params);
                    }
                    break;
                case EnumUIinputType.OnMove:
                    if (null == onMove)
                    {
                        onMove = new UIHandler();
                        onMove.SetUIHandler(_handler, _params);
                    }
                    break;

            }
        }

        public void OnDestroy()
        {
            RemoveAllHandler();
        }

        private void RemoveAllHandler()
        {
            RemoveHandler(onClick);
            RemoveHandler(onDown);
            RemoveHandler(onUp);
            RemoveHandler(onEnter);
            RemoveHandler(onExit);
            RemoveHandler(onSelect);
            RemoveHandler(onUpdateSelect);
            RemoveHandler(onDeSelect);
            RemoveHandler(onDrag);
            RemoveHandler(onEndDrag);
            RemoveHandler(onDrop);
            RemoveHandler(onScroll);
            RemoveHandler(onMove);
        }

        private void RemoveHandler(UIHandler _handler)
        {
            if (null != _handler)
            {
                _handler.ClearHandler();
                _handler = null;
            }
        }

        #region The implementions for Unityengine.EventSystem's Interface
        public void OnPointerClick(PointerEventData eventData)
        {
            if (null != onClick)
                onClick.CallHandler(this.gameObject, eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (null != onDown)
                onDown.CallHandler(this.gameObject, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (null != onUp)
                onUp.CallHandler(this.gameObject, eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (null != onEnter)
                onEnter.CallHandler(this.gameObject, eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (null != onExit)
                onExit.CallHandler(this.gameObject, eventData);
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (null != onSelect)
                onSelect.CallHandler(this.gameObject, eventData);
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (null != onUpdateSelect)
                onUpdateSelect.CallHandler(this.gameObject, eventData);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (null != onSelect)
                onSelect.CallHandler(this.gameObject, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (null != onDrag)
                onDrag.CallHandler(this.gameObject, eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (null != onDrop)
                onDrop.CallHandler(this.gameObject, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (null != onEndDrag)
                onEndDrag.CallHandler(this.gameObject, eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (null != onScroll)
                onScroll.CallHandler(this.gameObject, eventData);
        }

        public void OnMove(AxisEventData eventData)
        {
            if (null != onMove)
                onMove.CallHandler(this.gameObject, eventData);
        }
        #endregion

    }

}

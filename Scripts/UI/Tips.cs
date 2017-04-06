using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRFrameWork;
using UnityEngine.UI;

public class Tips : MonoBehaviour
{
    [HideInInspector]
    public Transform currentShow;

    private long frameCount = 0;
    private Vector3 _lineEndPos;
    private LineRenderer _lineRender;
    private Text _text;

    private void Start()
    {
        _lineRender = transform.GetComponentByPath<LineRenderer>("Line");
        _text = transform.GetComponentByPath<Text>("UITextFront");
        gameObject.SetActive(false);
    }

    public void SetTips(Vector3 endPos,string text,int index)
    {
        float localX;
        float localY;
        float localZ;
        if(index<0||index>8)
        {
            Debug.Log("The index is illegal!!");
            return;
        }
        _lineEndPos = endPos;
        _text.text = text;
        switch(index)
        {
            case 0: localX = -35f; localY = 18.5f; localZ=-5f; break;
            case 1: localX = -35f; localY = 0f; localZ = -5f; break;
            case 2: localX = -35f; localY = -18.5f; localZ = -5f; break;
            case 3: localX = 12f; localY = 25.5f; localZ = -8f; break;
            case 4: localX = -13f; localY = 6.5f; localZ = -8f; break;
            case 5: localX = -13f; localY = -6f; localZ = -8f; break;
            case 6: localX = 29f; localY = 24f; localZ = -5f; break;
            case 7: localX = 32f; localY = 0f; localZ = -5f; break;
            case 8: localX = 30.5f; localY = -7f; localZ = -5f; break;
            default:return;
        }
        transform.localPosition = new Vector3(localX, localY, localZ);
    }

    public void RefreshFrame(long newFrame)
    {
        frameCount = newFrame;
    }

    public bool Judge3Frame(long currentFrame)
    {
        if(currentFrame-frameCount<3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Judge1Frame(long currentFrame)
    {
        if (currentFrame - frameCount < 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        DrawLine();
        AutoHide();
    }

    private void DrawLine()
    {
        _lineRender.SetPosition(0, transform.position);
        _lineRender.SetPosition(1, _lineEndPos);
    }

    private void AutoHide()
    {
        if (!Judge3Frame(Time.frameCount))
        {
            gameObject.SetActive(false);
        }
    }
}

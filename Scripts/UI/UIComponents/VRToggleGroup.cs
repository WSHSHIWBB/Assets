using System.Collections.Generic;
using UnityEngine;

public class VRToggleGroup : MonoBehaviour
{
    private List<VRToggle> _toggleList = new List<VRToggle>();

    public void Add(VRToggle toggle)
    {
        _toggleList.Add(toggle);
    }
	
	public void SetOn(VRToggle toggle)
    {
        if(toggle.IsOn)
        {
            return;
        }
        for(int i=0;i<_toggleList.Count;++i)
        {
            if (_toggleList[i] != toggle)
            {
                _toggleList[i].IsOn = false;
                _toggleList[i].OnNormal();
            }
        }
        toggle.IsOn = true;
        toggle.OnSelected();
    }

    public void SetOn(int index)
    {
        if(index<0||index>_toggleList.Count-1)
        {
            Debug.LogError("The index is illegal!");
            return;
        }
        SetOn(_toggleList[index]);
        
    }
}

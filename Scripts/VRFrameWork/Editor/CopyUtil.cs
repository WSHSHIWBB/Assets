using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class CopyUtil : EditorWindow
{
    static Component[] Clipboard;
	
    [MenuItem("GameObject/Copy ALLCurrent Components #&C", false, 15)]
    static void Copy()
    {
        Clipboard = Selection.activeGameObject.GetComponents<Component>();
    }
	
	[MenuItem("GameObject/Paste ALL Current Components #&P",false,15)]
	static void Paste()
    {
        foreach(var targetGameObject in Selection.gameObjects)
        {
            if (!targetGameObject || Clipboard == null)
                continue;
            foreach(var copiedComponent in Clipboard)
            {
                if (!copiedComponent)
                    continue;
                UnityEditorInternal.ComponentUtility.CopyComponent(copiedComponent);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(targetGameObject);
            }
        }
    }

    [MenuItem("GameObject/Remove All Current Components #&D",false,15)]
    static void Delete()
    {
        /*
        foreach(var targetGameobject in Selection.gameObjects)
        {
            if (!targetGameobject)
                continue;
            Component[] components = targetGameobject.GetComponents<Component>();
            foreach(var component in components)
            {
                DestroyImmediate(component);
            }
        }
        */
    }

}

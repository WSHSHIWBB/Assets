using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// Ignore UI RayCast or not
/// </summary>
public class IsIgnoreRayCast : MonoBehaviour,ICanvasRaycastFilter
{
      public bool NotIgnore = false;
      public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
      {
            return NotIgnore;
      }

}

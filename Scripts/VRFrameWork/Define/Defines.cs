using UnityEngine;
using System.Collections;

namespace VRFrameWork
{

      public enum EnumObjectState
      {
            None,
            Initial,
            Loading,
            Ready,
            Disabled,
            Closing,
      }

      public delegate void ObjectStateChangeEvent(object sender, EnumObjectState newState, EnumObjectState oldState);

      public enum EnumUIType
      {
            None=-1,
            Play_State,
            Play_Equip,
      }



}

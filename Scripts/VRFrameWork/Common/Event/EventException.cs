using System;

/// <summary>
/// The Excepton for VRFrameWork 
/// </summary>

namespace VRFrameWork
{

      [Serializable]
      public class EventException : Exception
      {
            /// <summary>
            /// Init a ErrorMessage to a EventException instance
            /// </summary>
            /// <param name="message"></param>
            public EventException(string message)
            : base(message)
            {
            }

            /// <summary>
            /// Init a ErrorMessage and a reference of InnerException to a EventException instance
            /// </summary>
            /// <param name="message"></param>
            /// <param name="sysException"></param>
            public EventException(string message, Exception sysException)
            : base(message, sysException)
            {
            }
      }

}

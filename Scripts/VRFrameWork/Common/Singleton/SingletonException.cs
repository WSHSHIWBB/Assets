using System;

public class SingletonException : Exception
{
      /// <summary>
      /// Initializes a new instance of the <see cref="SingletonException"/> class.
      /// </summary>
      /// <param name="message"></param>
      public SingletonException(string message)
      : base(message)
      {

      }
	
}

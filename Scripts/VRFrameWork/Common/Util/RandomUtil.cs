using UnityEngine;

namespace VRFrameWork
{

    public class RandomUtil
    {

        #region Random.Range of UnityEngine.Random

        public static float Range(float min, float max)
        {
            return Random.Range(min, max);
        }

        public static int Range(int min, int max)
        {
            return Random.Range(min, max);
        }

        #endregion



        #region Return a Random value of a Array

        public static float Array(float[] array)
        {
            if (array == null || array.Length == 0)
            {
                Debug.LogError("The Array is null");
                return default(float);
            }
            else
            {
                int i = Random.Range(0, array.Length);
                return array[i];
            }
        }

        public static float Array(double[] array)
        {
            if(array==null||array.Length==0)
            {
                Debug.LogError("Tha Array is null or empty!");
                return default(float);
            }
            else
            {
                int i = Random.Range(0, array.Length);
                return (float)array[i];
            }
        }

        public static int Array(int[] array)
        {
            if(array==null||array.Length==0)
            {
                Debug.LogError("Tha Array is null or empty!");
                return default(int);
            }
            else
            {
                int i = Random.Range(0, array.Length);
                return array[i];
            }
        }

        public static T Array<T>(T[] array)
        {
            if(array==null||array.Length==0)
            {
                Debug.LogError("Tha Array is null or empty!");
                return default(T);
            }
            else
            {
                int i = Random.Range(0, array.Length);
                return array[i];
            }
        }

        #endregion

        public static bool Bool()
        {
            int i = Random.Range(0, 2);
            if(i==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public static class Extensions
//{
//    //transform.Find(); will only search the immediate children
//    public static Transform Search(this Transform target, string name)
//    {
//        if (target.name == name) return target;

//        for (int i = 0; i < target.childCount; ++i)
//        {
//            var result = Search(target.GetChild(i), name);
//            if (result != null) return result;
//        }

//        return null;
//    }
//}

//public class helper_extented_unity_objects : MonoBehaviour
//{
//}

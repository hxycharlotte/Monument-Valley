using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class TransformHelper
{
    public static Transform FindChildByName(this Transform currentTF, string childName)
    {
        foreach (Transform childTF in currentTF)
        {
            if (childTF.name == childName)
                return childTF;

            Transform tf = childTF.FindChildByName(childName);

            if (tf != null)
                return tf;
        }

        return null;
    }
}

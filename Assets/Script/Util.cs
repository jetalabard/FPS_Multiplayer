using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static void SetLayerRecursively(GameObject obj, int nameLayer)
    {
        if (obj == null)
        {
            return;
        }

        obj.layer = nameLayer;

        foreach (Transform child in obj.transform)
        {
            if (child != null)
            {
                SetLayerRecursively(child.gameObject, nameLayer);
            }
        }
    }

}

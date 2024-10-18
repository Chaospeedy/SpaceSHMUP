using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Utils : MonoBehaviour
{
    static public Vector3 Bezier( float u, params Vector3[] points){
        Vector3[,] vArr = new Vector3[points.Length, points.Length];

        int r = points.Length - 1;
        for( int c = 0; c < points.Length; c++){
            vArr[r,c] = vList[c];
        }
    }
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Math : MonoBehaviour {

    /*
        Name: Math
        Type: Global Script
        Purpose: Contains custom math functions that cannot be found in Mathf
    */
	// Use this for initialization

    public static Vector2 rayIntersection(Vector3 origin1, Vector3 slope1, Vector3 origin2, Vector3 slope2)
    {
        Vector3 slope3 = origin2 - origin1;
        Vector3 cross1_2 = Vector3.Cross(slope1, slope2);
        Vector3 cross3_2 = Vector3.Cross(slope3, slope2);
        Vector3 result;
        float planar = Vector3.Dot(slope3, cross3_2);

        if (Mathf.Abs(planar) < 0.0001f && cross1_2.sqrMagnitude > 0.0001f)
        {
            result = (origin1 + (slope1 * (Vector3.Dot(cross3_2, cross1_2) / cross1_2.sqrMagnitude)));
            return new Vector2(result.x, result.y);
        }
        else
        {
            return Vector2.zero;
        }
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

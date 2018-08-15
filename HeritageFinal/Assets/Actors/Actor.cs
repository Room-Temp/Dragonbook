using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

	// Use this for initialization
	protected virtual void Start () {
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(gameObject.GetComponent<Transform>().position.y * 1000);

    }
}

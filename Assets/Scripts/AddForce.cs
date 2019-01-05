using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour {
	public Vector3 force;
	// Use this for initialization
	void Start () {
		Rigidbody rb = this.GetComponent<Rigidbody> ();
		rb.AddForce (force, ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

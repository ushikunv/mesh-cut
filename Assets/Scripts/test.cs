using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
	public GameObject cutter;
	public GameObject point;
	public GameObject ob1;
	public GameObject ob2;
	private Plane cutPlane;
	private Vector3 p,c1;
	// Use this for initialization
	void Start () {
		p = ob1.transform.position;
		c1 = ob2.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		cutPlane = new Plane (cutter.transform.forward, cutter.transform.position);
		point.transform.position =  (p
			+ (-cutPlane.distance - Vector3.Dot (cutPlane.normal, p))
			*(c1-p) / Vector3.Dot (cutPlane.normal,c1-p));
	}
}

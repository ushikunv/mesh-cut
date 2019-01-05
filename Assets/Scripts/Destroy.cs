using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {
	public GameObject ball;

	void Start(){
		
	}


	void OnTriggerEnter(Collider other)
	{
		Destroy (other.gameObject);
		Instantiate (ball, ball.transform.position, ball.transform.rotation );
		//Instantiate (ball, new Vecter3(0,100,100), new Quaternion(0,0,0,1) );
	}
}

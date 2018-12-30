using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour {
	public GameObject rb;
	public GameObject cm;
	public float moveSpeed;
	private Vector2 touchVec;
	private int objNum = 0;
	public List<GameObject> objs = new List<GameObject> ();
	private bool reBool = true;
	// Use this for initialization
	void Start () {
		Instantiate (objs [objNum]);
	}

	void Update(){
		touchVec = OVRInput.Get (OVRInput.Axis2D.PrimaryTouchpad);

		if (OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger)) {
			if (reBool) {
				GameObject[] desObj = GameObject.FindGameObjectsWithTag ("obj");
				foreach (GameObject obj in desObj) {
					Destroy (obj);
				}
				Instantiate (objs [objNum%objs.Count]);
				reBool = false;
				Invoke ("boolOn",0.5f);
			}
			//	rb.transform.Translate (new Vector3 (0f, -moveSpeed, 0f));
//		} else if (OVRInput.Get (OVRInput.Button.PrimaryTouchpad) && touchVec.x > 0.7) {
//			rb.transform.Translate (new Vector3 (Mathf.Cos (cm.transform.localEulerAngles.y / 180 * Mathf.PI), 0f, -Mathf.Sin (cm.transform.localEulerAngles.y / 180 * Mathf.PI)) * moveSpeed);
//		} else if (OVRInput.Get (OVRInput.Button.PrimaryTouchpad) && touchVec.x < -0.7) {
//			rb.transform.Translate (new Vector3 (-Mathf.Cos (cm.transform.localEulerAngles.y / 180 * Mathf.PI), 0f, Mathf.Sin (cm.transform.localEulerAngles.y / 180 * Mathf.PI)) * moveSpeed);
//		} else if (OVRInput.Get (OVRInput.Button.PrimaryTouchpad) && touchVec.y > 0.3) {
//			rb.transform.Translate (new Vector3 (Mathf.Sin (cm.transform.localEulerAngles.y / 180 * Mathf.PI), 0f, Mathf.Cos (cm.transform.localEulerAngles.y / 180 * Mathf.PI)) * moveSpeed);
//		} else if (OVRInput.Get (OVRInput.Button.PrimaryTouchpad) && touchVec.y < -0.7) {
//			rb.transform.Translate (new Vector3 (-Mathf.Sin (cm.transform.localEulerAngles.y / 180 * Mathf.PI), 0f, -Mathf.Cos (cm.transform.localEulerAngles.y / 180 * Mathf.PI)) * moveSpeed);
		} else if (OVRInput.Get (OVRInput.Button.PrimaryTouchpad)) {
			if (reBool) {
				GameObject[] desObj = GameObject.FindGameObjectsWithTag ("obj");
				foreach (GameObject obj in desObj) {
					Destroy (obj);
				}
				objNum++;
				Instantiate (objs [objNum%objs.Count]);
				reBool = false;
				Invoke ("boolOn",0.5f);
			}
			//col.isTrigger = true;
			//		rb.transform.Translate (new Vector3 (0f, moveSpeed, 0f));
		} else {

		}
	}

	void boolOn(){
		reBool = true;
	}



}

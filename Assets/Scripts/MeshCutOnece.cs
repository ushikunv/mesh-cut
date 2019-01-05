using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCutOnece: MonoBehaviour
{

	private GameObject cutter;
	private GameController gameController;
	private Plane cutPlane;
	private MeshFilter attachedMeshFilter;
	private Mesh attachedMesh;
	private bool coliBool = false;
	public float delta = 0.00001f;
	public float skinWidth = 0.005f;

	float time0;

	void Start ()
	{
		//

		Invoke ("BoolOn", 0.8f);
	}

	void BoolOn ()
	{
		cutter = GameObject.Find ("cutter");
		attachedMeshFilter = GetComponent<MeshFilter> ();
		attachedMesh = attachedMeshFilter.mesh;

		coliBool = true;
	}

	void OnTriggerEnter (Collider other)
	{
		if (coliBool) {
			cutPlane = new Plane (cutter.transform.forward, cutter.transform.position);
			Cut ();
		}

	}


	public void Cut ()
	{

		Vector3 p1, p2, p3;
		bool p1Bool, p2Bool, p3Bool;
		var uv1 = new List<Vector2> ();
		var uv2 = new List<Vector2> ();
		var vertices1 = new List<Vector3> ();
		var vertices2 = new List<Vector3> ();
		var triangles1 = new List<int> ();
		var triangles2 = new List<int> ();
		var normals1 = new List<Vector3> ();
		var normals2 = new List<Vector3> ();
		var crossVertices = new List<Vector3> ();

		for (int i = 0; i < attachedMesh.triangles.Length; i += 3) {
			p1 = transform.TransformPoint (attachedMesh.vertices [attachedMesh.triangles [i]]);
			p2 = transform.TransformPoint (attachedMesh.vertices [attachedMesh.triangles [i + 1]]);
			p3 = transform.TransformPoint (attachedMesh.vertices [attachedMesh.triangles [i + 2]]);

			p1Bool = cutPlane.GetSide (p1);
			p2Bool = cutPlane.GetSide (p2);
			p3Bool = cutPlane.GetSide (p3);


			if (p1Bool && p2Bool && p3Bool) {

				for (int k = 0; k < 3; k++) {


					vertices1.Add (attachedMesh.vertices [attachedMesh.triangles [i + k]]);
					uv1.Add (attachedMesh.uv [attachedMesh.triangles [i + k]]);
					normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + k]]);
					triangles1.Add (vertices1.Count - 1);


				}


			} else if (!p1Bool && !p2Bool && !p3Bool) {

				for (int k = 0; k < 3; k++) {

					vertices2.Add (attachedMesh.vertices [attachedMesh.triangles [i + k]]);
					uv2.Add (attachedMesh.uv [attachedMesh.triangles [i + k]]);
					normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + k]]);
					triangles2.Add (vertices2.Count - 1);

				}
			} else if ((p1Bool && !p2Bool && !p3Bool) || (!p1Bool && p2Bool && !p3Bool) || (!p1Bool && !p2Bool && p3Bool)) {
				Vector3 p, c1, c2;
				int n1, n2, n3;
				if (p1Bool) {
					p = p1;
					c1 = p2;
					c2 = p3;
					n1 = 0;
					n2 = 1;
					n3 = 2;

				} else if (p2Bool) {
					p = p2;
					c1 = p3;
					c2 = p1;
					n1 = 1;
					n2 = 2;
					n3 = 0;

				} else {
					p = p3;
					c1 = p1;
					c2 = p2;
					n1 = 2;
					n2 = 0;
					n3 = 1;

				}

				Vector3 cross1 = transform.InverseTransformPoint (p
					+ (-cutPlane.distance - Vector3.Dot (cutPlane.normal, p))
					* (c1 - p) / Vector3.Dot (cutPlane.normal, c1 - p));
				Vector3 cross2 = transform.InverseTransformPoint (p
					+ (-cutPlane.distance - Vector3.Dot (cutPlane.normal, p))
					* (c2 - p) / Vector3.Dot (cutPlane.normal, c2 - p));

				crossVertices.Add (cross1);
				crossVertices.Add (cross2);
				Vector2 cross1Uv = Vector2.Lerp (attachedMesh.uv [attachedMesh.triangles [i + n2]], attachedMesh.uv [attachedMesh.triangles [i + n1]], (transform.TransformPoint (cross1) - c1).magnitude / (p - c1).magnitude);
				Vector2 cross2Uv = Vector2.Lerp (attachedMesh.uv [attachedMesh.triangles [i + n3]], attachedMesh.uv [attachedMesh.triangles [i + n1]], (transform.TransformPoint (cross2) - c2).magnitude / (p - c2).magnitude);

				vertices1.Add (cross1);
				uv1.Add (cross1Uv);
				normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
				triangles1.Add (vertices1.Count - 1);

				vertices1.Add (cross2);
				uv1.Add (cross2Uv);
				normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
				triangles1.Add (vertices1.Count - 1);

				vertices1.Add (attachedMesh.vertices [attachedMesh.triangles [i + n1]]);
				uv1.Add (attachedMesh.uv [attachedMesh.triangles [i + n1]]);
				normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
				triangles1.Add (vertices1.Count - 1);



				vertices2.Add (cross2);
				uv2.Add (cross2Uv);
				normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
				triangles2.Add (vertices2.Count - 1);

				vertices2.Add (attachedMesh.vertices [attachedMesh.triangles [i + n2]]);
				uv2.Add (attachedMesh.uv [attachedMesh.triangles [i + n2]]);
				normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n2]]);
				triangles2.Add (vertices2.Count - 1);

				vertices2.Add (attachedMesh.vertices [attachedMesh.triangles [i + n3]]);
				uv2.Add (attachedMesh.uv [attachedMesh.triangles [i + n3]]);
				normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n3]]);
				triangles2.Add (vertices2.Count - 1);



				vertices2.Add (cross2);
				uv2.Add (cross2Uv);
				normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
				triangles2.Add (vertices2.Count - 1);

				vertices2.Add (cross1);
				triangles2.Add (vertices2.Count - 1);
				uv2.Add (cross1Uv);
				normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);

				vertices2.Add (attachedMesh.vertices [attachedMesh.triangles [i + n2]]);
				uv2.Add (attachedMesh.uv [attachedMesh.triangles [i + n2]]);
				normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n2]]);
				triangles2.Add (vertices2.Count - 1);

			} else if ((!p1Bool && p2Bool && p3Bool) || (p1Bool && !p2Bool && p3Bool) || (p1Bool && p2Bool && !p3Bool)) {
				Vector3 p, c1, c2;
				int n1, n2, n3;
				if (!p1Bool) {
					p = p1;
					c1 = p2;
					c2 = p3;
					n1 = 0;
					n2 = 1;
					n3 = 2;
				} else if (!p2Bool) {
					p = p2;
					c1 = p3;
					c2 = p1;
					n1 = 1;
					n2 = 2;
					n3 = 0;
				} else {
					p = p3;
					c1 = p1;
					c2 = p2;
					n1 = 2;
					n2 = 0;
					n3 = 1;
				}

				Vector3 cross1 = transform.InverseTransformPoint (p
					+ (-cutPlane.distance - Vector3.Dot (cutPlane.normal, p))
					* (c1 - p) / Vector3.Dot (cutPlane.normal, c1 - p));
				Vector3 cross2 = transform.InverseTransformPoint (p
					+ (-cutPlane.distance - Vector3.Dot (cutPlane.normal, p))
					* (c2 - p) / Vector3.Dot (cutPlane.normal, c2 - p));
				crossVertices.Add (cross1);
				crossVertices.Add (cross2);
				Vector2 cross1Uv = Vector2.Lerp (attachedMesh.uv [attachedMesh.triangles [i + n2]], attachedMesh.uv [attachedMesh.triangles [i + n1]], (transform.TransformPoint (cross1) - c1).magnitude / (p - c1).magnitude);
				Vector2 cross2Uv = Vector2.Lerp (attachedMesh.uv [attachedMesh.triangles [i + n3]], attachedMesh.uv [attachedMesh.triangles [i + n1]], (transform.TransformPoint (cross2) - c2).magnitude / (p - c2).magnitude);




				vertices2.Add (cross1);
				triangles2.Add (vertices2.Count - 1);
				uv2.Add (cross1Uv);
				normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);

				vertices2.Add (cross2);
				triangles2.Add (vertices2.Count - 1);
				uv2.Add (cross2Uv);
				normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);

				vertices2.Add (attachedMesh.vertices [attachedMesh.triangles [i + n1]]);
				uv2.Add (attachedMesh.uv [attachedMesh.triangles [i + n1]]);
				normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
				triangles2.Add (vertices2.Count - 1);


				vertices1.Add (cross2);
				triangles1.Add (vertices1.Count - 1);
				uv1.Add (cross2Uv);
				normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);

				vertices1.Add (attachedMesh.vertices [attachedMesh.triangles [i + n2]]);
				uv1.Add (attachedMesh.uv [attachedMesh.triangles [i + n2]]);
				normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n2]]);
				triangles1.Add (vertices1.Count - 1);

				vertices1.Add (attachedMesh.vertices [attachedMesh.triangles [i + n3]]);
				uv1.Add (attachedMesh.uv [attachedMesh.triangles [i + n3]]);
				normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n3]]);
				triangles1.Add (vertices1.Count - 1);



				vertices1.Add (cross2);
				triangles1.Add (vertices1.Count - 1);
				uv1.Add (cross2Uv);
				normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);

				vertices1.Add (cross1);
				triangles1.Add (vertices1.Count - 1);
				uv1.Add (cross1Uv);
				normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);

				vertices1.Add (attachedMesh.vertices [attachedMesh.triangles [i + n2]]);
				uv1.Add (attachedMesh.uv [attachedMesh.triangles [i + n2]]);
				normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n2]]);
				triangles1.Add (vertices1.Count - 1);
			}
		}




		if (crossVertices.Count != 0) {



			Vector3 center = new Vector3 ();
			foreach (var v in crossVertices) {
				center += v;
			}
			center /= crossVertices.Count;
			vertices1.Add (center);
			uv1.Add (new Vector2 (0, 0));
			normals1.Add (-cutPlane.normal);
			vertices2.Add (center);
			uv2.Add (new Vector2 (0, 0));
			normals2.Add (cutPlane.normal);
			int centerNum1 = vertices1.Count - 1;
			int centerNum2 = vertices2.Count - 1;
			for (int i = 0; i < crossVertices.Count; i += 2) {
				if (Vector3.Dot (Vector3.Cross (transform.TransformPoint (crossVertices [i]) - transform.TransformPoint (center), transform.TransformPoint (crossVertices [i + 1]) - transform.TransformPoint (crossVertices [i])), cutPlane.normal) <= 0) {
					triangles1.Add (centerNum1);
					vertices1.Add (crossVertices [i]);
					triangles1.Add (vertices1.Count - 1);
					uv1.Add (new Vector2 (0, 0));
					normals1.Add (-cutPlane.normal);
					vertices1.Add (crossVertices [i + 1]);
					uv1.Add (new Vector2 (0, 0));
					normals1.Add (-cutPlane.normal);
					triangles1.Add (vertices1.Count - 1);

					vertices2.Add (crossVertices [i]);
					triangles2.Add (vertices2.Count - 1);
					uv2.Add (new Vector2 (0, 0));
					normals2.Add (cutPlane.normal);
					triangles2.Add (centerNum2);
					vertices2.Add (crossVertices [i + 1]);
					uv2.Add (new Vector2 (0, 0));
					normals2.Add (cutPlane.normal);
					triangles2.Add (vertices2.Count - 1);
				} else {
					vertices1.Add (crossVertices [i]);
					triangles1.Add (vertices1.Count - 1);
					uv1.Add (new Vector2 (0, 0));
					normals1.Add (-cutPlane.normal);
					triangles1.Add (centerNum1);
					vertices1.Add (crossVertices [i + 1]);
					uv1.Add (new Vector2 (0, 0));
					normals1.Add (-cutPlane.normal);
					triangles1.Add (vertices1.Count - 1);

					triangles2.Add (centerNum2);
					vertices2.Add (crossVertices [i]);
					triangles2.Add (vertices2.Count - 1);
					uv2.Add (new Vector2 (0, 0));
					normals2.Add (cutPlane.normal);
					vertices2.Add (crossVertices [i + 1]);
					uv2.Add (new Vector2 (0, 0));
					normals2.Add (cutPlane.normal);
					triangles2.Add (vertices2.Count - 1);

				}
			}
		}
		//		GameObject obj;
		//
		//		if (vertices1.Count > 400) {
		//			obj = new GameObject ("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody));
		//			Debug.Log ("done");
		//		} else {
		//			obj = new GameObject ("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshCut2));
		//		}

		GameObject obj = new GameObject ("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshCutOnece));
		var mesh = new Mesh ();
		mesh.vertices = vertices1.ToArray ();
		mesh.triangles = triangles1.ToArray ();
		mesh.uv = uv1.ToArray ();
		mesh.normals = normals1.ToArray ();
		obj.GetComponent<MeshFilter> ().mesh = mesh;
		obj.GetComponent<MeshRenderer> ().materials = GetComponent<MeshRenderer> ().materials;
		obj.GetComponent<MeshCollider> ().sharedMesh = mesh;
		obj.GetComponent<MeshCollider> ().inflateMesh = true;
		obj.GetComponent<MeshCollider> ().skinWidth = skinWidth;
		obj.GetComponent<MeshCutOnece>().skinWidth = skinWidth;
		obj.GetComponent<MeshCollider> ().convex = true;
		obj.GetComponent<MeshCollider> ().material = GetComponent<Collider> ().material;
		obj.transform.position = transform.position;
		obj.transform.rotation = transform.rotation;
		obj.transform.localScale = transform.localScale;
		obj.GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody> ().velocity;
		obj.GetComponent<Rigidbody> ().angularVelocity = GetComponent<Rigidbody> ().angularVelocity;

		//		GameObject obj2;
		//		if (vertices2.Count > 400) {
		//			obj2 = new GameObject ("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody));
		//		} else {
		//			 obj2 = new GameObject ("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshCut2));
		//		}
		GameObject obj2 = new GameObject ("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshCutOnece));

		var mesh2 = new Mesh ();
		mesh2.vertices = vertices2.ToArray ();
		mesh2.triangles = triangles2.ToArray ();
		mesh2.uv = uv2.ToArray ();
		mesh2.normals = normals2.ToArray ();
		obj2.GetComponent<MeshFilter> ().mesh = mesh2;
		obj2.GetComponent<MeshRenderer> ().materials = GetComponent<MeshRenderer> ().materials;
		obj2.GetComponent<MeshCollider> ().sharedMesh = mesh2;
		obj2.GetComponent<MeshCollider> ().inflateMesh = true;
		obj2.GetComponent<MeshCollider> ().skinWidth = skinWidth;
		obj2.GetComponent<MeshCutOnece>().skinWidth = skinWidth;
		obj2.GetComponent<MeshCollider> ().convex = true;
		obj2.GetComponent<MeshCollider> ().material = GetComponent<Collider> ().material;
		obj2.transform.position = transform.position;
		obj2.transform.rotation = transform.rotation;
		obj2.transform.localScale = transform.localScale;
		obj2.GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody> ().velocity;
		obj2.GetComponent<Rigidbody> ().angularVelocity = GetComponent<Rigidbody> ().angularVelocity;

		Destroy (gameObject);


	}
	// Update is called once per frame
	void Update ()
	{

	}


	int numRep (int i)
	{
		if (i % 3 == 0) {
			return 0;
		} else if (i % 3 == 1) {
			return 1;
		} else if (i % 3 == 2) {
			return 2;
		} else {
			return 0;
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCut181126 : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


//	using System.Collections;
//	using System.Collections.Generic;
//	using UnityEngine;
//
//	public class MeshCut : MonoBehaviour
//	{
//
//		private MeshFilter attachedMeshFilter;
//		private Mesh attachedMesh;
//		private bool coliBool = false;
//		private float delta = 0.0001f;
//		private float delta2 = 0.001f;
//		public float skinWidth = 0.005f;
//
//		float time0;
//
//		void Start ()
//		{
//			this.tag = "obj";
//			Invoke ("BoolOn", 0.2f);
//		}
//
//		void BoolOn ()
//		{
//			attachedMeshFilter = GetComponent<MeshFilter> ();
//			attachedMesh = attachedMeshFilter.mesh;
//			coliBool = true;
//		}
//
//
//		public void Cut (Plane cutPlane)
//		{
//			if (coliBool == false) {
//				return;
//			}
//
//			Vector3 p1, p2, p3;
//			bool p1Bool, p2Bool, p3Bool;
//			//カットした後の２つのオブジェクトに対応するuv,vertices, triangles, normals
//			var uvs1 = new List<Vector2> ();
//			var uvs2 = new List<Vector2> ();
//			var vertices1 = new List<Vector3> ();
//			var vertices2 = new List<Vector3> ();
//			var triangles1 = new List<int> ();
//			var triangles2 = new List<int> ();
//			var normals1 = new List<Vector3> ();
//			var normals2 = new List<Vector3> ();
//			//処理中に使ういれもの
//			var crossVertices = new List<Vector3> ();
//
//			/*カットしたいオブジェクトのメッシュをトライアングルごとに処理
//		*/
//			for (int i = 0; i < attachedMesh.triangles.Length; i += 3) {
//				//メッシュの3つの頂点を取得
//				p1 = transform.TransformPoint (attachedMesh.vertices [attachedMesh.triangles [i]]);
//				p2 = transform.TransformPoint (attachedMesh.vertices [attachedMesh.triangles [i + 1]]);
//				p3 = transform.TransformPoint (attachedMesh.vertices [attachedMesh.triangles [i + 2]]);
//
//				//頂点がカットする面のどちら側にあるか
//				p1Bool = cutPlane.GetSide (p1);
//				p2Bool = cutPlane.GetSide (p2);
//				p3Bool = cutPlane.GetSide (p3);
//
//				//3つの頂点が同じ側にある場合はそのまま代入、頂点がカットする場合はその処理を行う
//				if (p1Bool && p2Bool && p3Bool) {
//					//3つの頂点が同じ側にある、そのままそれぞれの1に代入
//					for (int k = 0; k < 3; k++) {
//						vertices1.Add (attachedMesh.vertices [attachedMesh.triangles [i + k]]);
//						uvs1.Add (attachedMesh.uv [attachedMesh.triangles [i + k]]);
//						normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + k]]);
//						triangles1.Add (vertices1.Count - 1);
//					}
//
//
//				} else if (!p1Bool && !p2Bool && !p3Bool) {
//					//3つの頂点が同じ側にある、そのままそれぞれの２に代入
//					for (int k = 0; k < 3; k++) {
//						vertices2.Add (attachedMesh.vertices [attachedMesh.triangles [i + k]]);
//						uvs2.Add (attachedMesh.uv [attachedMesh.triangles [i + k]]);
//						normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + k]]);
//						triangles2.Add (vertices2.Count - 1);
//					}
//				} else {
//					//3つの頂点が同じ側にない場合の処理１、以下仲間外れの頂点をp,それ以外をcとする
//					Vector3 p, c1, c2;
//					int n1, n2, n3;
//					if ((p1Bool && !p2Bool && !p3Bool) || (!p1Bool && p2Bool && p3Bool)) {
//						p = p1;
//						c1 = p2;
//						c2 = p3;
//						n1 = 0;
//						n2 = 1;
//						n3 = 2;
//
//					} else if ((!p1Bool && p2Bool && !p3Bool) || (p1Bool && !p2Bool && p3Bool)) {
//						p = p2;
//						c1 = p3;
//						c2 = p1;
//						n1 = 1;
//						n2 = 2;
//						n3 = 0;
//
//					} else {
//						p = p3;
//						c1 = p1;
//						c2 = p2;
//						n1 = 2;
//						n2 = 0;
//						n3 = 1;
//
//					} 
//
//					//カットした面に生じる新しい頂点を計算、カットする平面の法線方向に対するpとcの距離の比からc-pの長さを決める
//					Vector3 cross1 = transform.InverseTransformPoint (p　 + (c1 - p) * (cutPlane.distance + Vector3.Dot (cutPlane.normal, p)) / Vector3.Dot (cutPlane.normal, p - c1));
//					Vector3 cross2 = transform.InverseTransformPoint (p　 + (c2 - p) * (cutPlane.distance + Vector3.Dot (cutPlane.normal, p)) / Vector3.Dot (cutPlane.normal, p - c2));
//
//
//					//断面をつくるために取っておく
//					crossVertices.Add (cross1);
//					crossVertices.Add (cross2);
//
//					//新しい頂点のuvを計算、pとcの間で線形補間
//					Vector2 cross1Uv = Vector2.Lerp (attachedMesh.uv [attachedMesh.triangles [i + n1]], attachedMesh.uv [attachedMesh.triangles [i + n2]], (transform.TransformPoint (cross1) - p).magnitude / (p - c1).magnitude);
//					Vector2 cross2Uv = Vector2.Lerp (attachedMesh.uv [attachedMesh.triangles [i + n1]], attachedMesh.uv [attachedMesh.triangles [i + n3]], (transform.TransformPoint (cross2) - p).magnitude / (p - c2).magnitude);
//
//					//pの２通りの処理、カットする面に対してどちらにあるかで異なる
//					if ((p1Bool && !p2Bool && !p3Bool) || (!p1Bool && p2Bool && !p3Bool) || (!p1Bool && !p2Bool && p3Bool)) {
//						//p側のメッシュを追加
//						vertices1.Add (cross1);
//						uvs1.Add (cross1Uv);
//						normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
//						triangles1.Add (vertices1.Count - 1);
//
//						vertices1.Add (cross2);
//						uvs1.Add (cross2Uv);
//						normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
//						triangles1.Add (vertices1.Count - 1);
//
//						vertices1.Add (attachedMesh.vertices [attachedMesh.triangles [i + n1]]);
//						uvs1.Add (attachedMesh.uv [attachedMesh.triangles [i + n1]]);
//						normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
//						triangles1.Add (vertices1.Count - 1);
//
//						//c側のメッシュを追加１
//						vertices2.Add (cross2);
//						uvs2.Add (cross2Uv);
//						normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
//						triangles2.Add (vertices2.Count - 1);
//
//						vertices2.Add (attachedMesh.vertices [attachedMesh.triangles [i + n2]]);
//						uvs2.Add (attachedMesh.uv [attachedMesh.triangles [i + n2]]);
//						normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n2]]);
//						triangles2.Add (vertices2.Count - 1);
//
//						vertices2.Add (attachedMesh.vertices [attachedMesh.triangles [i + n3]]);
//						uvs2.Add (attachedMesh.uv [attachedMesh.triangles [i + n3]]);
//						normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n3]]);
//						triangles2.Add (vertices2.Count - 1);
//
//						//c側のメッシュを追加2
//						vertices2.Add (cross2);
//						uvs2.Add (cross2Uv);
//						normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
//						triangles2.Add (vertices2.Count - 1);
//
//						vertices2.Add (cross1);
//						triangles2.Add (vertices2.Count - 1);
//						uvs2.Add (cross1Uv);
//						normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
//
//						vertices2.Add (attachedMesh.vertices [attachedMesh.triangles [i + n2]]);
//						uvs2.Add (attachedMesh.uv [attachedMesh.triangles [i + n2]]);
//						normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n2]]);
//						triangles2.Add (vertices2.Count - 1);
//					} else {
//						//p側のメッシュを追加
//						vertices2.Add (cross1);
//						triangles2.Add (vertices2.Count - 1);
//						uvs2.Add (cross1Uv);
//						normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
//
//						vertices2.Add (cross2);
//						triangles2.Add (vertices2.Count - 1);
//						uvs2.Add (cross2Uv);
//						normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
//
//						vertices2.Add (attachedMesh.vertices [attachedMesh.triangles [i + n1]]);
//						uvs2.Add (attachedMesh.uv [attachedMesh.triangles [i + n1]]);
//						normals2.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
//						triangles2.Add (vertices2.Count - 1);
//
//						//c側のメッシュを追加１
//						vertices1.Add (cross2);
//						triangles1.Add (vertices1.Count - 1);
//						uvs1.Add (cross2Uv);
//						normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
//
//						vertices1.Add (attachedMesh.vertices [attachedMesh.triangles [i + n2]]);
//						uvs1.Add (attachedMesh.uv [attachedMesh.triangles [i + n2]]);
//						normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n2]]);
//						triangles1.Add (vertices1.Count - 1);
//
//						vertices1.Add (attachedMesh.vertices [attachedMesh.triangles [i + n3]]);
//						uvs1.Add (attachedMesh.uv [attachedMesh.triangles [i + n3]]);
//						normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n3]]);
//						triangles1.Add (vertices1.Count - 1);
//
//						//c側のメッシュを追加2
//						vertices1.Add (cross2);
//						triangles1.Add (vertices1.Count - 1);
//						uvs1.Add (cross2Uv);
//						normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
//
//						vertices1.Add (cross1);
//						triangles1.Add (vertices1.Count - 1);
//						uvs1.Add (cross1Uv);
//						normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n1]]);
//
//						vertices1.Add (attachedMesh.vertices [attachedMesh.triangles [i + n2]]);
//						uvs1.Add (attachedMesh.uv [attachedMesh.triangles [i + n2]]);
//						normals1.Add (attachedMesh.normals [attachedMesh.triangles [i + n2]]);
//						triangles1.Add (vertices1.Count - 1);
//					}
//				}
//			}
//
//
//			var verticeIndices = new List<int> ();
//			var pVertices = new List<Vector3> ();
//			var pNormals = new List<Vector3> ();
//			var pUvs = new List<Vector2> ();
//			Debug.Log ("-------");
//
//
//			if(true){
//				for (int i = 0; i < vertices1.Count; i += 3) {
//					verticeIndices.Clear ();
//					verticeIndices.Add (i);
//					for (int j = i + 3; j < vertices1.Count; j += 3) {
//						//同一の平面上にある三角形かどうか。数値計算のためdeltaで誤差を考慮する
//						if (Vector3.Dot (Vector3.Cross (vertices1 [i + 1] - vertices1 [i], vertices1 [i + 2] - vertices1 [i + 1]).normalized, Vector3.Cross (vertices1 [j + 1] - vertices1 [j], vertices1 [j + 2] - vertices1 [j + 1]).normalized) > 1 - delta2) {
//							verticeIndices.Add (j);	
//
//						}
//
//						if (j == vertices1.Count - 3) {
//
//							if (verticeIndices.Count > 1) {
//
//
//								for (int k = 0; k < verticeIndices.Count; k++) {
//									for (int l = 0; l < 3; l++) {
//										pVertices.Add (vertices1 [verticeIndices [k] + l]);
//										pNormals.Add (normals1 [verticeIndices [k] + l]);
//										pUvs.Add (uvs1 [verticeIndices [k] + l]);
//
//										pVertices.Add (vertices1 [verticeIndices [k] + numRep (l + 1)]);
//										pNormals.Add (normals1 [verticeIndices [k] + numRep (l + 1)]);
//										pUvs.Add (uvs1 [verticeIndices [k] + numRep (l + 1)]);
//
//									}
//								}	
//
//								for (int k = 0; k < pVertices.Count; k += 2) {
//									for (int l = k + 2; l < pVertices.Count; l += 2) {
//										if (((pVertices [l + 1] - pVertices [k]).magnitude < delta) && ((pVertices [l] - pVertices [k + 1]).magnitude < delta)) {
//											pVertices.RemoveRange (l, 2);
//											pVertices.RemoveRange (k, 2);
//											pNormals.RemoveRange (l, 2);
//											pNormals.RemoveRange (k, 2);
//											pUvs.RemoveRange (l, 2);
//											pUvs.RemoveRange (k, 2);
//											k -= 2;
//											break;
//										}
//									}
//								}
//
//
//								for (int l = 0; l < pVertices.Count; l += 2) {
//									for (int k = l + 2; k < pVertices.Count; k += 2) {
//										//4つの頂点が一直線上にあるか
//										if (Vector3.Dot ((pVertices [l] - pVertices [l + 1]).normalized, (pVertices [k] - pVertices [k + 1]).normalized) > 1 - delta2) {
//											//以下重なる点に応じた処理
//											if ((pVertices [l + 1] - pVertices [k]).magnitude < delta) {
//
//												Debug.Log ("1done");
//
//												pVertices.Add (pVertices [l]);
//												pVertices.Add (pVertices [k + 1]);
//												pNormals.Add (pNormals [l]);
//												pNormals.Add (pNormals [k + 1]);
//												pUvs.Add (pUvs [l]);
//												pUvs.Add (pUvs [k + 1]);
//
//												pVertices.RemoveRange (k, 2);
//												pVertices.RemoveRange (l, 2);
//												pNormals.RemoveRange (k, 2);
//												pNormals.RemoveRange (l, 2);
//												pUvs.RemoveRange (k, 2);
//												pUvs.RemoveRange (l, 2);
//
//												l -= 2;
//												break;
//											} else if ((pVertices [l] - pVertices [k + 1]).magnitude < delta) {
//												Debug.Log ("2done");
//												pVertices.Add (pVertices [k]);
//												pVertices.Add (pVertices [l + 1]);
//												pNormals.Add (pNormals [k]);
//												pNormals.Add (pNormals [l + 1]);
//												pUvs.Add (pUvs [k]);	
//												pUvs.Add (pUvs [l + 1]);
//												pVertices.RemoveRange (k, 2);
//												pVertices.RemoveRange (l, 2);
//												pNormals.RemoveRange (k, 2);
//												pNormals.RemoveRange (l, 2);
//												pUvs.RemoveRange (k, 2);
//												pUvs.RemoveRange (l, 2);
//												l -= 2;
//												break;
//											} else if ((pVertices [l + 1] - pVertices [k + 1]).magnitude < delta) {
//												Debug.Log ("3done");
//												pVertices.Add (pVertices [l]);
//												pVertices.Add (pVertices [k]);
//												pNormals.Add (pNormals [l]);
//												pNormals.Add (pNormals [k]);
//												pUvs.Add (pUvs [l]);
//												pUvs.Add (pUvs [k]);
//												pVertices.RemoveRange (k, 2);
//												pVertices.RemoveRange (l, 2);
//												pNormals.RemoveRange (k, 2);
//												pNormals.RemoveRange (l, 2);
//												pUvs.RemoveRange (k, 2);
//												pUvs.RemoveRange (l, 2);
//												l -= 2;
//												break;
//											} else if ((pVertices [l] - pVertices [k]).magnitude < delta) {
//												Debug.Log ("4done");
//												pVertices.Add (pVertices [k + 1]);
//												pVertices.Add (pVertices [l + 1]);
//												pNormals.Add (pNormals [k + 1]);
//												pNormals.Add (pNormals [l + 1]);
//												pUvs.Add (pUvs [k + 1]);
//												pUvs.Add (pUvs [l + 1]);
//												pVertices.RemoveRange (k, 2);
//												pVertices.RemoveRange (l, 2);
//												pNormals.RemoveRange (k, 2);
//												pNormals.RemoveRange (l, 2);
//												pUvs.RemoveRange (k, 2);
//												pUvs.RemoveRange (l, 2);
//												l -= 2;
//												break;
//											} 
//										}
//
//									}
//								}
//
//
//								for (int k = 0; k < pVertices.Count; k++) {
//									for (int l = k + 1; l < pVertices.Count; l++) {
//										if ((pVertices [k] - pVertices [l]).magnitude < delta) {
//											pVertices.RemoveAt (l);
//											pNormals.RemoveAt (l);
//											pUvs.RemoveAt (l);
//											break;
//										}
//									}
//								}
//
//								int pValue = 1000000;
//								for (int k = 2; k < pVertices.Count; k++) {
//									for (int l = k + 1; l < pVertices.Count; l++) {						
//										if (System.Math.Acos (Vector3.Dot ((pValue * pVertices [0] - pValue * pVertices [1]).normalized, (pValue * pVertices [0] - pValue * pVertices [k]).normalized)) > System.Math.Acos (Vector3.Dot ((pValue * pVertices [0] - pValue * pVertices [1]).normalized, (pValue * pVertices [0] - pValue * pVertices [l]).normalized))) {
//											pVertices.Insert (k, pVertices [l]);
//											pVertices.RemoveAt (l + 1);
//											pNormals.Insert (k, pNormals [l]);
//											pNormals.RemoveAt (l + 1);
//											pUvs.Insert (k, pUvs [l]);
//											pUvs.RemoveAt (l + 1);
//											k = 1;
//											break;
//										}
//									}
//								}
//
//								for (int k = 1; k < pVertices.Count - 1; k++) {
//
//									vertices1.Insert (0, pVertices [0]);
//									normals1.Insert (0, pNormals [0]);
//									uvs1.Insert (0, pUvs [0]);
//									triangles1.Insert (0, (pVertices.Count - 2) * 3 - (3 * k));
//
//									vertices1.Insert (1, pVertices [k]);
//									normals1.Insert (1, pNormals [k]);
//									uvs1.Insert (1, pUvs [k]);
//									triangles1.Insert (1, (pVertices.Count - 2) * 3 - (3 * k - 1));
//
//									vertices1.Insert (2, pVertices [k + 1]);
//									normals1.Insert (2, pNormals [k + 1]);
//									uvs1.Insert (2, pUvs [k + 1]);	
//									triangles1.Insert (2, (pVertices.Count - 2) * 3 - (3 * k - 2));
//
//
//								}
//
//								for (int k = verticeIndices.Count - 1; k >= 0; k--) {
//									vertices1.RemoveRange (verticeIndices [k] + 3 * (pVertices.Count - 2), 3);
//									normals1.RemoveRange (verticeIndices [k] + 3 * (pVertices.Count - 2), 3);
//									uvs1.RemoveRange (verticeIndices [k] + 3 * (pVertices.Count - 2), 3);
//									triangles1.RemoveRange (verticeIndices [k] + 3 * (pVertices.Count - 2), 3);
//
//								}
//
//								//						for (int l =  3*(pVertices.Count-2) ; l < triangles1.Count; l++) {
//								//							
//								//							for (int k =0; k <verticeIndices.Count; k++) {
//								//								if (triangles1 [l] > verticeIndices[k]+2) {
//								//									triangles1 [l] -= 3;
//								//								}
//								//							}
//								//							triangles1 [l] += 3 * (pVertices.Count-2);
//								//
//								//						}
//
//
//								for (int k = 0; k < triangles1.Count; k++) {
//									triangles1 [k] = k;
//									//Debug.Log (triangles1 [k]);
//								}	
//
//								pVertices.Clear ();
//								pNormals.Clear ();
//								pUvs.Clear ();
//
//							}
//
//
//						}
//
//					}
//				}
//
//				for (int i = 0; i < vertices2.Count; i += 3) {
//					verticeIndices.Clear ();
//					verticeIndices.Add (i);
//					for (int j = i + 3; j < vertices2.Count; j += 3) {
//						//同一の平面上にある三角形かどうか。数値計算のためdeltaで誤差を考慮する
//						if (Vector3.Dot (Vector3.Cross (vertices2 [i + 1] - vertices2 [i], vertices2 [i + 2] - vertices2 [i + 1]).normalized, Vector3.Cross (vertices2 [j + 1] - vertices2 [j], vertices2 [j + 2] - vertices2 [j + 1]).normalized) > 1 - delta) {
//							verticeIndices.Add (j);	
//
//						}
//
//						if (j == vertices2.Count - 3) {
//
//							if (verticeIndices.Count > 1) {
//
//
//								for (int k = 0; k < verticeIndices.Count; k++) {
//									for (int l = 0; l < 3; l++) {
//										pVertices.Add (vertices2 [verticeIndices [k] + l]);
//										pNormals.Add (normals2 [verticeIndices [k] + l]);
//										pUvs.Add (uvs2 [verticeIndices [k] + l]);
//
//										pVertices.Add (vertices2 [verticeIndices [k] + numRep (l + 1)]);
//										pNormals.Add (normals2 [verticeIndices [k] + numRep (l + 1)]);
//										pUvs.Add (uvs2 [verticeIndices [k] + numRep (l + 1)]);
//
//									}
//								}	
//
//								for (int k = 0; k < pVertices.Count; k += 2) {
//									for (int l = k + 2; l < pVertices.Count; l += 2) {
//										if (((pVertices [l + 1] - pVertices [k]).magnitude < delta) && ((pVertices [l] - pVertices [k + 1]).magnitude < delta)) {
//											pVertices.RemoveRange (l, 2);
//											pVertices.RemoveRange (k, 2);
//											pNormals.RemoveRange (l, 2);
//											pNormals.RemoveRange (k, 2);
//											pUvs.RemoveRange (l, 2);
//											pUvs.RemoveRange (k, 2);
//											k -= 2;
//											break;
//										}
//									}
//								}
//
//
//								for (int l = 0; l < pVertices.Count; l += 2) {
//									for (int k = l + 2; k < pVertices.Count; k += 2) {
//										//4つの頂点が一直線上にあるか
//										if (Vector3.Dot ((pVertices [l] - pVertices [l + 1]).normalized, (pVertices [k] - pVertices [k + 1]).normalized) > 1 - delta2) {
//											//以下重なる点に応じた処理
//											if ((pVertices [l + 1] - pVertices [k]).magnitude < delta) {
//
//												Debug.Log ("1done");
//
//												pVertices.Add (pVertices [l]);
//												pVertices.Add (pVertices [k + 1]);
//												pNormals.Add (pNormals [l]);
//												pNormals.Add (pNormals [k + 1]);
//												pUvs.Add (pUvs [l]);
//												pUvs.Add (pUvs [k + 1]);
//
//												pVertices.RemoveRange (k, 2);
//												pVertices.RemoveRange (l, 2);
//												pNormals.RemoveRange (k, 2);
//												pNormals.RemoveRange (l, 2);
//												pUvs.RemoveRange (k, 2);
//												pUvs.RemoveRange (l, 2);
//
//												l -= 2;
//												break;
//											} else if ((pVertices [l] - pVertices [k + 1]).magnitude < delta) {
//												Debug.Log ("2done");
//												pVertices.Add (pVertices [k]);
//												pVertices.Add (pVertices [l + 1]);
//												pNormals.Add (pNormals [k]);
//												pNormals.Add (pNormals [l + 1]);
//												pUvs.Add (pUvs [k]);	
//												pUvs.Add (pUvs [l + 1]);
//												pVertices.RemoveRange (k, 2);
//												pVertices.RemoveRange (l, 2);
//												pNormals.RemoveRange (k, 2);
//												pNormals.RemoveRange (l, 2);
//												pUvs.RemoveRange (k, 2);
//												pUvs.RemoveRange (l, 2);
//												l -= 2;
//												break;
//											} else if ((pVertices [l + 1] - pVertices [k + 1]).magnitude < delta) {
//												Debug.Log ("3done");
//												pVertices.Add (pVertices [l]);
//												pVertices.Add (pVertices [k]);
//												pNormals.Add (pNormals [l]);
//												pNormals.Add (pNormals [k]);
//												pUvs.Add (pUvs [l]);
//												pUvs.Add (pUvs [k]);
//												pVertices.RemoveRange (k, 2);
//												pVertices.RemoveRange (l, 2);
//												pNormals.RemoveRange (k, 2);
//												pNormals.RemoveRange (l, 2);
//												pUvs.RemoveRange (k, 2);
//												pUvs.RemoveRange (l, 2);
//												l -= 2;
//												break;
//											} else if ((pVertices [l] - pVertices [k]).magnitude < delta) {
//												Debug.Log ("4done");
//												pVertices.Add (pVertices [k + 1]);
//												pVertices.Add (pVertices [l + 1]);
//												pNormals.Add (pNormals [k + 1]);
//												pNormals.Add (pNormals [l + 1]);
//												pUvs.Add (pUvs [k + 1]);
//												pUvs.Add (pUvs [l + 1]);
//												pVertices.RemoveRange (k, 2);
//												pVertices.RemoveRange (l, 2);
//												pNormals.RemoveRange (k, 2);
//												pNormals.RemoveRange (l, 2);
//												pUvs.RemoveRange (k, 2);
//												pUvs.RemoveRange (l, 2);
//												l -= 2;
//												break;
//											} 
//										}
//
//									}
//								}
//
//
//								for (int k = 0; k < pVertices.Count; k++) {
//									for (int l = k + 1; l < pVertices.Count; l++) {
//										if ((pVertices [k] - pVertices [l]).magnitude < delta) {
//											pVertices.RemoveAt (l);
//											pNormals.RemoveAt (l);
//											pUvs.RemoveAt (l);
//											break;
//										}
//									}
//								}
//
//								int pValue = 1000000;
//								for (int k = 2; k < pVertices.Count; k++) {
//									for (int l = k + 1; l < pVertices.Count; l++) {						
//										if (System.Math.Acos (Vector3.Dot ((pValue * pVertices [0] - pValue * pVertices [1]).normalized, (pValue * pVertices [0] - pValue * pVertices [k]).normalized)) > System.Math.Acos (Vector3.Dot ((pValue * pVertices [0] - pValue * pVertices [1]).normalized, (pValue * pVertices [0] - pValue * pVertices [l]).normalized))) {
//											pVertices.Insert (k, pVertices [l]);
//											pVertices.RemoveAt (l + 1);
//											pNormals.Insert (k, pNormals [l]);
//											pNormals.RemoveAt (l + 1);
//											pUvs.Insert (k, pUvs [l]);
//											pUvs.RemoveAt (l + 1);
//											k = 1;
//											break;
//										}
//									}
//								}
//
//								for (int k = 1; k < pVertices.Count - 1; k++) {
//
//									vertices2.Insert (0, pVertices [0]);
//									normals2.Insert (0, pNormals [0]);
//									uvs2.Insert (0, pUvs [0]);
//									triangles2.Insert (0, (pVertices.Count - 2) * 3 - (3 * k));
//
//									vertices2.Insert (1, pVertices [k]);
//									normals2.Insert (1, pNormals [k]);
//									uvs2.Insert (1, pUvs [k]);
//									triangles2.Insert (1, (pVertices.Count - 2) * 3 - (3 * k - 1));
//
//									vertices2.Insert (2, pVertices [k + 1]);
//									normals2.Insert (2, pNormals [k + 1]);
//									uvs2.Insert (2, pUvs [k + 1]);	
//									triangles2.Insert (2, (pVertices.Count - 2) * 3 - (3 * k - 2));
//
//
//								}
//
//								for (int k = verticeIndices.Count - 1; k >= 0; k--) {
//									vertices2.RemoveRange (verticeIndices [k] + 3 * (pVertices.Count - 2), 3);
//									normals2.RemoveRange (verticeIndices [k] + 3 * (pVertices.Count - 2), 3);
//									uvs2.RemoveRange (verticeIndices [k] + 3 * (pVertices.Count - 2), 3);
//									triangles2.RemoveRange (verticeIndices [k] + 3 * (pVertices.Count - 2), 3);
//
//								}
//
//								//						for (int l =  3*(pVertices.Count-2) ; l < triangles2.Count; l++) {
//								//							
//								//							for (int k =0; k <verticeIndices.Count; k++) {
//								//								if (triangles2 [l] > verticeIndices[k]+2) {
//								//									triangles2 [l] -= 3;
//								//								}
//								//							}
//								//							triangles2 [l] += 3 * (pVertices.Count-2);
//								//
//								//						}
//
//
//								for (int k = 0; k < triangles2.Count; k++) {
//									triangles2 [k] = k;
//									//Debug.Log (triangles2 [k]);
//								}	
//
//								pVertices.Clear ();
//								pNormals.Clear ();
//								pUvs.Clear ();
//
//							}
//
//
//						}
//
//					}
//				}
//
//			}//this
//
//			//メッシュを減らす処理
//			//新しく頂点を作っていくだけでもカットしていけますが、どんどん頂点が増えて処理時間が長くなるため、一つの三角形にできるものを出来るだけ１つにします
//			//ただ、１回、もしくは数回だけ切るのであればこの処理はない方が速いです
//			//１つの三角形に対して全ての三角形を検査するループを全てまわす
//			//		for (int i = 0; i < vertices1.Count; i += 3) {
//			//			for (int j = i + 3; j < vertices1.Count; j += 3) {
//			//				//同一の平面上にある三角形かどうか。数値計算のためdeltaで誤差を考慮する
//			//				if (Vector3.Dot (Vector3.Cross (vertices1 [i + 1] - vertices1 [i], vertices1 [i + 2] - vertices1 [i + 1]).normalized, Vector3.Cross (vertices1 [j + 1] - vertices1 [j], vertices1 [j + 2] - vertices1 [j + 1]).normalized) > 1 - delta) {
//			//					//３つの辺に対して一致する辺があるかループ
//			//					for (int k = 0; k < 3; k++) {
//			//						for (int n = 0; n < 3; n++) {
//			//							//一致する辺であるかどうか
//			//							if ((vertices1 [i + numRep (1 + k)] - vertices1 [j + n]).magnitude < delta && (vertices1 [i + k] - vertices1 [j + numRep (1 + n)]).magnitude < delta) {
//			//								//一致する辺のとなりの辺同士が直線上にあるかどうか
//			//								for (int m = 0; m < 2; m++) {
//			//									if (Vector3.Dot ((vertices1 [i + numRep (2 + m + k)] - vertices1 [i + numRep (1 + k + m)]).normalized, (vertices1 [j + numRep (2 * m + n)] - vertices1 [j + numRep (2 + 2 * m + n)]).normalized) > 1 - delta) {
//			//
//			//										//新しい合体した三角形を追加
//			//										vertices1.Add (vertices1 [i + numRep (m + k)]);
//			//										uv1.Add (uv1 [i + numRep (m + k)]);
//			//										normals1.Add (normals1 [i + numRep (m + k)]);
//			//										triangles1.Add (vertices1.Count - 1);
//			//
//			//										//追加する順序(表面の方向)が異なる
//			//										if (m == 0) {
//			//											vertices1.Add (vertices1 [j + numRep (2 + n)]);
//			//											uv1.Add (uv1 [j + numRep (2 + n)]);
//			//											normals1.Add (normals1 [j + numRep (2 + n)]);
//			//											triangles1.Add (vertices1.Count - 1);
//			//
//			//											vertices1.Add (vertices1 [i + numRep (2 + k)]);
//			//											uv1.Add (uv1 [i + numRep (2 + k)]);
//			//											normals1.Add (normals1 [i + numRep (2 + k)]);
//			//											triangles1.Add (vertices1.Count - 1);
//			//
//			//										} else {
//			//
//			//											vertices1.Add (vertices1 [i + numRep (2 + k)]);
//			//											uv1.Add (uv1 [i + numRep (2 + k)]);
//			//											normals1.Add (normals1 [i + numRep (2 + k)]);
//			//											triangles1.Add (vertices1.Count - 1);
//			//
//			//											vertices1.Add (vertices1 [j + numRep (2 + n)]);
//			//											uv1.Add (uv1 [j + numRep (2 + n)]);
//			//											normals1.Add (normals1 [j + numRep (2 + n)]);
//			//											triangles1.Add (vertices1.Count - 1);
//			//
//			//										}
//			//
//			//										//合体前の２つの三角形を消去
//			//										vertices1.RemoveRange (j, 3);
//			//										uv1.RemoveRange (j, 3);
//			//										normals1.RemoveRange (j, 3);
//			//										triangles1.RemoveRange (j, 3);
//			//
//			//										vertices1.RemoveRange (i, 3);
//			//										uv1.RemoveRange (i, 3);
//			//										normals1.RemoveRange (i, 3);
//			//										triangles1.RemoveRange (i, 3);
//			//
//			//
//			//										//triangle[]はvertices[]のインデックスで三角形を指定するため、vertices[]中の点を消去するとズレるため、それを修正
//			//										for (int l = 0; l < triangles1.Count; l++) {
//			//											if (triangles1 [l] >= j) {
//			//												triangles1 [l] -= 3;
//			//											}
//			//											if (triangles1 [l] >= i) {
//			//												triangles1 [l] -= 3;
//			//											}
//			//										}
//			//										//ふたつ上のループも抜け出すために
//			//										j = vertices1.Count;
//			//										n = 3;
//			//										k = 3;
//			//										//頂点が3つ減るため、その分を考慮
//			//										i = -3;
//			//							
//			//										break;
//			//
//			//									}
//			//								}
//			//							}
//			//						}
//			//					}
//			//				}	
//			//			}
//			//		}  
//			//		//もう片方についても同様の処理
//			//		for (int i = 0; i < vertices2.Count; i += 3) {
//			//			for (int j = i + 3; j < vertices2.Count; j += 3) {
//			//				//同一の平面上にある三角形かどうか。数値計算のためdeltaで誤差を考慮する
//			//				if (Vector3.Dot (Vector3.Cross (vertices2 [i + 1] - vertices2 [i], vertices2 [i + 2] - vertices2 [i + 1]).normalized, Vector3.Cross (vertices2 [j + 1] - vertices2 [j], vertices2 [j + 2] - vertices2 [j + 1]).normalized) > 1 - delta) {
//			//					//３つの辺に対して一致する辺があるかループ
//			//					for (int k = 0; k < 3; k++) {
//			//						for (int n = 0; n < 3; n++) {
//			//							//一致する辺であるかどうか
//			//							if ((vertices2 [i + numRep (1 + k)] - vertices2 [j + n]).magnitude < delta && (vertices2 [i + k] - vertices2 [j + numRep (1 + n)]).magnitude < delta) {
//			//								//一致する辺のとなりの辺同士が直線上にあるかどうか
//			//								for (int m = 0; m < 2; m++) {
//			//									if (Vector3.Dot ((vertices2 [i + numRep (2 + m + k)] - vertices2 [i + numRep (1 + k + m)]).normalized, (vertices2 [j + numRep (2 * m + n)] - vertices2 [j + numRep (2 + 2 * m + n)]).normalized) > 1 - delta) {
//			//
//			//										//新しい合体した三角形を追加
//			//										vertices2.Add (vertices2 [i + numRep (m + k)]);
//			//										uv2.Add (uv2 [i + numRep (m + k)]);
//			//										normals2.Add (normals2 [i + numRep (m + k)]);
//			//										triangles2.Add (vertices2.Count - 1);
//			//
//			//										//追加する順序(表面の方向)が異なる
//			//										if (m == 0) {
//			//											vertices2.Add (vertices2 [j + numRep (2 + n)]);
//			//											uv2.Add (uv2 [j + numRep (2 + n)]);
//			//											normals2.Add (normals2 [j + numRep (2 + n)]);
//			//											triangles2.Add (vertices2.Count - 1);
//			//
//			//											vertices2.Add (vertices2 [i + numRep (2 + k)]);
//			//											uv2.Add (uv2 [i + numRep (2 + k)]);
//			//											normals2.Add (normals2 [i + numRep (2 + k)]);
//			//											triangles2.Add (vertices2.Count - 1);
//			//
//			//										} else {
//			//
//			//											vertices2.Add (vertices2 [i + numRep (2 + k)]);
//			//											uv2.Add (uv2 [i + numRep (2 + k)]);
//			//											normals2.Add (normals2 [i + numRep (2 + k)]);
//			//											triangles2.Add (vertices2.Count - 1);
//			//
//			//											vertices2.Add (vertices2 [j + numRep (2 + n)]);
//			//											uv2.Add (uv2 [j + numRep (2 + n)]);
//			//											normals2.Add (normals2 [j + numRep (2 + n)]);
//			//											triangles2.Add (vertices2.Count - 1);
//			//
//			//										}
//			//
//			//
//			//
//			//										//合体前の２つの三角形を消去
//			//										vertices2.RemoveRange (j, 3);
//			//										uv2.RemoveRange (j, 3);
//			//										normals2.RemoveRange (j, 3);
//			//										triangles2.RemoveRange (j, 3);
//			//
//			//										vertices2.RemoveRange (i, 3);
//			//										uv2.RemoveRange (i, 3);
//			//										normals2.RemoveRange (i, 3);
//			//										triangles2.RemoveRange (i, 3);
//			//
//			//
//			//
//			//										//triangle[]はvertices[]のインデックスで三角形を指定するため、vertices[]中の点を消去するとズレるため、それを修正
//			//										for (int l = 0; l < triangles2.Count; l++) {
//			//											if (triangles2 [l] >= j) {
//			//												triangles2 [l] -= 3;
//			//											}
//			//											if (triangles2 [l] >= i) {
//			//												triangles2 [l] -= 3;
//			//											}
//			//										}
//			//										//ループも抜け出すために
//			//										j = vertices2.Count;
//			//										n = 3;
//			//										k = 3;
//			//										//頂点が3つ減るため、その分を考慮
//			//										i = -3;
//			//
//			//										break;
//			//
//			//									}
//			//								}
//			//							}
//			//						}
//			//					}
//			//				}
//			//			}
//			//		} 
//			//
//			Debug.Log("cross");
//			Debug.Log(crossVertices.Count);
//			//断面をつくる処理
//			if (crossVertices.Count != 0) {
//				//断面で頂点を減らす処理、直線状にある頂点を2つのみにする
//				for (int i = 0; i < crossVertices.Count; i += 2) {
//					for (int k = i + 2; k < crossVertices.Count; k += 2) {
//						//4つの頂点が一直線上にあるか
//						if ((System.Math.Abs (Vector3.Dot ((crossVertices [i] - crossVertices [i + 1]).normalized, (crossVertices [k] - crossVertices [k + 1]).normalized)) > 1 - delta2)) {
//							//以下重なる点に応じた処理
//							if ((crossVertices [i + 1] - crossVertices [k]).magnitude < delta) {
//								Debug.Log("1cross");					
//								crossVertices.Add (crossVertices [i]);
//								crossVertices.Add (crossVertices [k + 1]);
//								crossVertices.RemoveRange (k, 2);
//								crossVertices.RemoveRange (i, 2);
//								i -= 2;
//								break;
//							} else if ((crossVertices [i] - crossVertices [k + 1]).magnitude < delta) {
//								Debug.Log("2cross");
//
//								crossVertices.Add (crossVertices [k]);
//								crossVertices.Add (crossVertices [i + 1]);
//								crossVertices.RemoveRange (k, 2);
//								crossVertices.RemoveRange (i, 2);
//								i -= 2;
//								break;
//							} else if ((crossVertices [i + 1] - crossVertices [k + 1]).magnitude < delta) {
//								Debug.Log("3cross");
//								crossVertices.Add (crossVertices [i]);
//								crossVertices.Add (crossVertices [k]);
//
//								crossVertices.RemoveRange (k, 2);
//								crossVertices.RemoveRange (i, 2);
//								i -= 2;
//								break;
//							} else if ((crossVertices [i] - crossVertices [k]).magnitude < delta) {
//								Debug.Log("4cross");
//								crossVertices.Add (crossVertices [k + 1]);
//								crossVertices.Add (crossVertices [i + 1]);
//								crossVertices.RemoveRange (k, 2);
//								crossVertices.RemoveRange (i, 2);
//								i -= 2;
//								break;
//							} 
//						}
//
//					}
//				}
//				Debug.Log(crossVertices.Count);	
//				for (int k = 0; k < crossVertices.Count; k++) {
//
//					Debug.Log (crossVertices [k].ToString ("F5"));
//				}
//
//				//断面の三角形を作る処理
//
//				for (int i = 0; i < crossVertices.Count; i++) {
//					for (int j = i + 1; j < crossVertices.Count; j++) {
//						if ((crossVertices [i] - crossVertices [j]).magnitude < delta) {
//							crossVertices.RemoveAt (j);
//							break;
//						}
//					}
//				}
//				int pValue = 1000000;
//				for (int i = 2; i < crossVertices.Count; i++) {
//					for (int j = i + 1; j < crossVertices.Count; j++) {						
//						if (System.Math.Acos (Vector3.Dot ((pValue * crossVertices [0] - pValue * crossVertices [1]).normalized, (pValue * crossVertices [0] - pValue * crossVertices [i]).normalized)) > System.Math.Acos (Vector3.Dot ((pValue * crossVertices [0] - pValue * crossVertices [1]).normalized, (pValue * crossVertices [0] - pValue * crossVertices [j]).normalized))) {
//							crossVertices.Insert (i, crossVertices [j]);
//							crossVertices.RemoveAt (j + 1);
//							i = 1;
//							break;
//						}
//					}
//				}
//
//
//
//
//
//				for (int i = 1; i < crossVertices.Count - 1; i++) {
//					for (int j = 0; j < 3; j++) {
//						normals1.Add (-cutPlane.normal);
//						uvs1.Add (new Vector2 (0, 0));
//						normals2.Add (cutPlane.normal);
//						uvs2.Add (new Vector2 (0, 0));
//					}
//					if (Vector3.Dot (Vector3.Cross ((transform.TransformPoint (crossVertices [i].normalized) - transform.TransformPoint (crossVertices [0].normalized)), (transform.TransformPoint (crossVertices [i + 1].normalized) - transform.TransformPoint (crossVertices [i].normalized))), cutPlane.normal) < 0) {
//
//						vertices1.Add (crossVertices [0]);
//						triangles1.Add (vertices1.Count - 1);
//
//						vertices1.Add (crossVertices [i]);
//						triangles1.Add (vertices1.Count - 1);
//
//						vertices1.Add (crossVertices [i + 1]);
//						triangles1.Add (vertices1.Count - 1);
//
//						vertices2.Add (crossVertices [i]);
//						triangles2.Add (vertices2.Count - 1);
//
//						vertices2.Add (crossVertices [0]);
//						triangles2.Add (vertices2.Count - 1);
//
//						vertices2.Add (crossVertices [i + 1]);
//						triangles2.Add (vertices2.Count - 1);
//					} else {
//
//
//						vertices1.Add (crossVertices [i]);
//						triangles1.Add (vertices1.Count - 1);
//
//						vertices1.Add (crossVertices [0]);
//						triangles1.Add (vertices1.Count - 1);
//
//						vertices1.Add (crossVertices [i + 1]);
//						triangles1.Add (vertices1.Count - 1);
//
//						vertices2.Add (crossVertices [0]);
//						triangles2.Add (vertices2.Count - 1);
//
//						vertices2.Add (crossVertices [i]);
//						triangles2.Add (vertices2.Count - 1);
//
//						vertices2.Add (crossVertices [i + 1]);
//						triangles2.Add (vertices2.Count - 1);
//
//					}
//				}
//
//
//
//			}
//
//
//
//
//			GameObject obj = new GameObject ("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshCut));
//			var mesh = new Mesh ();
//			mesh.vertices = vertices1.ToArray ();
//			mesh.triangles = triangles1.ToArray ();
//			mesh.uv = uvs1.ToArray ();
//			mesh.normals = normals1.ToArray ();
//			obj.GetComponent<MeshFilter> ().mesh = mesh;
//			obj.GetComponent<MeshRenderer> ().materials = GetComponent<MeshRenderer> ().materials;
//			obj.GetComponent<MeshCollider> ().sharedMesh = mesh;
//			obj.GetComponent<MeshCollider> ().inflateMesh = true;
//			obj.GetComponent<MeshCollider> ().skinWidth = skinWidth;
//			obj.GetComponent<MeshCollider> ().convex = true;
//			obj.GetComponent<MeshCollider> ().material = GetComponent<Collider> ().material;
//			obj.transform.position = transform.position;
//			obj.transform.rotation = transform.rotation;
//			obj.transform.localScale = transform.localScale;
//			obj.GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody> ().velocity;
//			obj.GetComponent<Rigidbody> ().angularVelocity = GetComponent<Rigidbody> ().angularVelocity;
//			obj.GetComponent<MeshCut> ().skinWidth = skinWidth;
//			obj.tag = "obj";
//			//	obj.GetComponent<MeshCut>().Start();
//
//			GameObject obj2 = new GameObject ("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshCut));
//
//			var mesh2 = new Mesh ();
//			mesh2.vertices = vertices2.ToArray ();
//			mesh2.triangles = triangles2.ToArray ();
//			mesh2.uv = uvs2.ToArray ();
//			mesh2.normals = normals2.ToArray ();
//			obj2.GetComponent<MeshFilter> ().mesh = mesh2;
//			obj2.GetComponent<MeshRenderer> ().materials = GetComponent<MeshRenderer> ().materials;
//			obj2.GetComponent<MeshCollider> ().sharedMesh = mesh2;
//			obj2.GetComponent<MeshCollider> ().inflateMesh = true;
//			obj2.GetComponent<MeshCollider> ().skinWidth = skinWidth;
//			obj2.GetComponent<MeshCollider> ().convex = true;
//			obj2.GetComponent<MeshCollider> ().material = GetComponent<Collider> ().material;
//			obj2.transform.position = transform.position;
//			obj2.transform.rotation = transform.rotation;
//			obj2.transform.localScale = transform.localScale;
//			obj2.GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody> ().velocity;
//			obj2.GetComponent<Rigidbody> ().angularVelocity = GetComponent<Rigidbody> ().angularVelocity;
//			obj2.GetComponent<MeshCut> ().skinWidth = skinWidth;
//			obj2.tag = "obj";
//			//	obj2.GetComponent<MeshCut>().Start();
//
//			Destroy (gameObject);
//
//		}
//
//		int numRep (int i)
//		{
//			if (i % 3 == 0) {
//				return 0;
//			} else if (i % 3 == 1) {
//				return 1;
//			} else if (i % 3 == 2) {
//				return 2;
//			} else {
//				return 0;
//			}
//		}
//
//	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCutComponentTest : MonoBehaviour
{

    private MeshFilter attachedMeshFilter;

    private bool coliBool = false;
    private float delta = 0.000001f;
    private float delta2 = 0.001f;
    public float skinWidth = 0.005f;

    float time0;
    public GameObject point;
    public GameObject point2;
    public GameObject point3;
    public GameObject planeObj;
    public GameObject objP1;
    public GameObject objP2;
    public GameObject objP3;

    public GameObject objP4;
    public GameObject objP5;
    public GameObject objP6;


    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

	private float updateSecond = 0.1f;	
	public bool bool1=false;
	public bool bool2=false;
    void Start()
    {

		initTri();
    }

    void BoolOn()
    {

    }

    void initTri()
    {

        GameObject[] desObj = GameObject.FindGameObjectsWithTag("obj");
        foreach (GameObject obj in desObj)
        {
            Destroy(obj);
        }
        vertices.Clear();
        triangles.Clear();

        vertices.Add(objP1.transform.position);
        vertices.Add(objP2.transform.position);
        vertices.Add(objP3.transform.position);

        vertices.Add(objP1.transform.position);
        vertices.Add(objP3.transform.position);
        vertices.Add(objP4.transform.position);

		vertices.Add(objP1.transform.position);
        vertices.Add(objP4.transform.position);
        vertices.Add(objP5.transform.position);

		vertices.Add(objP5.transform.position);
		vertices.Add(objP4.transform.position);
        vertices.Add(objP6.transform.position);

        for (int i = 0; i < vertices.Count; i++)
        {
            triangles.Add(i);
            //  Instantiate(point, vertices[i], Quaternion.identity);
        }

        Cut(new Plane(planeObj.transform.right, planeObj.transform.position));

		Invoke("initTri",updateSecond );

    }


    public void Cut(Plane cutPlane)
    {

        int avoidInfCount = 0;


        Vector3 p1, p2, p3;
        bool p1Bool, p2Bool, p3Bool;
        //カットした後の２つのオブジェクトに対応するuv,vertices, triangles, normals
        var uvs1 = new List<Vector2>();
        var uvs2 = new List<Vector2>();
        var vertices1 = new List<Vector3>();
        var vertices2 = new List<Vector3>();
        var triangles1 = new List<int>();
        var triangles2 = new List<int>();
        var normals1 = new List<Vector3>();
        var normals2 = new List<Vector3>();
        //処理中に使ういれもの
        var crossVertices = new List<Vector3>();
        int numLimit = 3000;
        /*カットしたいオブジェクトのメッシュをトライアングルごとに処理
		*/

        for (int i = 0; i < triangles.Count; i += 3)
        {
			if(bool2){
				                for (int k = 0; k < 3; k++)
                {
                    vertices1.Add(vertices[triangles[i + k]]);
                    triangles1.Add(vertices1.Count - 1);
                }

			}else{
            //メッシュの3つの頂点を取得
            p1 = vertices[triangles[i]];
            p2 = vertices[triangles[i + 1]];
            p3 = vertices[triangles[i + 2]];

            //頂点がカットする面のどちら側にあるか
            p1Bool = cutPlane.GetSide(p1);
            p2Bool = cutPlane.GetSide(p2);
            p3Bool = cutPlane.GetSide(p3);

            //3つの頂点が同じ側にある場合はそのまま代入、頂点がカットする場合はその処理を行う
            if (p1Bool && p2Bool && p3Bool)
            {
                //3つの頂点が同じ側にある、そのままそれぞれの1に代入
                for (int k = 0; k < 3; k++)
                {
                    vertices1.Add(vertices[triangles[i + k]]);
                    triangles1.Add(vertices1.Count - 1);
                }


            }
            else if (!p1Bool && !p2Bool && !p3Bool)
            {
                //3つの頂点が同じ側にある、そのままそれぞれの２に代入
                for (int k = 0; k < 3; k++)
                {
                    vertices2.Add(vertices[triangles[i + k]]);
                    triangles2.Add(vertices2.Count - 1);
                }
            }
            else
            {
                //3つの頂点が同じ側にない場合の処理１、以下仲間外れの頂点をp,それ以外をcとする
                Vector3 p, c1, c2;
                int n1, n2, n3;
                if ((p1Bool && !p2Bool && !p3Bool) || (!p1Bool && p2Bool && p3Bool))
                {
                    p = p1;
                    c1 = p2;
                    c2 = p3;
                    n1 = 0;
                    n2 = 1;
                    n3 = 2;

                }
                else if ((!p1Bool && p2Bool && !p3Bool) || (p1Bool && !p2Bool && p3Bool))
                {
                    p = p2;
                    c1 = p3;
                    c2 = p1;
                    n1 = 1;
                    n2 = 2;
                    n3 = 0;

                }
                else
                {
                    p = p3;
                    c1 = p1;
                    c2 = p2;
                    n1 = 2;
                    n2 = 0;
                    n3 = 1;

                }

                //カットした面に生じる新しい頂点を計算、カットする平面の法線方向に対するpとcの距離の比からc-pの長さを決める
                Vector3 cross1 = (p + (c1 - p) * (cutPlane.distance + Vector3.Dot(cutPlane.normal, p)) / Vector3.Dot(cutPlane.normal, p - c1));
                Vector3 cross2 = (p + (c2 - p) * (cutPlane.distance + Vector3.Dot(cutPlane.normal, p)) / Vector3.Dot(cutPlane.normal, p - c2));


                //断面をつくるために取っておく
                crossVertices.Add(cross1);
                crossVertices.Add(cross2);


                //pの２通りの処理、カットする面に対してどちらにあるかで異なる
                if ((p1Bool && !p2Bool && !p3Bool) || (!p1Bool && p2Bool && !p3Bool) || (!p1Bool && !p2Bool && p3Bool))
                {
                    //p側のメッシュを追加
                    vertices1.Add(cross1);
                    triangles1.Add(vertices1.Count - 1);

                    vertices1.Add(cross2);
                    triangles1.Add(vertices1.Count - 1);

                    vertices1.Add(vertices[triangles[i + n1]]);
                    triangles1.Add(vertices1.Count - 1);

                    //c側のメッシュを追加１
                    vertices2.Add(cross2);
                    triangles2.Add(vertices2.Count - 1);

                    vertices2.Add(vertices[triangles[i + n2]]);
                    triangles2.Add(vertices2.Count - 1);

                    vertices2.Add(vertices[triangles[i + n3]]);
                    triangles2.Add(vertices2.Count - 1);

                    //c側のメッシュを追加2
                    vertices2.Add(cross2);
                    triangles2.Add(vertices2.Count - 1);

                    vertices2.Add(cross1);
                    triangles2.Add(vertices2.Count - 1);

                    vertices2.Add(vertices[triangles[i + n2]]);
                    triangles2.Add(vertices2.Count - 1);
                }
                else
                {
                    //p側のメッシュを追加
                    vertices2.Add(cross1);
                    triangles2.Add(vertices2.Count - 1);

                    vertices2.Add(cross2);
                    triangles2.Add(vertices2.Count - 1);

                    vertices2.Add(vertices[triangles[i + n1]]);
                    triangles2.Add(vertices2.Count - 1);

                    //c側のメッシュを追加１
                    vertices1.Add(cross2);
                    triangles1.Add(vertices1.Count - 1);

                    vertices1.Add(vertices[triangles[i + n2]]);
                    triangles1.Add(vertices1.Count - 1);

                    vertices1.Add(vertices[triangles[i + n3]]);
                    triangles1.Add(vertices1.Count - 1);

                    //c側のメッシュを追加2
                    vertices1.Add(cross2);
                    triangles1.Add(vertices1.Count - 1);

                    vertices1.Add(cross1);
                    triangles1.Add(vertices1.Count - 1);

                    vertices1.Add(vertices[triangles[i + n2]]);
                    triangles1.Add(vertices1.Count - 1);
                }
            }
        }
		}
		for(int i = 0;i<crossVertices.Count;i++){
			Instantiate(point,crossVertices[i],Quaternion.identity);
		}


        var verticeIndices = new List<int>();
        var pVertices = new List<Vector3>();
        var pNormals = new List<Vector3>();
        var pUvs = new List<Vector2>();

        //Debug.Log ("-------");
        int numCur = 10;
        Debug.Log(triangles1.Count);

        for (int i = 0; i < vertices1.Count; i += 3)
        {
            uvs1.Add(new Vector2(0, 0));
            uvs1.Add(new Vector2(0, 1));
            uvs1.Add(new Vector2(1, 0));
            normals1.Add(new Vector3(0, 0, -1).normalized);
            normals1.Add(new Vector3(0, 0, -1).normalized);
            normals1.Add(new Vector3(0, 0, -1).normalized);
        }

        for (int i = 0; i < vertices2.Count; i += 3)
        {
            uvs2.Add(new Vector2(0, 0));
            uvs2.Add(new Vector2(0, 1));
            uvs2.Add(new Vector2(1, 0));
            normals2.Add(new Vector3(0, 0, -1).normalized);
            normals2.Add(new Vector3(0, 0, -1).normalized);
            normals2.Add(new Vector3(0, 0, -1).normalized);
        }


        if (bool1)
        {
            for (int i = 0; i < vertices1.Count; i += 3)
            {

                verticeIndices.Clear();
                verticeIndices.Add(i);
                Debug.Log("---------------------------------");
                for (int j = i + 3; j < vertices1.Count; j += 3)
                {
                    //同一の平面上にある三角形かどうか。数値計算のためdeltaで誤差を考慮する
                    //	Debug.Log(Vector3.Dot (Vector3.Cross (vertices1 [i + 1] - vertices1 [i], vertices1 [i + 1] - vertices1 [i + 2]).normalized, Vector3.Cross (vertices1 [j + 1] - vertices1 [j], vertices1 [j + 1] - vertices1 [j + 2]).normalized));
                    //	Debug.Log(Vector3.Dot (Vector3.Cross (vertices1 [i + 1] - vertices1 [i], vertices1 [i +2] - vertices1 [i + 1]).normalized, Vector3.Cross (vertices1 [j + 1] - vertices1 [j], vertices1 [j + 2] - vertices1 [j + 1]).normalized));
                    //if (Vector3.Dot (Vector3.Cross ((10^numCur)*vertices1 [i + 1] - (10^numCur)*vertices1 [i],  (10^numCur)*vertices1 [i + 2]-(10^numCur)*vertices1 [i + 1]).normalized, Vector3.Cross ((10^numCur)*vertices1 [j + 1] - (10^numCur)*vertices1 [j],  (10^numCur)*vertices1 [j + 2]-(10^numCur)*vertices1 [j + 1] ).normalized)>1-delta2) {
                    //if (Vector3.Dot (Vector3.Cross (vertices1 [i + 1] - vertices1 [i], vertices1 [i +2] - vertices1 [i + 1]).normalized, Vector3.Cross (vertices1 [j + 1] - vertices1 [j], vertices1 [j + 2] - vertices1 [j + 1]).normalized)>1-delta2) {
                    if (Vector3.Dot(Vector3.Cross((numCur) * vertices1[i + 1] - (numCur) * vertices1[i], (numCur) * vertices1[i + 2] - (numCur) * vertices1[i + 1]).normalized, Vector3.Cross((numCur) * vertices1[j + 1] - (numCur) * vertices1[j], (numCur) * vertices1[j + 2] - (numCur) * vertices1[j + 1]).normalized) > 1 - delta2)
                    {
                        verticeIndices.Add(j);

                    }
                    if (j == vertices1.Count - 3)
                    {
                        Debug.Log("onSamePlane");
                        Debug.Log(verticeIndices.Count);
                        if (verticeIndices.Count > 1)
                        {
                            //三角形から直線を3つずつ追加
                            for (int k = 0; k < verticeIndices.Count; k++)
                            {
                                for (int l = 0; l < 3; l++)
                                {
                                    pVertices.Add(vertices1[verticeIndices[k] + l]);
                                    pNormals.Add(normals1[verticeIndices[k] + l]);
                                    pUvs.Add(uvs1[verticeIndices[k] + l]);

                                    pVertices.Add(vertices1[verticeIndices[k] + numRep(l + 1)]);
                                    pNormals.Add(normals1[verticeIndices[k] + numRep(l + 1)]);
                                    pUvs.Add(uvs1[verticeIndices[k] + numRep(l + 1)]);

                                }

                            }
                        }
                        //一致する直線(図形の外側に位置しない直線)を消去

                        for (int k = 0; k < pVertices.Count; k += 2)
                        {
                            for (int l = k + 2; l < pVertices.Count; l += 2)
                            {
                                if (((pVertices[l + 1] - pVertices[k]).magnitude < delta) && ((pVertices[l] - pVertices[k + 1]).magnitude < delta))
                                {
                                    pVertices.RemoveRange(l, 2);
                                    pVertices.RemoveRange(k, 2);
                                    pNormals.RemoveRange(l, 2);
                                    pNormals.RemoveRange(k, 2);
                                    pUvs.RemoveRange(l, 2);
                                    pUvs.RemoveRange(k, 2);
                                    k -= 2;

                                    Debug.Log("一致する直線を消去");

                                    avoidInfCount++;
                                    if (avoidInfCount > numLimit)
                                    {

                                        Debug.Log("error1");
                                        return;//kyousei
                                    }
                                    break;
                                }
                            }
                        }



                        for (int l = 0; l < pVertices.Count; l += 2)
                        {
                            for (int k = l + 2; k < pVertices.Count; k += 2)
                            {
                                //4つの頂点が一直線上にあるか
                                if (Vector3.Dot(((numCur) * pVertices[l] - (numCur) * pVertices[l + 1]).normalized, ((numCur) * pVertices[k] - (numCur) * pVertices[k + 1]).normalized) > 1 - delta2)
                                {
                                    //以下重なる点に応じた処理
                                    if ((pVertices[l + 1] - pVertices[k]).magnitude < delta)
                                    {

                                        //Debug.Log ("1done");

                                        pVertices.Add(pVertices[l]);
                                        pVertices.Add(pVertices[k + 1]);
                                        pNormals.Add(pNormals[l]);
                                        pNormals.Add(pNormals[k + 1]);
                                        pUvs.Add(pUvs[l]);
                                        pUvs.Add(pUvs[k + 1]);

                                        pVertices.RemoveRange(k, 2);
                                        pVertices.RemoveRange(l, 2);
                                        pNormals.RemoveRange(k, 2);
                                        pNormals.RemoveRange(l, 2);
                                        pUvs.RemoveRange(k, 2);
                                        pUvs.RemoveRange(l, 2);

                                        l -= 2;
                                        avoidInfCount++;
                                        if (avoidInfCount > numLimit)
                                        {

                                            Debug.Log("error2");
                                            return;//kyousei
                                        }
                                        break;
                                    }
                                    else if ((pVertices[l] - pVertices[k + 1]).magnitude < delta)
                                    {
                                        //	Debug.Log ("2done");
                                        pVertices.Add(pVertices[k]);
                                        pVertices.Add(pVertices[l + 1]);
                                        pNormals.Add(pNormals[k]);
                                        pNormals.Add(pNormals[l + 1]);
                                        pUvs.Add(pUvs[k]);
                                        pUvs.Add(pUvs[l + 1]);
                                        pVertices.RemoveRange(k, 2);
                                        pVertices.RemoveRange(l, 2);
                                        pNormals.RemoveRange(k, 2);
                                        pNormals.RemoveRange(l, 2);
                                        pUvs.RemoveRange(k, 2);
                                        pUvs.RemoveRange(l, 2);

                                        avoidInfCount++;
                                        if (avoidInfCount > numLimit)
                                        {

                                            Debug.Log("error3");
                                            return;//kyousei
                                        }
                                        l -= 2;
                                        break;
                                    }
                                    else if ((pVertices[l + 1] - pVertices[k + 1]).magnitude < delta)
                                    {
                                        //	Debug.Log ("3done");
                                        pVertices.Add(pVertices[l]);
                                        pVertices.Add(pVertices[k]);
                                        pNormals.Add(pNormals[l]);
                                        pNormals.Add(pNormals[k]);
                                        pUvs.Add(pUvs[l]);
                                        pUvs.Add(pUvs[k]);
                                        pVertices.RemoveRange(k, 2);
                                        pVertices.RemoveRange(l, 2);
                                        pNormals.RemoveRange(k, 2);
                                        pNormals.RemoveRange(l, 2);
                                        pUvs.RemoveRange(k, 2);
                                        pUvs.RemoveRange(l, 2);

                                        avoidInfCount++;
                                        if (avoidInfCount > numLimit)
                                        {

                                            Debug.Log("error4");
                                            return;//kyousei
                                        }
                                        l -= 2;
                                        break;
                                    }
                                    else if ((pVertices[l] - pVertices[k]).magnitude < delta)
                                    {
                                        //		Debug.Log ("4done");
                                        pVertices.Add(pVertices[k + 1]);
                                        pVertices.Add(pVertices[l + 1]);
                                        pNormals.Add(pNormals[k + 1]);
                                        pNormals.Add(pNormals[l + 1]);
                                        pUvs.Add(pUvs[k + 1]);
                                        pUvs.Add(pUvs[l + 1]);
                                        pVertices.RemoveRange(k, 2);
                                        pVertices.RemoveRange(l, 2);
                                        pNormals.RemoveRange(k, 2);
                                        pNormals.RemoveRange(l, 2);
                                        pUvs.RemoveRange(k, 2);
                                        pUvs.RemoveRange(l, 2);
                                        l -= 2;
                                        avoidInfCount++;
                                        if (avoidInfCount > numLimit)
                                        {

                                            Debug.Log("error5");
                                            return;//kyousei
                                        }
                                        break;
                                    }
                                }

                            }
                        }



                        //重なる点を消去
                        for (int k = 0; k < pVertices.Count; k++)
                        {
                            for (int l = k + 1; l < pVertices.Count; l++)
                            {
                                if ((pVertices[k] - pVertices[l]).magnitude < delta)
                                {
                                    pVertices.RemoveAt(l);
                                    pNormals.RemoveAt(l);
                                    pUvs.RemoveAt(l);
                                    avoidInfCount++;
                                    if (avoidInfCount > numLimit)
                                    {
                                        Debug.Log("errorD2");
                                        return;//kyousei
                                    }



                                }
                            }
                        }

                        //並べ替え
                        for (int k = 2; k < pVertices.Count; k++)
                        {
                            for (int l = k + 1; l < pVertices.Count; l++)
                            {
                                if (System.Math.Acos(Vector3.Dot((pVertices[0] - pVertices[1]).normalized, (pVertices[0] - pVertices[k]).normalized)) >= System.Math.Acos(Vector3.Dot((pVertices[0] - pVertices[1]).normalized, (pVertices[0] - pVertices[l]).normalized)))
                                {
                                    if (System.Math.Acos(Vector3.Dot((pVertices[0] - pVertices[1]).normalized, (pVertices[0] - pVertices[k]).normalized)) == System.Math.Acos(Vector3.Dot((pVertices[0] - pVertices[1]).normalized, (pVertices[0] - pVertices[l]).normalized)))
                                    {
                                        if ((pVertices[0] - pVertices[k]).magnitude > (pVertices[0] - pVertices[l]).magnitude)
                                        {

                                            pVertices.Insert(0, pVertices[l]);
                                            pVertices.RemoveAt(l + 1);
                                            pNormals.Insert(0, pNormals[l]);
                                            pNormals.RemoveAt(l + 1);
                                            pUvs.Insert(0, pUvs[l]);
                                            pUvs.RemoveAt(l + 1);
                                        }
                                        else
                                        {
                                            pVertices.Insert(0, pVertices[k]);
                                            pVertices.RemoveAt(k + 1);
                                            pNormals.Insert(0, pNormals[k]);
                                            pNormals.RemoveAt(k + 1);
                                            pUvs.Insert(0, pUvs[k]);
                                            pUvs.RemoveAt(k + 1);
                                        }

                                        avoidInfCount++;
                                        if (avoidInfCount > numLimit)
                                        {

                                            Debug.Log("error6-1-1");
                                            avoidInfCount = 0;
                                            return;//kyousei
                                        }
                                        k = 1;
                                        break;
                                    }

                                    pVertices.Insert(k, pVertices[l]);
                                    pVertices.RemoveAt(l + 1);
                                    pNormals.Insert(k, pNormals[l]);
                                    pNormals.RemoveAt(l + 1);
                                    pUvs.Insert(k, pUvs[l]);
                                    pUvs.RemoveAt(l + 1);
                                    avoidInfCount++;
                                    if (avoidInfCount > numLimit)
                                    {

                                        Debug.Log("error6-1-2");
                                        avoidInfCount = 0;
                                        return;//kyousei 
                                    }

                                    k = 1;

                                    break;
                                }
                            }

                        }
                        Debug.Log("	pVertices.Count");
                        Debug.Log(pVertices.Count);
                        //三角形形成
                        for (int k = 1; k < pVertices.Count - 1; k++)
                        {

                            vertices1.Insert(0, pVertices[0]);
                            normals1.Insert(0, pNormals[0]);
                            uvs1.Insert(0, pUvs[0]);
                            triangles1.Insert(0, (pVertices.Count - 2) * 3 - (3 * k));

                            vertices1.Insert(1, pVertices[k]);
                            normals1.Insert(1, pNormals[k]);
                            uvs1.Insert(1, pUvs[k]);
                            triangles1.Insert(1, (pVertices.Count - 2) * 3 - (3 * k - 1));

                            vertices1.Insert(2, pVertices[k + 1]);
                            normals1.Insert(2, pNormals[k + 1]);
                            uvs1.Insert(2, pUvs[k + 1]);
                            triangles1.Insert(2, (pVertices.Count - 2) * 3 - (3 * k - 2));

                            avoidInfCount++;
                            if (avoidInfCount > numLimit)
                            {
                                Debug.Log("errorD3");
                                return;//kyousei 
                            }
                        }
                        //	古い三角形を消す
                        for (int k = verticeIndices.Count - 1; k >= 0; k--)
                        {
                            vertices1.RemoveRange(verticeIndices[k] + 3 * (pVertices.Count - 2), 3);
                            normals1.RemoveRange(verticeIndices[k] + 3 * (pVertices.Count - 2), 3);
                            uvs1.RemoveRange(verticeIndices[k] + 3 * (pVertices.Count - 2), 3);
                            triangles1.RemoveRange(verticeIndices[k] + 3 * (pVertices.Count - 2), 3);
                            avoidInfCount++;
                            if (avoidInfCount > numLimit)
                            {
                                Debug.Log("errorD4");
                                return;//kyousei 
                            }
                        }

                        for (int k = 0; k < triangles1.Count; k++)
                        {
                            triangles1[k] = k;
                            //Debug.Log (triangles1 [k]);

                        }

                        pVertices.Clear();
                        pNormals.Clear();
                        pUvs.Clear();
                        i = vertices1.Count;
                        break;

                    }
                }
            }


            for (int i = 0; i < vertices2.Count; i += 3)
            {

                verticeIndices.Clear();
                verticeIndices.Add(i);
                Debug.Log("---------------------------------");
                for (int j = i + 3; j < vertices2.Count; j += 3)
                {
                    //同一の平面上にある三角形かどうか。数値計算のためdeltaで誤差を考慮する
                    //	Debug.Log(Vector3.Dot (Vector3.Cross (vertices2 [i + 1] - vertices2 [i], vertices2 [i + 1] - vertices2 [i + 2]).normalized, Vector3.Cross (vertices2 [j + 1] - vertices2 [j], vertices2 [j + 1] - vertices2 [j + 2]).normalized));
                    //	Debug.Log(Vector3.Dot (Vector3.Cross (vertices2 [i + 1] - vertices2 [i], vertices2 [i +2] - vertices2 [i + 1]).normalized, Vector3.Cross (vertices2 [j + 1] - vertices2 [j], vertices2 [j + 2] - vertices2 [j + 1]).normalized));
                    //if (Vector3.Dot (Vector3.Cross ((10^numCur)*vertices2 [i + 1] - (10^numCur)*vertices2 [i],  (10^numCur)*vertices2 [i + 2]-(10^numCur)*vertices2 [i + 1]).normalized, Vector3.Cross ((10^numCur)*vertices2 [j + 1] - (10^numCur)*vertices2 [j],  (10^numCur)*vertices2 [j + 2]-(10^numCur)*vertices2 [j + 1] ).normalized)>1-delta2) {
                    //if (Vector3.Dot (Vector3.Cross (vertices2 [i + 1] - vertices2 [i], vertices2 [i +2] - vertices2 [i + 1]).normalized, Vector3.Cross (vertices2 [j + 1] - vertices2 [j], vertices2 [j + 2] - vertices2 [j + 1]).normalized)>1-delta2) {
                    if (Vector3.Dot(Vector3.Cross((numCur) * vertices2[i + 1] - (numCur) * vertices2[i], (numCur) * vertices2[i + 2] - (numCur) * vertices2[i + 1]).normalized, Vector3.Cross((numCur) * vertices2[j + 1] - (numCur) * vertices2[j], (numCur) * vertices2[j + 2] - (numCur) * vertices2[j + 1]).normalized) > 1 - delta2)
                    {
                        verticeIndices.Add(j);

                    }
                    if (j == vertices2.Count - 3)
                    {
                        Debug.Log("onSamePlane");
                        Debug.Log(verticeIndices.Count);
                        if (verticeIndices.Count > 1)
                        {
                            //三角形から直線を3つずつ追加
                            for (int k = 0; k < verticeIndices.Count; k++)
                            {
                                for (int l = 0; l < 3; l++)
                                {
                                    pVertices.Add(vertices2[verticeIndices[k] + l]);
                                    pNormals.Add(normals2[verticeIndices[k] + l]);
                                    pUvs.Add(uvs2[verticeIndices[k] + l]);

                                    pVertices.Add(vertices2[verticeIndices[k] + numRep(l + 1)]);
                                    pNormals.Add(normals2[verticeIndices[k] + numRep(l + 1)]);
                                    pUvs.Add(uvs2[verticeIndices[k] + numRep(l + 1)]);

                                }

                            }
                        }
                        //一致する直線(図形の外側に位置しない直線)を消去

                        for (int k = 0; k < pVertices.Count; k += 2)
                        {
                            for (int l = k + 2; l < pVertices.Count; l += 2)
                            {
                                if (((pVertices[l + 1] - pVertices[k]).magnitude < delta) && ((pVertices[l] - pVertices[k + 1]).magnitude < delta))
                                {
                                    pVertices.RemoveRange(l, 2);
                                    pVertices.RemoveRange(k, 2);
                                    pNormals.RemoveRange(l, 2);
                                    pNormals.RemoveRange(k, 2);
                                    pUvs.RemoveRange(l, 2);
                                    pUvs.RemoveRange(k, 2);
                                    k -= 2;

                                    Debug.Log("一致する直線を消去");

                                    avoidInfCount++;
                                    if (avoidInfCount > numLimit)
                                    {

                                        Debug.Log("error1");
                                        return;//kyousei
                                    }
                                    break;
                                }
                            }
                        }



                        for (int l = 0; l < pVertices.Count; l += 2)
                        {
                            for (int k = l + 2; k < pVertices.Count; k += 2)
                            {
                                //4つの頂点が一直線上にあるか
                                if (Vector3.Dot(((numCur) * pVertices[l] - (numCur) * pVertices[l + 1]).normalized, ((numCur) * pVertices[k] - (numCur) * pVertices[k + 1]).normalized) > 1 - delta2)
                                {
                                    //以下重なる点に応じた処理
                                    if ((pVertices[l + 1] - pVertices[k]).magnitude < delta)
                                    {

                                        //Debug.Log ("1done");

                                        pVertices.Add(pVertices[l]);
                                        pVertices.Add(pVertices[k + 1]);
                                        pNormals.Add(pNormals[l]);
                                        pNormals.Add(pNormals[k + 1]);
                                        pUvs.Add(pUvs[l]);
                                        pUvs.Add(pUvs[k + 1]);

                                        pVertices.RemoveRange(k, 2);
                                        pVertices.RemoveRange(l, 2);
                                        pNormals.RemoveRange(k, 2);
                                        pNormals.RemoveRange(l, 2);
                                        pUvs.RemoveRange(k, 2);
                                        pUvs.RemoveRange(l, 2);

                                        l -= 2;
                                        avoidInfCount++;
                                        if (avoidInfCount > numLimit)
                                        {

                                            Debug.Log("error2");
                                            return;//kyousei
                                        }
                                        break;
                                    }
                                    else if ((pVertices[l] - pVertices[k + 1]).magnitude < delta)
                                    {
                                        //	Debug.Log ("2done");
                                        pVertices.Add(pVertices[k]);
                                        pVertices.Add(pVertices[l + 1]);
                                        pNormals.Add(pNormals[k]);
                                        pNormals.Add(pNormals[l + 1]);
                                        pUvs.Add(pUvs[k]);
                                        pUvs.Add(pUvs[l + 1]);
                                        pVertices.RemoveRange(k, 2);
                                        pVertices.RemoveRange(l, 2);
                                        pNormals.RemoveRange(k, 2);
                                        pNormals.RemoveRange(l, 2);
                                        pUvs.RemoveRange(k, 2);
                                        pUvs.RemoveRange(l, 2);

                                        avoidInfCount++;
                                        if (avoidInfCount > numLimit)
                                        {

                                            Debug.Log("error3");
                                            return;//kyousei
                                        }
                                        l -= 2;
                                        break;
                                    }
                                    else if ((pVertices[l + 1] - pVertices[k + 1]).magnitude < delta)
                                    {
                                        //	Debug.Log ("3done");
                                        pVertices.Add(pVertices[l]);
                                        pVertices.Add(pVertices[k]);
                                        pNormals.Add(pNormals[l]);
                                        pNormals.Add(pNormals[k]);
                                        pUvs.Add(pUvs[l]);
                                        pUvs.Add(pUvs[k]);
                                        pVertices.RemoveRange(k, 2);
                                        pVertices.RemoveRange(l, 2);
                                        pNormals.RemoveRange(k, 2);
                                        pNormals.RemoveRange(l, 2);
                                        pUvs.RemoveRange(k, 2);
                                        pUvs.RemoveRange(l, 2);

                                        avoidInfCount++;
                                        if (avoidInfCount > numLimit)
                                        {

                                            Debug.Log("error4");
                                            return;//kyousei
                                        }
                                        l -= 2;
                                        break;
                                    }
                                    else if ((pVertices[l] - pVertices[k]).magnitude < delta)
                                    {
                                        //		Debug.Log ("4done");
                                        pVertices.Add(pVertices[k + 1]);
                                        pVertices.Add(pVertices[l + 1]);
                                        pNormals.Add(pNormals[k + 1]);
                                        pNormals.Add(pNormals[l + 1]);
                                        pUvs.Add(pUvs[k + 1]);
                                        pUvs.Add(pUvs[l + 1]);
                                        pVertices.RemoveRange(k, 2);
                                        pVertices.RemoveRange(l, 2);
                                        pNormals.RemoveRange(k, 2);
                                        pNormals.RemoveRange(l, 2);
                                        pUvs.RemoveRange(k, 2);
                                        pUvs.RemoveRange(l, 2);
                                        l -= 2;
                                        avoidInfCount++;
                                        if (avoidInfCount > numLimit)
                                        {

                                            Debug.Log("error5");
                                            return;//kyousei
                                        }
                                        break;
                                    }
                                }

                            }
                        }



                        //重なる点を消去
                        for (int k = 0; k < pVertices.Count; k++)
                        {
                            for (int l = k + 1; l < pVertices.Count; l++)
                            {
                                if ((pVertices[k] - pVertices[l]).magnitude < delta)
                                {
                                    pVertices.RemoveAt(l);
                                    pNormals.RemoveAt(l);
                                    pUvs.RemoveAt(l);
                                    avoidInfCount++;
                                    if (avoidInfCount > numLimit)
                                    {
                                        Debug.Log("errorD2");
                                        return;//kyousei
                                    }



                                }
                            }
                        }

                        //並べ替え
                        for (int k = 2; k < pVertices.Count; k++)
                        {
                            for (int l = k + 1; l < pVertices.Count; l++)
                            {
                                if (System.Math.Acos(Vector3.Dot((pVertices[0] - pVertices[1]).normalized, (pVertices[0] - pVertices[k]).normalized)) >= System.Math.Acos(Vector3.Dot((pVertices[0] - pVertices[1]).normalized, (pVertices[0] - pVertices[l]).normalized)))
                                {
                                    if (System.Math.Acos(Vector3.Dot((pVertices[0] - pVertices[1]).normalized, (pVertices[0] - pVertices[k]).normalized)) == System.Math.Acos(Vector3.Dot((pVertices[0] - pVertices[1]).normalized, (pVertices[0] - pVertices[l]).normalized)))
                                    {
                                        if ((pVertices[0] - pVertices[k]).magnitude > (pVertices[0] - pVertices[l]).magnitude)
                                        {

                                            pVertices.Insert(0, pVertices[l]);
                                            pVertices.RemoveAt(l + 1);
                                            pNormals.Insert(0, pNormals[l]);
                                            pNormals.RemoveAt(l + 1);
                                            pUvs.Insert(0, pUvs[l]);
                                            pUvs.RemoveAt(l + 1);
                                        }
                                        else
                                        {
                                            pVertices.Insert(0, pVertices[k]);
                                            pVertices.RemoveAt(k + 1);
                                            pNormals.Insert(0, pNormals[k]);
                                            pNormals.RemoveAt(k + 1);
                                            pUvs.Insert(0, pUvs[k]);
                                            pUvs.RemoveAt(k + 1);
                                        }

                                        avoidInfCount++;
                                        if (avoidInfCount > numLimit)
                                        {

                                            Debug.Log("error6-1-1");
                                            avoidInfCount = 0;
                                            return;//kyousei
                                        }
                                        k = 1;
                                        break;
                                    }

                                    pVertices.Insert(k, pVertices[l]);
                                    pVertices.RemoveAt(l + 1);
                                    pNormals.Insert(k, pNormals[l]);
                                    pNormals.RemoveAt(l + 1);
                                    pUvs.Insert(k, pUvs[l]);
                                    pUvs.RemoveAt(l + 1);
                                    avoidInfCount++;
                                    if (avoidInfCount > numLimit)
                                    {

                                        Debug.Log("error6-1-2");
                                        avoidInfCount = 0;
                                        return;//kyousei 
                                    }

                                    k = 1;

                                    break;
                                }
                            }

                        }
                        Debug.Log("	pVertices.Count");
                        Debug.Log(pVertices.Count);
                        //三角形形成
                        for (int k = 1; k < pVertices.Count - 1; k++)
                        {

                            vertices2.Insert(0, pVertices[0]);
                            normals2.Insert(0, pNormals[0]);
                            uvs2.Insert(0, pUvs[0]);
                            triangles2.Insert(0, (pVertices.Count - 2) * 3 - (3 * k));

                            vertices2.Insert(1, pVertices[k]);
                            normals2.Insert(1, pNormals[k]);
                            uvs2.Insert(1, pUvs[k]);
                            triangles2.Insert(1, (pVertices.Count - 2) * 3 - (3 * k - 1));

                            vertices2.Insert(2, pVertices[k + 1]);
                            normals2.Insert(2, pNormals[k + 1]);
                            uvs2.Insert(2, pUvs[k + 1]);
                            triangles2.Insert(2, (pVertices.Count - 2) * 3 - (3 * k - 2));

                            avoidInfCount++;
                            if (avoidInfCount > numLimit)
                            {
                                Debug.Log("errorD3");
                                return;//kyousei 
                            }
                        }
                        //	古い三角形を消す
                        for (int k = verticeIndices.Count - 1; k >= 0; k--)
                        {
                            vertices2.RemoveRange(verticeIndices[k] + 3 * (pVertices.Count - 2), 3);
                            normals2.RemoveRange(verticeIndices[k] + 3 * (pVertices.Count - 2), 3);
                            uvs2.RemoveRange(verticeIndices[k] + 3 * (pVertices.Count - 2), 3);
                            triangles2.RemoveRange(verticeIndices[k] + 3 * (pVertices.Count - 2), 3);
                            avoidInfCount++;
                            if (avoidInfCount > numLimit)
                            {
                                Debug.Log("errorD4");
                                return;//kyousei 
                            }
                        }

                        for (int k = 0; k < triangles2.Count; k++)
                        {
                            triangles2[k] = k;
                            //Debug.Log (triangles2 [k]);

                        }

                        pVertices.Clear();
                        pNormals.Clear();
                        pUvs.Clear();
                        i = vertices2.Count;
                        break;

                    }
                }
            }

        }






		
		uvs1.Clear();
		uvs2.Clear();

		for (int i = 0; i < vertices1.Count; i += 3)
        {
            uvs1.Add(new Vector2(0, 0));
            uvs1.Add(new Vector2(0, 1));
            uvs1.Add(new Vector2(1, 0));
            
        }

        for (int i = 0; i < vertices2.Count; i += 3)
        {
            uvs2.Add(new Vector2(0, 0));
            uvs2.Add(new Vector2(0, 1));
            uvs2.Add(new Vector2(1, 0));
           
        }




        GameObject obj = new GameObject("cut obj", typeof(MeshFilter), typeof(MeshRenderer));
        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();
        mesh.triangles = triangles1.ToArray();
        mesh.uv = uvs1.ToArray();
        mesh.normals = normals1.ToArray();
        obj.GetComponent<MeshFilter>().mesh = mesh;
        obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        obj.tag = "obj";

        GameObject obj2 = new GameObject("cut obj", typeof(MeshFilter), typeof(MeshRenderer));
        var mesh2 = new Mesh();
        mesh2.vertices = vertices2.ToArray();
        mesh2.triangles = triangles2.ToArray();
        mesh2.uv = uvs2.ToArray();
        mesh2.normals = normals2.ToArray();
        obj2.GetComponent<MeshFilter>().mesh = mesh2;
        obj2.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        obj2.tag = "obj";
        // obj.transform.position = transform.position;
        // obj.transform.rotation = transform.rotation;
        // obj.transform.localScale = transform.localScale;
        // //	obj.GetComponent<MeshCut>().Start();

        // GameObject obj2 = new GameObject ("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshCut));

        // var mesh2 = new Mesh ();
        // mesh2.vertices = vertices2.ToArray ();
        // mesh2.triangles = triangles2.ToArray ();
        // mesh2.uv = uvs2.ToArray ();
        // mesh2.normals = normals2.ToArray ();
        // obj2.GetComponent<MeshFilter> ().mesh = mesh2;
        // obj2.GetComponent<MeshRenderer> ().materials = GetComponent<MeshRenderer> ().materials;
        // obj2.GetComponent<MeshCollider> ().sharedMesh = mesh2;
        // obj2.GetComponent<MeshCollider> ().inflateMesh = true;
        // obj2.GetComponent<MeshCollider> ().skinWidth = skinWidth;
        // obj2.GetComponent<MeshCollider> ().convex = true;
        // obj2.GetComponent<MeshCollider> ().material = GetComponent<Collider> ().material;
        // obj2.transform.position = transform.position;
        // obj2.transform.rotation = transform.rotation;
        // obj2.transform.localScale = transform.localScale;
        // obj2.GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody> ().velocity;
        // obj2.GetComponent<Rigidbody> ().angularVelocity = GetComponent<Rigidbody> ().angularVelocity;
        // obj2.GetComponent<MeshCut> ().skinWidth = skinWidth;
        // obj2.tag = "obj";
        // //	obj2.GetComponent<MeshCut>().Start();

        //Destroy (gameObject);


    }

    int numRep(int i)
    {
        if (i % 3 == 0)
        {
            return 0;
        }
        else if (i % 3 == 1)
        {
            return 1;
        }
        else if (i % 3 == 2)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

}
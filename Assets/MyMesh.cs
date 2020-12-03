using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class MyMesh : MonoBehaviour {
    public Slider N_Slider;
    public Slider M_Slider;
    public Slider Rotation;
    public int size;
    int pre_N;
    int pre_M;
    int pre_D;
    int pre_selection;
    int tri_amount;
    int[] t;
	// Use this for initialization
	void Start () {
        size = 5;
        pre_N = (int)N_Slider.GetComponent<Slider>().value;
        pre_M = (int)M_Slider.GetComponent<Slider>().value;
        pre_D = (int)Rotation.GetComponent<Slider>().value;
        initMesh(pre_N, pre_M);
        /*Mesh theMesh = GetComponent<MeshFilter>().mesh;   // get the mesh component
        theMesh.Clear();    // delete whatever is there!!

        Vector3[] v = new Vector3[9];   // 2x2 mesh needs 3x3 vertices
        int[] t = new int[8*3];         // Number of triangles: 2x2 mesh and 2x triangles on each mesh-unit
        Vector3[] n = new Vector3[9];   // MUST be the same as number of vertices


        v[0] = new Vector3( -5, 0, -5);
        v[1] = new Vector3( 0, 0, -5);
        v[2] = new Vector3( 5, 0, -5);

        v[3] = new Vector3(-5, 0, 0);
        v[4] = new Vector3( 0, 0, 0);
        v[5] = new Vector3( 5, 0, 0);

        v[6] = new Vector3(-5, 0, 5);
        v[7] = new Vector3( 0, 0, 5);
        v[8] = new Vector3( 5, 0, 5);

        n[0] = new Vector3(0, 1, 0);
        n[1] = new Vector3(0, 1, 0);
        n[2] = new Vector3(0, 1, 0);
        n[3] = new Vector3(0, 1, 0);
        n[4] = new Vector3(0, 1, 0);
        n[5] = new Vector3(0, 1, 0);
        n[6] = new Vector3(0, 1, 0);
        n[7] = new Vector3(0, 1, 0);
        n[8] = new Vector3(0, 1, 0);

        // First triangle
        t[0] = 0; t[1] = 3; t[2] = 4;  // 0th triangle
        t[3] = 0; t[4] = 4; t[5] = 1;  // 1st triangle

        t[6] = 1; t[7] = 4; t[8] = 5;  // 2nd triangle
        t[9] = 1; t[10] = 5; t[11] = 2;  // 3rd triangle

        t[12] = 3; t[13] = 6; t[14] = 7;  // 4th triangle
        t[15] = 3; t[16] = 7; t[17] = 4;  // 5th triangle

        t[18] = 4; t[19] = 7; t[20] = 8;  // 6th triangle
        t[21] = 4; t[22] = 8; t[23] = 5;  // 7th triangle

        theMesh.vertices = v; //  new Vector3[3];
        theMesh.triangles = t; //  new int[3];
        theMesh.normals = n;

        InitControllers(v);
        InitNormals(v, n);*/
    }

    // Update is called once per frame
    void Update () {
        // Quad Mesh
        int N = (int)N_Slider.GetComponent<Slider>().value;
        int M = (int)M_Slider.GetComponent<Slider>().value;
        int D = (int)Rotation.GetComponent<Slider>().value;
        // Mesh scale changed, reset everything
        if (N != pre_N || M != pre_M || D!=pre_D)
        {
            Debug.Log("reset");
            pre_N = N;
            pre_M = M;
            pre_D = D;
            GetComponent<CTRL>().enable = true;
            GetComponent<PointSelect>().SelectPoint.SetActive(false);
            destroyAllChildren();
            initMesh(N, M);
        }

        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] v = theMesh.vertices;
        Vector3[] n = theMesh.normals;
        for (int i = 0; i < mControllers.Length; i++)
        {
            v[i] = mControllers[i].transform.localPosition;
        }

        ComputeNormals(v, n);

        theMesh.vertices = v;
        theMesh.normals = n;
    }

    void initMesh(int N, int M)
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;   // get the mesh component
        theMesh.Clear();    // delete whatever is there!!
        Vector3[] v = new Vector3[N*M];   // 2x2 mesh needs 3x3 vertices
        if (Rotation.GetComponent<Slider>().value == 360&&this.name=="MyCylinder")
        {
            tri_amount = (N - 1) * 2 * (M - 1) + (M - 1) * 2;
        }
        else
        {
            tri_amount = (N - 1) * 2 * (M - 1); 
        }
        t = new int[tri_amount * 3]; // Number of triangles: 2x2 mesh and 2x triangles on each mesh-unit
        Vector3[] n = new Vector3[N*M];   // MUST be the same as number of vertices

        // Initialize normals
        for (int i = 0; i < N * M; i++)
        {
            n[i] = new Vector3(0, 1, 0);
        }
        float length = 2 * size;
        float N_dif = length / (N - 1);
        float M_dif = length / (M - 1);
        int index = 0;
        // Initialize vertices
        if (this.name == "MyMesh")
        {
            for (int m1 = 0; m1 < M; m1++)
            {
                for (int n1 = 0; n1 < N; n1++)
                {
                    v[index] = new Vector3(-size + n1 * N_dif, 0, -size + m1 * M_dif);
                    index++;
                }
            }
        }
        else if (this.name == "MyCylinder")
        {
            int kNum = N;
            float degree = Rotation.GetComponent<Slider>().value;
            float kdTheta = (degree / kNum) * Mathf.Deg2Rad;
            Vector3 p;
            float KRadius = 3;
            float delta = 2;
            for (int h = 0; h < M; h++)
            {
                float yVal = h * delta;
                for(int i = 0; i<kNum; i++)
                {
                    p.x = KRadius * Mathf.Cos(i * kdTheta);
                    p.y = yVal;
                    p.z = KRadius * Mathf.Sin(i * kdTheta);
                    v[index] = p;
                    index++;
                }
            }
        }

        // Initialize triangles
        int stop = N * M - N - 1; // for example, 3*3 will stop before 5;
        int cur_index = 0;
        int counter = 0;
        index = 0;
        while (cur_index < stop)
        {
            if (counter == N-1)
            {
                counter = 0;
                cur_index++;
                continue;
            }
            // connect triangle from bottom left, each point will have 2 corresponding triangles
            // comments below are examples for 1st iteration
            t[index] = cur_index;             // t[0] = 0
            t[index + 1] = cur_index + N;     // t[1] = 3
            t[index + 2] = cur_index + N + 1; // t[2] = 4 

            t[index + 3] = cur_index;         // t[3] = 0
            t[index + 4] = cur_index + N + 1; // t[4] = 4
            t[index + 5] = cur_index + 1;     // t[5] = 1
            index+=6;
            cur_index++;
            counter++;
        }
        if (Rotation.GetComponent<Slider>().value == 360 && this.name == "MyCylinder")
        {
            counter = 0;
            while (counter < M-1)
            {
                t[index] = counter*N;
                t[index + 1] = (counter+1) * N;
                t[index + 2] = (counter + 2) * N - 1;

                t[index + 3] = counter*N;
                t[index + 4] = (counter+1) * N - 1;
                t[index + 5] = (counter + 2) * N - 1;
                counter++;
                index += 6;
            }
        }
        theMesh.vertices = v; //  new Vector3[3];
        theMesh.triangles = t; //  new int[3];
        theMesh.normals = n;

        InitControllers(v);
        InitNormals(v, n);
    }

    

    void destroyAllChildren()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

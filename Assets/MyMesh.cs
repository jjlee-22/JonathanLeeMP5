using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class MyMesh : MonoBehaviour
{
    public Slider N_Slider;
    public Slider M_Slider;
    public Slider Rotation;
    public Dropdown l;
    public int size;
    int pre_N;
    int pre_M;
    int pre_D;
    int pre_l;
    int N;
    int M;
    int D;
    int L;
    int pre_selection;
    int tri_amount;
    int[] t;
    private Transform UVtransform;
    public XFormControl formcontrol;
    // Use this for initialization
    void Start()
    {
        size = 5;
        pre_N = (int)N_Slider.GetComponent<Slider>().value;
        pre_M = (int)M_Slider.GetComponent<Slider>().value;
        pre_D = (int)Rotation.GetComponent<Slider>().value;
        pre_l = (int)l.GetComponent<Dropdown>().value;

        initMesh(pre_N, pre_M);

        UVtransform = new GameObject("Texture").transform;

        if (gameObject.name == "MyMesh")
        {
            formcontrol.SetSelectedObject(UVtransform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Quad Mesh
        N = (int)N_Slider.GetComponent<Slider>().value;
        M = (int)M_Slider.GetComponent<Slider>().value;
        D = (int)Rotation.GetComponent<Slider>().value;
        L = l.GetComponent<Dropdown>().value;
        // Mesh scale changed, reset everything
        if (N != pre_N || M != pre_M || D != pre_D || L!=pre_l)
        {
            Debug.Log("reset");
            pre_N = N;
            pre_M = M;
            pre_D = D;
            pre_l = L;
            GetComponent<CTRL>().enable = true;
            GetComponent<PointSelect>().SelectPoint.SetActive(false);
            destroyAllChildren();
            initMesh(N, M);
        }

        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] v = theMesh.vertices;
        Vector3[] n = theMesh.normals;
        Vector2[] uv = theMesh.uv; //UV Mapping
        ChangeTextureTransform(uv);

        for (int i = 0; i < mControllers.Length; i++)
        {
            v[i] = mControllers[i].transform.localPosition;
        }

        ComputeNormals(v, n);

        theMesh.vertices = v;
        theMesh.normals = n;
        theMesh.uv = uv;
    }

    void initMesh(int N, int M)
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;   // get the mesh component
        theMesh.Clear();    // delete whatever is there!!

        Vector3[] v = new Vector3[N * M];   // 2x2 mesh needs 3x3 vertices
        if (Rotation.GetComponent<Slider>().value == 360 && this.name == "MyCylinder")
        {
            tri_amount = (N - 1) * 2 * (M - 1) + (M - 1) * 2;
        }
        else
        {
            tri_amount = (N - 1) * 2 * (M - 1);
        }
        t = new int[tri_amount * 3]; // Number of triangles: 2x2 mesh and 2x triangles on each mesh-unit
        Vector3[] n = new Vector3[N * M];   // MUST be the same as number of vertices
        Vector2[] uv = new Vector2[N * M];  // UV vectors for the mesh

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
                for (int i = 0; i < kNum; i++)
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
            if (counter == N - 1)
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
            index += 6;
            cur_index++;
            counter++;
        }
        if (Rotation.GetComponent<Slider>().value == 360 && this.name == "MyCylinder")
        {
            counter = 0;
            while (counter < M - 1)
            {
                t[index] = counter * N;
                t[index + 1] = (counter + 1) * N;
                t[index + 2] = (counter + 2) * N - 1;

                t[index + 3] = counter * N;
                t[index + 4] = (counter + 1) * N - 1;
                t[index + 5] = (counter + 2) * N - 1;
                counter++;
                index += 6;
            }
        }
        theMesh.vertices = v; //  new Vector3[3];
        theMesh.triangles = t; //  new int[3];
        theMesh.normals = n;
        theMesh.uv = uv;

        InitControllers(v);
        InitNormals(v, n);
        ComputeUV(uv);
    }



    void destroyAllChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void ChangeTextureTransform(Vector2[] uv)
    {
        // Calculate the texture transform using the given helpers
        Matrix3x3 translation = Matrix3x3Helpers.CreateTranslation(new Vector2(UVtransform.localPosition.x, UVtransform.localPosition.y));
        Matrix3x3 scale = Matrix3x3Helpers.CreateScale(new Vector2(UVtransform.localScale.x, UVtransform.localScale.y));
        Matrix3x3 rotation = Matrix3x3Helpers.CreateRotation(UVtransform.localRotation.eulerAngles.z);
        Matrix3x3 m = translation * rotation * scale;

        ComputeUV(uv);

        for (int i = 0; i < uv.Length; ++i)
            uv[i] = Matrix3x3.MultiplyVector2(m, uv[i]);
    }

    void ComputeUV(Vector2[] uv)
    {
        float num = 2f / (float)((N * M) - 1);
        int vertexIndex = 0;
        for (int y = 0; y < N; ++y)
        {
            for (int x = 0; x < M; ++x)
            {
                uv[vertexIndex] = new Vector2((float)((double)x * (double)num * 0.5), (float)((double)y * (double)num * 0.5));
                vertexIndex++;
            }
        }
    }
}

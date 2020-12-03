using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshCylinderSelect : MonoBehaviour
{
    public static int selection;
    public GameObject MyMesh;
    public GameObject MyCylinder;
    // Start is called before the first frame update
    void Start()
    {
        selection = GetComponent<Dropdown>().value;
    }

    // Update is called once per frame
    void Update()
    {
        selection = GetComponent<Dropdown>().value;
        if (selection == 0)
        {
            MyMesh.SetActive(true);
            MyCylinder.SetActive(false);
        }
        else
        {
            MyMesh.SetActive(false);
            MyCylinder.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Portal : MonoBehaviour
{

    [Header("Jago")]
    [SerializeField] private GameObject jago;

    [Header("Materials")]
    [SerializeField] private Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var mat in materials)
        {
            mat.SetInt("stest", (int)CompareFunction.Equal);
        }
    }


    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag!="MainCamera")
        {
            return;
        }


        Vector3 user_position = Camera.main.transform.position+Camera.main.transform.forward*Camera.main.nearClipPlane;
        Vector3 relativePosition = transform.InverseTransformPoint(user_position);
         
        

        //outside
        if (relativePosition.z<0)
        {
            jago.SetActive(true);

            foreach (var mat in materials)
            {
                mat.SetInt("stest",(int)CompareFunction.Equal);
            }
        }
        //inside
        else
        {
            jago.SetActive(false);

            foreach (var mat in materials)
            {
                mat.SetInt("stest", (int)CompareFunction.NotEqual);
            }
        } 



    }


}

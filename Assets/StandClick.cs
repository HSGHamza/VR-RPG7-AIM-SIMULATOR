using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.TechDemo.Scripts;
public class StandClick : MonoBehaviour
{
    private DemoUIManager demo;
    // Start is called before the first frame update
    void Start()
    {
        demo.OnSitStandClicked();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

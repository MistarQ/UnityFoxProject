using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraControll : MonoBehaviour
{
    public Transform target;

    public void SetTarget(Transform target) {
        this.target = target;
    }


    // Update is called once per frame
    void Update()
    {   
            if (target!=null) {
            transform.position = new Vector3(target.transform.position.x, 0f, -15f);
        }
            
    }
}   

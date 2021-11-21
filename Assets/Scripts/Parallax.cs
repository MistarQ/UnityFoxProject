using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public Transform Cam;
    public float MoveRate;
    private float startPointX,startPointY;
    public bool lockY;//false;

    // Start is called before the first frame update
    void Start()
    {
        startPointX = transform.position.x;
        startPointY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockY)
        {
            transform.position = new Vector2(startPointX + Cam.position.x * MoveRate, transform.position.y);
        }
        else {
            transform.position = new Vector2(startPointX + Cam.position.x * MoveRate, startPointY +Cam.position.y * MoveRate);
        }
    }
}

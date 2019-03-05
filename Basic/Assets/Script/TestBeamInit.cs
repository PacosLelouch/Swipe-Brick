using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBeamInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Transform>().GetChild(0).GetComponent<LineRenderer>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () { 
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.GetComponent<Transform>().Rotate(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.GetComponent<Transform>().Rotate(0, 1, 0);
        }
        /*
        this.GetComponent<Transform>().position = new Vector3(translateX, 0, 0);
        Debug.Log(this.GetComponent<Transform>().localPosition.ToString());
        float yAngle = 0;
        if (Input.mousePosition.z > 0)
        {
            yAngle = Mathf.Atan2(Input.mousePosition.x, Input.mousePosition.z);
        }
        this.GetComponent<Transform>().rotation = Quaternion.FromToRotation(new Vector3(0, 0, 0), new Vector3(0, yAngle, 0));
        */
    }

    //float translateX;
    public GameObject ball;
}

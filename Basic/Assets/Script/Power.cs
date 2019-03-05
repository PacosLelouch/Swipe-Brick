using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour {

    enum PowerMode {Normal, Double, Half };

    void Awake()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Launcher>();
    }
    // Use this for initialization
    void Start () {
        float randNum = Random.Range(0, 30);
        if(randNum < 3 && playerScript.TotalPower >= 30)
        {
            powerMode = PowerMode.Double;
        }
        else if(randNum < 8 && playerScript.TotalPower >= 60)
        {
            powerMode = PowerMode.Half;
        }
        else
        {
            powerMode = PowerMode.Normal;
        }
        this.GetComponent<MeshRenderer>().material = this.materials[(int)powerMode];
	}
	
	// Update is called once per frame
	void Update () {
        if(this.GetComponent<Transform>().position.z <= -15)
        {
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            switch (powerMode)
            {
                case PowerMode.Normal:
                    playerScript.NextPower += 1;
                    break;
                case PowerMode.Double:
                    playerScript.NextPower *= 2;
                    break;
                case PowerMode.Half:
                    playerScript.NextPower = (playerScript.NextPower + 1)/ 2;
                    break;
            }
            Destroy(this.gameObject);
        }
    }

    public List<Material> materials;
    PowerMode powerMode;
    GameObject player;
    Launcher playerScript;
}

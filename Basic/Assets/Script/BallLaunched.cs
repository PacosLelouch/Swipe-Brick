using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLaunched : MonoBehaviour {

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Launcher>();
        power = 1;
    }

    // Use this for initialization
    void Start() {
        this.GetComponent<MeshRenderer>().material = this.materials[(int)mode];
        this.moveOkay = false;
        this.isLanding = false;
        this.rigid = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate()
    {
        if (playerScript.IsFirstLanding && this.GetComponent<MeshRenderer>().enabled && this.isLanding && Vector3.SqrMagnitude(this.playerScript.NextPos - this.GetComponent<Transform>().position) < this.GetComponent<Transform>().localScale.z * this.GetComponent<Transform>().localScale.z)
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.moveOkay = true;
            this.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Ground")
        {
            this.isLanding = true;
            this.GetComponent<Transform>().position = new Vector3(this.GetComponent<Transform>().position.x, Launcher.originalPos.y, Launcher.originalPos.z);
            if (!playerScript.IsFirstLanding)
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.playerScript.NextPos = this.GetComponent<Transform>().position;
                this.playerScript.IsFirstLanding = true;
                
            }
            else if (Vector3.SqrMagnitude(this.playerScript.NextPos - this.GetComponent<Transform>().position) < this.GetComponent<Transform>().localScale.z * this.GetComponent<Transform>().localScale.z)
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.GetComponent<MeshRenderer>().enabled = false;
                this.moveOkay = true;
            }
            else
            {
                this.GetComponent<Rigidbody>().velocity = (this.playerScript.NextPos - this.GetComponent<Transform>().position).normalized * velRate;
            }
            playerScript.LandingCount += 1;
            //Destroy(this.gameObject);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (this.isLanding)
        {
            return;
        }
        if (this.rigid.velocity.z >= 0 && this.rigid.velocity.z < 1)
        {
            this.rigid.velocity = new Vector3(this.rigid.velocity.x, this.rigid.velocity.y, 1);
        }
        if (this.rigid.velocity.z < 0 && this.rigid.velocity.z > -1)
        {
            this.rigid.velocity = new Vector3(this.rigid.velocity.x, this.rigid.velocity.y, -1);
        }
    }

    public int Power
    {
        get
        {
            return this.power;
        }
        set
        {
            this.power = value;
        }
    }
    public Launcher.LaunchMode LaunchMode
    {
        set
        {
            this.mode = value;
        }
        get
        {
            return this.mode;
        }
    }

    public bool MoveOkay
    {
        get
        {
            return this.moveOkay;
        }
    }

    public List<Material> materials;
    Rigidbody rigid;
    GameObject player;
    Launcher.LaunchMode mode;
    Launcher playerScript;
    public bool isLanding;
    bool moveOkay;
    const float velRate = 40f;
    int power;
}

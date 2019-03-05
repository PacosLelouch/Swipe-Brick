using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    private void Awake()
    {
        this.player = GameObject.Find("Player");
        this.collided = false;
        Brick.hSize = this.GetComponent<Transform>().localScale.z;
        Brick.wSize = this.GetComponent<Transform>().localScale.x;
    }
    // Use this for initialization
    void Start () {
        this.GetComponent<MeshRenderer>().material = origin;
        this.brick = this.GetComponent<Transform>().parent.gameObject;
        this.life = Score.GetScore + 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (collided)
        {
            brick.GetComponent<Transform>().Find("BrickText").GetComponent<TextMesh>().text = "";
            this.GetComponent<Transform>().localScale -= Vector3.one * 0.2f;
            if(this.GetComponent<Transform>().localScale.z <= 0)
            {
                Destroy(this.gameObject);
            }
            return;
        }
        brick.GetComponent<Transform>().Find("BrickText").GetComponent<TextMesh>().text = life.ToString();
        if(brick.GetComponent<Transform>().position.z - Brick.Height <= -15)
        {
            player.GetComponent<Launcher>().GameOver = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.relativeVelocity.ToString());
        if(collision.gameObject.tag == "Ball")
        {
            life -= collision.gameObject.GetComponent<BallLaunched>().Power;
            this.GetComponent<MeshRenderer>().material = hit;
        }
        if (life <= 0)
        {
            //Debug.Log("Destroy " + brick.ToString());
            //Destroy(brick.gameObject);
            collided = true;
            this.GetComponent<BoxCollider>().enabled = false;
            //this.GetComponent<BoxCollider>().size = Vector3.zero;
        }
        if(life < 0 && collision.gameObject.GetComponent<BallLaunched>().LaunchMode == Launcher.LaunchMode.Stone)
        {
            //Debug.Log("Break!");
            collision.gameObject.GetComponent<Rigidbody>().velocity = collision.relativeVelocity;
            if(collision.gameObject.GetComponent<BallLaunched>().Power >= 1)
            {
                collision.gameObject.GetComponent<BallLaunched>().Power -= 1;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        this.GetComponent<MeshRenderer>().material = origin;
    }

    static public float Height
    {
        get
        {
            return Brick.hSize;
        }
    }

    static public float Width
    {
        get
        {
            return Brick.wSize;
        }
    }

    public int Life
    {
        get
        {
            return this.life;
        }
    }

    public Material hit;
    public Material origin;
    GameObject player;
    GameObject brick;
    int life;
    bool collided;
    static float hSize;
    static float wSize;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {

    public enum LaunchMode {Stone, Snake, Unknown};
    public static Vector3 originalPos = new Vector3(0, 1, -14.3f);

    // Use this for initialization
    void Start () {
        //this.GetComponent<Rigidbody>().velocity = Vector3.back * Launcher.velRate;
        this.isFirstLanding = false; // The first ball landing. The launcher's next position.
        this.isLastLanding = false; // The last ball landing. Bricks move.
        this.moveOkay = false;
        this.tuning = false;
        this.returnOrigin = false;
        this.modeSelected = true;
        this.nextPower = this.totalPower = 1;
        this.gameOver = false;
        this.launchMode = LaunchMode.Unknown;
        this.launchTime = 0;
        this.landingCount = 0;
        this.launchVelocity = Vector3.zero;
        this.nextPos = originalPos;
        this.balls = new List<GameObject>();
        this.line = this.TestBeam.GetComponent<Transform>().GetChild(0).GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        //balls.RemoveAll(b => b.gameObject == null);
        this.GetComponentInChildren<TextMesh>().text = "";
        
        //Launch Balls
        if (this.launchMode == LaunchMode.Snake && this.balls.Count > 0 && this.balls.Count < this.totalPower)
        {
            int launchCycle = 5;
            this.launchTime++;
            if (this.launchTime >= launchCycle)
            {
                launchTime = 0;
                AddBalls(launchVelocity);
            }
            this.GetComponentInChildren<TextMesh>().text = "x" + (this.totalPower - this.balls.Count).ToString();
        }/*
        if(this.launchMode == LaunchMode.Snake && this.GetComponent<Rigidbody>().velocity == Vector3.zero && this.balls.Count + 1 == this.totalPower)
        {
            int launchCycle = 5;
            this.launchTime++;
            if (this.launchTime >= launchCycle)
            {
                launchTime = 0;
                this.GetComponent<Rigidbody>().velocity = launchVelocity;
            }
        }*/
        //Hide Player
        if (this.launchMode == LaunchMode.Stone && !this.isFirstLanding || this.balls.Count == this.totalPower)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {
        if (this.gameOver)
        {
            this.gameOverCanvas.GetComponent<Canvas>().enabled = true;
            return;
        }
        //Debug.Log(moveOkay.ToString());
        //Debug.Log(this.GetComponent<Rigidbody>().velocity.ToString());
        if (this.isFirstLanding) 
        {
            if (!this.GetComponent<MeshRenderer>().enabled)
            {
                this.GetComponent<Transform>().position = nextPos;
                this.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        if(this.landingCount == this.balls.Count && !this.isLastLanding)
        {
            Score.AddScore();
            this.isLastLanding = true;
            //Debug.Log("Landing, score = " + Score.GetScore.ToString());
        }
        if (this.isLastLanding && this.moveOkay && this.allBallMoveOkay())
        {
            this.launchMode = LaunchMode.Unknown;
            DestroyBalls();
            this.totalPower = this.nextPower;
            this.landingCount = 0;
            this.GetComponentInChildren<TextMesh>().text = "x" + (this.totalPower).ToString();
            //Launch Preparing
            if (ValidControlDown(0))
            {
                tuning = true;
                //Debug.Log("LeftMouseDown");
                if (this.modeSelected)
                {
                    line.enabled = true;
                    this.TestBeam.GetComponent<Transform>().position = this.GetComponent<Transform>().position;
                    this.TestBeam.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else if (line.enabled && (this.controlUp(0)|| returnOrigin))
            {
                if (returnOrigin)
                {
                    this.returnOrigin = false;
                    this.launchMode = LaunchMode.Unknown;
                    this.modeSelectCanvas.GetComponent<Canvas>().enabled = false;
                    this.line.enabled = false;
                    this.modeSelected = true;

                }
                else if (tuning)
                {
                    //Debug.Log("LeftMouseUp");
                    //Debug.Log(axis.ToString() + "," + angle.ToString());
                    this.modeSelected = false;
                }
                tuning = false;
            }
            //Turning
            if (tuning && this.line.enabled && modeSelected)
            {
                if (this.controlUp(1))
                {
                    returnOrigin = true;
                }
                this.controlDirection();
                //Debug.Log(Input.GetAxis("Mouse X").ToString() + ", " + Input.GetAxis("Mouse Y").ToString());
                //Debug.Log(this.TestBeamClone.GetComponent<Transform>().rotation.eulerAngles.ToString());
            }
        }
        //Mode Select
        if (!modeSelected && launchMode == LaunchMode.Unknown && !returnOrigin)
        {
            if (this.controlUp(1))
            {
                returnOrigin = true;
            }
            this.modeSelectCanvas.GetComponent<Canvas>().enabled = true;
        }
    }
    
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            /*if (!this.isFirstLanding)
            {
                this.isFirstLanding = true;
                this.nextPos = this.GetComponent<Transform>().position;
            }
            this.landingCount++;*/
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    
    public void StoneMode()
    {
        this.modeSelected = true;
        this.launchMode = LaunchMode.Stone;
        this.modeSelectCanvas.GetComponent<Canvas>().enabled = false;
        LaunchBalls();
    }

    public void SnakeMode()
    {
        this.modeSelected = true;
        this.launchMode = LaunchMode.Snake;
        this.modeSelectCanvas.GetComponent<Canvas>().enabled = false;
        LaunchBalls();
    }

    void LaunchBalls()
    {
        isFirstLanding = isLastLanding = false;
        float radians = Mathf.Deg2Rad * this.TestBeam.GetComponent<Transform>().rotation.eulerAngles.y;
        //Debug.Log(this.TestBeamClone.GetComponent<Transform>().rotation.eulerAngles.ToString());
        if (line.enabled)
        {
            line.enabled = false;
        }
        launchVelocity = new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians)) * Launcher.velRate;
        if (Mathf.Abs(launchVelocity.z) < 0.1f)
        {
            launchVelocity = new Vector3(Mathf.Sin(radians), 0, launchVelocity.z >= 0 ? 0.1f : -0.1f) * Launcher.velRate;
        }
        //Debug.Log("Launch: " + launchMode.ToString());
        switch (launchMode)
        {
            case LaunchMode.Stone:
                //this.GetComponent<Rigidbody>().velocity = launchVelocity;
                AddBalls(launchVelocity, totalPower);
                break;
            case LaunchMode.Snake:
                AddBalls(launchVelocity);
                break;
        }
    }

    void AddBalls(Vector3 velocity, int power = 1)
    {
        GameObject addBall = Instantiate(ballLaunched);
        addBall.GetComponent<Transform>().position = this.GetComponent<Transform>().position + velocity * 0.01f;
        addBall.GetComponent<Transform>().parent = ballsParent.GetComponent<Transform>();
        addBall.GetComponent<Rigidbody>().velocity = velocity;
        addBall.GetComponent<BallLaunched>().Power = power;
        addBall.GetComponent<BallLaunched>().LaunchMode = launchMode;
        balls.Add(addBall);
    }

    void DestroyBalls()
    {
        if(balls.Count == 0)
        {
            return;
        }
        foreach(GameObject ball in balls)
        {
            Destroy(ball.gameObject);
        }
        balls.Clear();
    }

    bool allBallMoveOkay()
    {
        return balls.TrueForAll(b => b.GetComponent<BallLaunched>().MoveOkay);
    }

    void controlDirection()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                this.TestBeam.GetComponent<Transform>().Rotate(0, Input.GetTouch(0).deltaPosition.x * 0.5f - Input.GetTouch(0).deltaPosition.y * 0.1f, 0);
            }
        }
        else
        {
            this.TestBeam.GetComponent<Transform>().Rotate(0, Input.GetAxis("Mouse X") * 5 - Input.GetAxis("Mouse Y"), 0);
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.TestBeam.GetComponent<Transform>().Rotate(0, -1, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.TestBeam.GetComponent<Transform>().Rotate(0, 1, 0);
            }
        }
        float degree = this.TestBeam.GetComponent<Transform>().rotation.eulerAngles.y;
        if (degree > 85 && degree < 180)
        {
            this.TestBeam.GetComponent<Transform>().rotation = Quaternion.Euler(0, 85, 0);
        }
        else if (degree < 275 && degree >= 180)
        {
            this.TestBeam.GetComponent<Transform>().rotation = Quaternion.Euler(0, 275, 0);
        }
    }

    bool controlUp(int i)
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return Input.touchCount > i && (Input.GetTouch(i).phase == TouchPhase.Ended);
            //return new List<Touch>(Input.touches).TrueForAll(t => !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(t.fingerId));

        }
        else
        {
            return Input.GetMouseButtonUp(i);
        }
    }

    bool ValidControlDown(int i)
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //Debug.Log(Input.touchCount);
            //Debug.Log(i.ToString() + "," + Input.GetTouch(i).phase.ToString());
            return Input.touchCount > i && (Input.GetTouch(i).phase == TouchPhase.Began) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId);
            //return new List<Touch>(Input.touches).TrueForAll(t => !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(t.fingerId));
        }
        else
        {
            return Input.GetMouseButtonDown(i) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        }
    }

    public void Restart()
    {
        this.GetComponent<Transform>().position = originalPos;
        Score.GetScore = -1;
        GameObject.Find("Bricks").GetComponent<CreateBricks>().init();
        this.gameOverCanvas.GetComponent<Canvas>().enabled = false;
        this.Start();
    }

    public int TotalPower
    {
        get
        {
            return this.totalPower;
        }
        set
        {
            this.totalPower = value;
        }
    }

    public bool IsFirstLanding
    {
        get
        {
            return this.isFirstLanding;
        }
        set
        {
            this.isFirstLanding = value;
        }
    }

    public bool IsLastLanding
    {
        get
        {
            return this.isLastLanding;
        }
        set
        {
            this.isLastLanding = value;
        }
    }

    public bool MoveOkay
    {
        set
        {
            this.moveOkay = value;
        }
    }

    public bool GameOver
    {
        set
        {
            this.gameOver = value;
        }
        get
        {
            return this.gameOver;
        }
    }

    public int LandingCount
    {
        set
        {
            this.landingCount = value;
        }
        get
        {
            return this.landingCount;
        }
    }

    public Vector3 NextPos
    {
        set
        {
            this.nextPos = value;
        }
        get
        {
            return this.nextPos;
        }
    }

    public int NextPower
    {
        set
        {
            this.nextPower = value;
        }
        get
        {
            return this.nextPower;
        }
    }

    LineRenderer line;
    List<GameObject> balls;
    LaunchMode launchMode;
    const float velRate = 40f;
    public GameObject mainCanvas;
    public GameObject modeSelectCanvas;
    public GameObject ballLaunched;
    public GameObject ballsParent;
    public GameObject gameOverCanvas;
    public GameObject TestBeam;
    Vector3 launchVelocity;
    Vector3 nextPos;
    bool modeSelected;
    bool gameOver;
    int totalPower;
    int nextPower;
    int launchTime;
    int landingCount;
    bool moveOkay;
    bool isFirstLanding;
    public bool isLastLanding;
    bool tuning;
    bool returnOrigin;
}

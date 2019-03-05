using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBricks : MonoBehaviour {

	// Use this for initialization
	void Start () {
        created = false;
        offset = 0;
        bricks = new List<GameObject>();
        powers = new List<GameObject>();
        launcher = player.GetComponent<Launcher>();
    }

    public void init()
    {
        foreach(GameObject brickClone in bricks)
        {
            Destroy(brickClone.gameObject);
        }
        foreach (GameObject powerClone in powers)
        {
            Destroy(powerClone.gameObject);
        }
        Start();
    }

    // Update is called once per frame
    void Update () {
        bricks.RemoveAll(b => b.gameObject == null);
        powers.RemoveAll(b => b.gameObject == null);
        if (launcher.IsLastLanding)
        {
            if (!created)
            {
                launcher.MoveOkay = false;
                float min = 1 + Mathf.Min(1.9f, float.Parse((Score.GetScore / 20).ToString()));
                float max = 2 + Mathf.Min(6.9f, float.Parse((Score.GetScore / 10).ToString()));
                //Debug.Log(min.ToString() + ", " + max.ToString());
                int number = Mathf.FloorToInt(Random.Range(min, max));
                List<int> grid = new List<int>();
                for (int i = 0; i < number + 1; i++) // The last is the power adder.
                {
                    int addNum = Mathf.FloorToInt(Random.Range(0, 9.9f));
                    while (grid.Contains(addNum))
                    {
                        addNum = Mathf.FloorToInt(Random.Range(0, 9.9f));
                    }
                    grid.Add(addNum);
                }
                for (int i = 0; i < number; i++)
                {
                    GameObject brickClone = Instantiate(brick);
                    brickClone.GetComponent<Transform>().Translate(grid[i] * Brick.Width, 0, Brick.Height);
                    //Debug.Log(brickClone.GetComponent<Transform>().position.ToString());
                    brickClone.GetComponent<Transform>().parent = this.GetComponent<Transform>();
                    bricks.Add(brickClone);
                }
                GameObject powerClone = Instantiate(power);
                powerClone.GetComponent<Transform>().Translate(grid[number] * Brick.Width, 0, Brick.Height);
                //Debug.Log(brickClone.GetComponent<Transform>().position.ToString());
                powerClone.GetComponent<Transform>().parent = this.powersObj.GetComponent<Transform>();
                bricks.Add(powerClone);
                offset = 0;
                created = true;
            }
            else // created
            {
                if(offset < Brick.Height)
                {
                    offset += 1;
                    foreach(GameObject brickClone in bricks)
                    {
                        brickClone.GetComponent<Transform>().Translate(Vector3.back);
                    }
                }
                else
                {
                    launcher.MoveOkay = true;
                }
            }
            //Debug.Log(this.GetComponentsInChildren<Transform>(true).Length.ToString());
        }
        else // Not Landing
        {
            created = false;
            offset = 0;
        }
	}

    public GameObject powersObj;
    public GameObject power;
    public GameObject player;
    public GameObject brick;
    bool created;
    float offset;
    Launcher launcher;
    public List<GameObject> powers;
    public List<GameObject> bricks;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePages : MonoBehaviour {

    // Use this for initialization
    void Start () {
        page = 0;
        pages = this.GetComponent<Transform>().GetChild(1);
        pageNum = this.GetComponent<Transform>().Find("PageNum").gameObject;
        previousButton = this.GetComponent<Transform>().Find("PreviousPage").gameObject;
        nextButton = this.GetComponent<Transform>().Find("NextPage").gameObject;
        previousButton.GetComponent<Button>().interactable = false;
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void nextPage()
    {
        if(this.page < pages.childCount - 1)
        {
            this.pages.GetChild(page).gameObject.SetActive(false);
            this.page++;
            this.pages.GetChild(page).gameObject.SetActive(true);
            this.previousButton.GetComponent<Button>().interactable = true;
            this.pageNum.GetComponent<Text>().text = (this.page + 1).ToString() + "/" + pages.childCount;
        }
        if (this.page == pages.childCount - 1)
        {
            this.nextButton.GetComponent<Button>().interactable = false;
        }
    }

    public void previousPage()
    {
        if (page > 0)
        {
            this.pages.GetChild(page).gameObject.SetActive(false);
            this.page--;
            this.pages.GetChild(page).gameObject.SetActive(true);
            this.nextButton.GetComponent<Button>().interactable = true;
            this.pageNum.GetComponent<Text>().text = (this.page + 1).ToString() + "/" + pages.childCount;
        }
        if (this.page == 0)
        {
            this.previousButton.GetComponent<Button>().interactable = false;
        }
    }

    Transform pages;
    int page;
    GameObject pageNum;
    GameObject previousButton;
    GameObject nextButton;
}

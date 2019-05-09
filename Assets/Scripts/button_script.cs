using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class button_script : MonoBehaviour
{
    //public Button outcropsTab;
    //public Button handSamplesTab;
    //public Button demTab;
    //public Button crystalLatticeTab;

    public GameObject outcropsTab;
    public GameObject handSamplesTab;
    public GameObject demTab;
    public GameObject crystalLatticeTab;
    //public PopulateGrid populateGridObject;
    PopulateGrid other;


    void Start(){
        GameObject go = GameObject.FindWithTag("Content");
        other = (PopulateGrid)go.GetComponent(typeof(PopulateGrid));
    }

    public void activateOutcrops(){
        //GameObject thing = GameObject.FindWithTag("Border");
        //thing.SetActive(false);

        outcropsTab.SetActive(true);
        handSamplesTab.SetActive(false);
        demTab.SetActive(false);
        crystalLatticeTab.SetActive(false);


        //handSamplesTab.transform.Find("Selected Border").SetActive(false);
        //demTab.FindWithTag("Border").SetActive(false);
        //crystalLatticeTab.FindWithTag("Border").SetActive(false);

        //handSamplesTab.GetComponent<Image>().color = Color.white;
        //demTab.GetComponent<Image>().color = Color.white;
        //crystalLatticeTab.GetComponent<Image>().color = Color.white;
        other.deleteItems();

    }
    public void activateHandSamples()
    {

        outcropsTab.SetActive(false);
        handSamplesTab.SetActive(true);
        demTab.SetActive(false);
        crystalLatticeTab.SetActive(false);
        //handSamplesTab.GetComponent<Image>().color = Color.red;
        //outcropsTab.GetComponent<Image>().color = Color.white;
        //demTab.GetComponent<Image>().color = Color.white;
        //crystalLatticeTab.GetComponent<Image>().color = Color.white;
        other.deleteItems();


    }
    public void activateDEM()
    {

        outcropsTab.SetActive(false);
        handSamplesTab.SetActive(false);
        demTab.SetActive(true);
        crystalLatticeTab.SetActive(false);
        //demTab.GetComponent<Image>().color = Color.red;
        //handSamplesTab.GetComponent<Image>().color = Color.white;
        //outcropsTab.GetComponent<Image>().color = Color.white;
        //crystalLatticeTab.GetComponent<Image>().color = Color.white;
        other.deleteItems();


    }
    public void activateCrystalLattice()
    {

        outcropsTab.SetActive(false);
        handSamplesTab.SetActive(false);
        demTab.SetActive(false);
        crystalLatticeTab.SetActive(true);
        //crystalLatticeTab.GetComponent<Image>().color = Color.red;

        //handSamplesTab.GetComponent<Image>().color = Color.white;
        //demTab.GetComponent<Image>().color = Color.white;
        //outcropsTab.GetComponent<Image>().color = Color.white;
        other.deleteItems();

    }

}

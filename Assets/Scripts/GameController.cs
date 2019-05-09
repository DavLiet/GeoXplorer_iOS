using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour
{

    //public GameObject centerpiece;
    public GameObject menu;
    public GameObject loadingScreen;
    public GameObject adjustMenu;
    public bool isMenuShowing = false;
    public bool isLoadScreenShowing = false;
    public bool isAdjustMenuShowing = false;
    public Text exampleText;
    public GameObject current;  // current assetbundle that user can instantiate
    public string currentPrefabName; // prefabname of current assetbundle
    public string currentPrefabURL; // url of current asset bundle
    public List<GeoViewerAsset> globalAssets;
    public PopulateGrid populateGrid;
    public List<Texture2D> logoImages = new List<Texture2D>();  // caches all images
    public List<string> logoURLs = new List<string>(0); // caches url of images
    private bool areButtonsShowing = false;
    public GameObject buttonsMenu;
    public GameObject buttonsGroup;

    public GameObject startScreen; // initial scene
    private bool isStartScreenShowing = true;

    public List<GameObject> assetBundleCache = new List<GameObject>();  // caches past 5 asset bundles
    public List<string> assetBundleCacheURL = new List<string>(); // caches urls of past 5 asset bundles
    public List<AssetBundle> assetBundleOriginalCache = new List<AssetBundle>();


    // Start is called before the first frame update
    void Start()
    {
        logoURLs.Clear();
       
    }

    // Update is called once per frame
    void Update()
    {
        if(isMenuShowing){
            menu.SetActive(true);
        }
        else{
            menu.SetActive(false);
        }
        if (isLoadScreenShowing)
        {
            loadingScreen.SetActive(true);
            loadingScreen.transform.SetAsLastSibling();
        }
        else
        {
            loadingScreen.SetActive(false);
        }

        adjustMenu.SetActive(isAdjustMenuShowing);

        buttonsMenu.SetActive(!areButtonsShowing);

        buttonsGroup.SetActive(areButtonsShowing);

        startScreen.SetActive(isStartScreenShowing);

    }

    // accessor methods for logoURLs list data structure
    public int getSizeOfLogoURLs(){
        return logoURLs.Count;
    }

    public void hideStartScreen()
    {
        isStartScreenShowing = false;
    }

    public void showStartScreen(){
        isStartScreenShowing = true;

    }

    public void addToLogoURLs(string item){
        logoURLs.Add(item);
    }

    public void removeFromLogoURLs(System.Predicate<string> item){
        string found = logoURLs.Find(item);
        logoURLs.Remove(found);
    }

    public bool isInLogoURLs(string item){
        

        return logoURLs.Contains(item);
    }

   
    public void DisplayButtons(){
        areButtonsShowing = true;
    }

    public void HideButtons(){
        areButtonsShowing = false;
    }

    public void DisplayMenu(){
        
        isMenuShowing = true;

    }
    public void HideMenu()
    {
        isMenuShowing = false;
    }

    public void DisplayLoadingScreen(){
        //print("YESSSSSSS");
        isLoadScreenShowing = true;
        //print(isLoadScreenShowing);
    }
    public void HideLoadingScreen()
    {
        isLoadScreenShowing = false;
    }

    public void DisplayAdjust(){
        isAdjustMenuShowing = true;
    }
    public void HideAdjust()
    {
        isAdjustMenuShowing = false;
    }

    public void loadCrystalLatticeData(){
        populateGrid.loadData("https://salty-oasis-92702.herokuapp.com/geoViewerHiRise");
    }
    public void loadElevationModelData()
    {
        populateGrid.loadData("https://salty-oasis-92702.herokuapp.com/geoViewerDEM");

    }
    public void loadHandSamplesData()
    {
        populateGrid.loadData("https://salty-oasis-92702.herokuapp.com/geoViewerHandSample");
    }
    public void loadOutcropsData()
    {
        populateGrid.loadData("https://salty-oasis-92702.herokuapp.com/geoViewerAssetBundles");
    }


}

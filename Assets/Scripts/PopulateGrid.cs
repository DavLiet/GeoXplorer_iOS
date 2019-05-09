using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;



// Thanks: https://www.youtube.com/watch?time_continue=607&v=VyIo5tlNNeA


public class PopulateGrid : MonoBehaviour
{

    //private bool requestStarted = false;
    UnityWebRequest uwr;
    Coroutine theCoroutine;

    public GameController gameController;
    
    public GameObject prefab;
    public GameObject fetchingIndicator;
    public GameObject wifiPanel;

    public string container;

    public API api;
    
    private List<GeoViewerAsset> assets;

    public bool isWaiting = false;

    // content GameObject
    public GameObject content;

    // Progress Bar
    public Image progressBar;
    public Text progressText;
    public Button cancelButton;


    // Start is called before the first frame update
    void Start()
    {
        //GameObject newObj;

        wifiPanel.SetActive(false);

        //foreach(GeoViewerAsset i in assets){
        //    newObj = (GameObject)Instantiate(prefab, transform);
        //    newObj.GetComponent<Image>().color = Random.ColorHSV();
        //}
        //for (int i = 0; i < assets; i++)
        // {
        //newObj = (GameObject)Instantiate(prefab, transform);
        //newObj.GetComponent<Image>().color = Random.ColorHSV();
        //}

    }

    public void loadData(string containerURL){

        if(!isWaiting){   // only make request if one has already not been made
            theCoroutine = StartCoroutine(MakeRequest(containerURL));
        }

    }
    public IEnumerator MakeRequest(string containerURL)
    {

        isWaiting = true;
        using (uwr = UnityWebRequest.Get(containerURL))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                if (uwr.error == "Cannot resolve destination host"){
                    // not connected to internet
                    wifiPanel.SetActive(true);
                }
                isWaiting = false;
            }
            else
            {
                wifiPanel.SetActive(false);

                //gameController.globalAssets.Clear();

                JSONObject N = (JSONObject)SimpleJSON.JSON.Parse(uwr.downloadHandler.text);
                if(N != null){
                    JSONObject test = N.AsObject;
                    JSONNode.KeyEnumerator abc = test.Keys;

                    // remove items currently in grid

                    deleteItems();

                    var width = content.GetComponent<RectTransform>().rect.width - 20;

                    foreach (string i in abc)
                    {
                        GeoViewerAsset newAsset = new GeoViewerAsset();
                        newAsset.prefabName = N[i]["metadata"]["prefabname"];
                        newAsset.modelName = N[i]["metadata"]["modelname"];
                        newAsset.authorName = N[i]["metadata"]["author"];

                        newAsset.url = N[i]["url"];
                        GameObject newObj = (GameObject)Instantiate(prefab, transform);
                  
                        newObj.GetComponent<ButtonData>().prefabName = newAsset.prefabName;
                        newObj.GetComponent<ButtonData>().authorName = newAsset.authorName;
                        newObj.GetComponent<ButtonData>().modelName = newAsset.modelName;
                        //newObj.GetComponent<ButtonData>().fileName = N[i]["blobName"];


                        newObj.GetComponent<ButtonData>().prefabURL = newAsset.url;



                        newObj.GetComponent<ButtonData>().logoURL = "https://fossett.blob.core.windows.net/screenshot-container/" + newAsset.prefabName + ".png";
                        newObj.GetComponent<ButtonData>().gameController = gameController;
                     
                        newObj.GetComponent<ButtonData>().api = api;

                        newObj.GetComponent<ButtonData>().cellWidth = width/3;
                        //newObj.GetComponent<ButtonData>().width = width/3;




                        //newObj.GetComponentInChildren<Text>().text = newAsset.modelName;

                        //newObj.GetComponentInChildren<Text>().text = newAsset.authorName;
                    }
                }
             


               

                isWaiting = false;


               

                //requestStarted = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        fetchingIndicator.SetActive(isWaiting);

       

    }

    public void deleteItems(){
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    [System.Serializable]
    public class ItemInfo
    {
        public string name;
        public int lives;
        public float health;


        // Given JSON input:
        // {"name":"Dr Charles","lives":3,"health":0.8}
        // this example will return a PlayerInfo object with
        // name == "Dr Charles", lives == 3, and health == 0.8f.
    }
}

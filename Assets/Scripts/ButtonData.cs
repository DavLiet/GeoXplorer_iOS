using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ButtonData : MonoBehaviour
{
    public string prefabName = "No Prefab Name";
    public string modelName = "No Model Name";
    public string authorName = "No Author Name";
    public string prefabURL = "";
    public API api;
    public string logoURL = "";
    public Texture2D defaultLogo;
    public GameController gameController;
    bool isLoadingImage = false;
    public GameObject imageLoadingIndicator;
    public Text author;
    public Text model;
    public float cellWidth;

    //public GameController gameController;
    //private bool requestStarted = false;
    //UnityWebRequest uwr;
    //Coroutine theCoroutine;

    // Progress Bar
    //public Image progressBar;
    //public Text progressText;
    //public Button cancelButton;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(FetchImage());

        author.GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth - 5, 50); // resize img to correct size
        author.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 75); // resize img to correct size

        model.GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth - 5, 50); // resize img to correct size
        model.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 150); // resize img to correct size

        author.text = "Author: " + authorName;
        model.text = modelName;

        int offset = 5;
        this.GetComponentInChildren<RawImage>().GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth - offset, cellWidth - offset); // resize img to correct size
        this.GetComponentInChildren<RawImage>().GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -cellWidth / 2, 0); // move img down 
    }


    private IEnumerator FetchImage()
    {
      
        if (!gameController.logoURLs.Contains(logoURL))
        {
            isLoadingImage = true;

            UnityWebRequest www = UnityWebRequestTexture.GetTexture(logoURL,true);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                this.GetComponentInChildren<RawImage>().texture = defaultLogo;
                isLoadingImage = false;

            }
            else
            {

                Texture2D logoTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;


                //print(logoTexture.isReadable);

                //Texture2D newTexture = new Texture2D(logoTexture.width, logoTexture.height);
                //newTexture.LoadImage(www.downloadHandler.data);


                //////Create new empty Texture
                //Texture2D newTex = new Texture2D(logoTexture.width, logoTexture.height, TextureFormat.RGB24, false);
                ////////Copy old texture pixels into new one
                //newTex.SetPixels(logoTexture.GetPixels());
                ////////Apply
                //newTex.Apply();

                
                this.GetComponentInChildren<RawImage>().texture = logoTexture;


                gameController.logoImages.Add(logoTexture); // cache image
                gameController.addToLogoURLs(logoURL);
                isLoadingImage = false;

                // Set size of image to appropriate scale
                //var width = container.GetComponent<RectTransform>().rect.width;  // get width of cell

            }


            www.Dispose(); // dispose of UnityWebRequest
        }
        else{
            int targetIndex = gameController.logoURLs.IndexOf(logoURL);
            Texture targetTexture = gameController.logoImages[targetIndex];  // retrieve image from cache
            this.GetComponentInChildren<RawImage>().texture = targetTexture;

        }
    }
    void Update(){
        imageLoadingIndicator.SetActive(isLoadingImage);
    }


    public void getData(){
        api.LoadAssetBundle(prefabURL,prefabName);
   }

}

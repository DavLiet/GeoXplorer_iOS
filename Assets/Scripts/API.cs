using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class API : MonoBehaviour
{

    public GameController gameController;
    private bool requestStarted = false;
    UnityWebRequest uwr;
    public Image progressBar;
    public Text progressText;
    Coroutine theCoroutine;
    //AssetBundleRequest downloadedAsset;

    public ARTapToPlaceObject lastEffort;

    public GameObject parentContainer;
    public GameObject scaleControls;  // accesses ScaleControls.cs script




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(requestStarted)
        {
            gameController.DisplayLoadingScreen();
            int progress = (int)(uwr.downloadProgress * 100f);
            progressText.text = progress.ToString() + "%";           
            progressBar.fillAmount = uwr.downloadProgress;
          


        }
        else{
            gameController.HideLoadingScreen();

        }

    }

  

    public void abortRequest(){
        uwr.Abort(); // halts the UnityWebRequest as soon as possible.
        uwr.Dispose(); // Signals that this UnityWebRequest is no longer being used, and should clean up any resources it is using.
        requestStarted = false;
        StopCoroutine(theCoroutine);

    }

    public IEnumerator MakeRequest(string assetURL, string prefabName){

        if (!gameController.assetBundleCacheURL.Contains(assetURL))
        {
            requestStarted = true;
            using (uwr = UnityWebRequestAssetBundle.GetAssetBundle(assetURL))
            {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Debug.Log(uwr.error);
                }
                else
                {


                    // Get downloaded asset bundle
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                    AssetBundleRequest downloadedAsset = bundle.LoadAssetAsync(prefabName, typeof(GameObject));
                    yield return downloadedAsset;
                    GameObject newObject = downloadedAsset.asset as GameObject;


                    // Once NEW asset bundle has been successfully downloaded, reset scaling and position
                    scaleControls.GetComponent<ScaleControls>().resetScale();
                    scaleControls.GetComponent<ScaleControls>().resetPosition();

           

                    int index = assetURL.IndexOf("geoviewer-hirise");
                    if (index != -1)
                    {
                        // If assetbundle is Mars HiRes

                       

                    }
                    else
                    {
                    
                        // Fix for asset bundle rendering
                        //Shader shader1 = Shader.Find("Mobile/Unlit (Supports Lightmap)");   
                        Shader shader1 = Shader.Find("Standard");
                        Renderer[] rends = newObject.GetComponentsInChildren<MeshRenderer>();
                        foreach (Renderer rend in rends)
                        {
                            foreach (var mat in rend.sharedMaterials)
                            {
                                mat.shader = shader1;
                            }
                        }

                    }


                    NormalizeGameObject(newObject, parentContainer.transform, 5f); //Make the normalized size of the whole model 5m across in the X dimension

                    //var obj = Instantiate(newObject,parentContainer.transform);
                    //UVMapper sc = obj.AddComponent(typeof(UVMapper)) as UVMapper;
                    //sc.meshName = prefabName; //should be the filename 
                    //sc.surfaceType = "sb";
                    //sc.FetchTexture();


                    //gameController.current = newObject;  // update the currently downloaded 3d model
                    lastEffort.objectToPlace = newObject;
                    gameController.currentPrefabURL = assetURL;  // update the URL of currently downloaded 3d model
                    gameController.currentPrefabName = prefabName;  // update the prefab of currently downloaded 3d model
                    //gameController.assetBundleCache.Add(newObject);  // add asset bundle to cache
                    //gameController.assetBundleCacheURL.Add(assetURL); // add url of asset bundle to cache

                    gameController.assetBundleOriginalCache.Add(bundle); // ******

                    // cache asset bundles...currently we don't really cache multiple past assetbundles, but changing the predicate from 1 to a higher number will allow us to cache multiple bundles
                    if (gameController.assetBundleOriginalCache.Count > 1)
                    {

                        //gameController.assetBundleCache.RemoveAt(0); // remove null from cache
                        //gameController.assetBundleCacheURL.RemoveAt(0); // remove corresponding URL from cache

                        AssetBundle toRemove = gameController.assetBundleOriginalCache[0]; // ******
                        gameController.assetBundleOriginalCache.RemoveAt(0);
                        toRemove.Unload(true);  // Destroy asset bundle to free up space


                    }

                    // Frees the memory from the web stream
                    uwr.Dispose();

                    requestStarted = false;

                }
            }
        }
        else{
            int targetIndex = gameController.assetBundleCacheURL.IndexOf(assetURL);
            gameController.current = gameController.assetBundleCache[targetIndex];
        }



    }

    public void LoadAssetBundle(string assetURL, string prefabName){
   
            theCoroutine = StartCoroutine(MakeRequest(assetURL, prefabName));
    
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
                gameController.exampleText.text = webRequest.error;

            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                gameController.exampleText.text = webRequest.downloadHandler.text;
            }
        }
    }




    //// Quick and Dirty Normalization Function: - Martin Pratt
    private void NormalizeGameObject(GameObject newObject, Transform parentTransform, float targetXSize)
    {
        float minXBound = 10000000;
        float maxXBound = -10000000;
        Renderer[] rends = newObject.GetComponentsInChildren<Renderer>();  //The prefabs may contain multiple submeshes so cycle through all of them
        foreach (var rend in rends)
        {
            float rendMin = rend.bounds.min.x;  //we'll work with the x dimension for now. Could be expanded so that it goes through the y and z dimensions as well, finds the largest one and normalizes by that dimension
            float rendMax = rend.bounds.max.x;

            if (rendMin < minXBound)
            {
                minXBound = rendMin;
            }
            if (rendMax > maxXBound)
            {
                maxXBound = rendMax;
            }
        }

        float scaleFactor = targetXSize / (maxXBound - minXBound);
        parentTransform.localScale = Vector3.one * scaleFactor;  //Apply the normalization to the parent transform
    }

}







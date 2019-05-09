using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class UVMapper : MonoBehaviour
{
    //Script to define how a texure is mapped onto a HiRISE DTM mesh from an assetbundle
    //
    //Written by Martin Pratt, Fossett Lab for Virtual Planetary Exploration
    //Washington University in St. Louis
    //
    //April 2019


    public Material surfaceMaterial; //Make this an Unlit Texture material, call the material "HiRiseMaterial" as it gets defined below in FetchTexture
    public string meshName;          //This will get filled by your assetbundle loader (is the same as the file name -- has img at the end of it)
    public string surfaceType;       //This should be either 'sb' or 'cb', for Satellite image or color altimetry respectively
    public string maxHeight;         //The max height of the altimetry information, useful for generating a scale bar
    public string minHeight;         // "  min  "   "   "   "   "   "   "   "   "   "   "   "   "   "   "   "   "

    string texturePath;              //Path of texture information
    string readmeTexturePath;        //Path of texture readme that provides information for altimetry range



    // Start is called before the first frame update
    void Start()
    {
        //FetchTexture();
    }

    //Use this method to generate the texture (if the surfaceType variable is changed, can be used to switch between textures)
    public void FetchTexture()
    {

        //Generate the texture path from the HiRISE ID
        CreateTexturePath(meshName); 

        //Find the mesh material and define surfaceMaterial
        surfaceMaterial = (Material)Resources.Load("HiRiseMaterial", typeof(Material));

        //Go get the mesh texture
        StartCoroutine(GoFetchTexture());


        //Go get the altimetry information
        // StartCoroutine(FindElevInfo()); not necessary is SurfaceType is not "cb"
    }

    private IEnumerator FindElevInfo()
    {

        using (UnityWebRequest uwr = new UnityWebRequest("https://www.uahirise.org/PDS/EXTRAS/DTM/" + readmeTexturePath))
        {
            //wait for download to complete
            yield return uwr;

            //asign altimetry information to public string variables
            print("https://www.uahirise.org/PDS/EXTRAS/DTM/" + readmeTexturePath);
            string[] lineData = uwr.downloadHandler.text.Split("\n"[0]);
            string[] minHeightHeightData = lineData[13].Split(" "[0]);
            string[] maxHeightHeightData = lineData[14].Split(" "[0]);
            minHeight = minHeightHeightData[7] + " m";
            maxHeight = maxHeightHeightData[7] + " m";

            uwr.Dispose();
        }
    }


    // Use this for initialization
    IEnumerator GoFetchTexture()
    {
        print("GO FETCH TEXTURE CALLED -------------------------------------------------------");
        string url = "https://www.uahirise.org/PDS/EXTRAS/DTM/" + texturePath + surfaceType + ".jpg";

        //Renderer initialRenderer = GetComponent<Renderer>();
        //initialRenderer.material = surfaceMaterial;


        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url, true);
        
        //wait for download to complete
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            print("SUCCESS");
            //assign texture
            Renderer renderer = GetComponent<Renderer>();
            //surfaceMaterial.mainTexture = uwr.texture;
            surfaceMaterial.mainTexture = ((DownloadHandlerTexture)uwr.downloadHandler).texture;

            renderer.material = surfaceMaterial;

            uwr.Dispose();
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;

        //Define the UVs for the mesh so that the texture is mapped correctly
        Vector3 maxBounds = mesh.bounds.max;
        Vector3 minBounds = mesh.bounds.min;

        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / (maxBounds.x + minBounds.x), vertices[i].y / -(maxBounds.y + minBounds.y));
        }
        mesh.uv = uvs;
    }

    //Both texure and readme paths are derived here
    void CreateTexturePath(string mName)
    {
        string orbNumStr = mName.Substring(6, 6);
        string orbNumStr1 = mName.Substring(13, 4);
        string orbNumStr2 = mName.Substring(18, 6);
        string orbNumStr3 = mName.Substring(25, 4);
        string orbNumStr4 = mName.Substring(0, 34);
        int orbNumInt = int.Parse(orbNumStr);
        int lowerOrbInt = RoundDown(orbNumInt);
        int upperOrbInt = RoundUp(orbNumInt) - 1;

        //Extended (ESP) or Primary (PSP) mission time, it gets a little tricky when the orbit times go across this time, may need to be fixed, but it works for the vast majority of models
        string missionTime = "ESP";
        if (lowerOrbInt < 11000)
        {
            missionTime = "PSP";
        }

        readmeTexturePath = missionTime + "/ORB_" + lowerOrbInt.ToString("000000") + "_" + upperOrbInt.ToString("000000") + "/" + missionTime + "_" + orbNumStr + "_" + orbNumStr1 + "_" + missionTime + "_" + orbNumStr2 + "_" + orbNumStr3 + "/" + "README.TXT";
        texturePath = missionTime + "/ORB_" + lowerOrbInt.ToString("000000") + "_" + upperOrbInt.ToString("000000") + "/" + missionTime + "_" + orbNumStr + "_" + orbNumStr1 + "_" + missionTime + "_" + orbNumStr2 + "_" + orbNumStr3 + "/" + orbNumStr4;
    }


    int RoundUp(int toRound)
    {
        if (toRound % 100 == 0) return toRound;
        return (100 - toRound % 100) + toRound;
    }

    int RoundDown(int toRound)
    {
        return toRound - toRound % 100;
    }
}

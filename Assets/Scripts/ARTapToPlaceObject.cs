using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject oldObject;
    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    public GameObject placementIndicator;
    public GameObject parentContainer;
    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        //if(objectToPlace != null){
        //    gameController.current = objectToPlace;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        // For every frame, we want to check the world around us, check where the camera is pointing, and check where to place the object.
        // To represent this object in space, we use a Pose data structure that describes the position and rotation of a 3D point.
        UpdatePlacementPose();

        UpdatePlacementIndicator();

     
        // if user has any fingers currently on screen, AND then we have to check the phase of one of these fingers (ex. first finger), and check if the touch just began
        if(placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){

            // Check if finger is over a UI element
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) // if the user has NOT touched a ui element
            {
               // if(gameController.current != null){
                    //Destroy(objectToPlace); 
                    //Destroy(objectToPlace.GetComponent<UVMapper>());
                    //objectToPlace = gameController.current;
                    PlaceObject();
                //}
            }
        }
    }

    public void PlaceObject(){

        if(oldObject != null){
            print("THIS HAS BEEN DESTROYED: " + oldObject.name + "*********************************");
            
            Destroy(oldObject);    // Creating a new object will cause the old one to be deleted
            Destroy(oldObject.GetComponent<UVMapper>());
            print("IS THIS DESTROYED: " + oldObject.name + "?????????????????????????????????????");

        }

        oldObject = Instantiate(objectToPlace, parentContainer.transform);


        int index = gameController.currentPrefabURL.IndexOf("geoviewer-hirise");

        // If assetbundle is Mars HiRes
        if (index != -1)
        {
            //UVMapper sc = oldObject.AddComponent(typeof(UVMapper)) as UVMapper;
            //sc.meshName = gameController.currentPrefabName;
            //sc.surfaceType = "sb";
            //sc.FetchTexture();

            oldObject.AddComponent<UVMapper>();

            oldObject.GetComponent<UVMapper>().meshName = gameController.currentPrefabName;
            oldObject.GetComponent<UVMapper>().surfaceType = "sb";
            oldObject.GetComponent<UVMapper>().FetchTexture();

            //objectToPlace = oldObject; // ????????????

            Vector3 meshCenter = oldObject.GetComponent<Renderer>().bounds.center;

            oldObject.transform.position = placementPose.position - meshCenter;

        }
        else{
            oldObject.transform.position = placementPose.position;
            oldObject.transform.rotation = placementPose.rotation;
        }


        arOrigin.MakeContentAppearAt(oldObject.transform, oldObject.transform.position, oldObject.transform.rotation);

    }

    private void UpdatePlacementPose(){

     
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)); // use to be Camera.current

        var hits = new List<ARRaycastHit>();
        // Second param is a list of ARRaycastHit objects, each object represents a physical point in the real world that the ray can hit
        // Third param is the trackable type, which is all the types of collisions with the physical world that can be detected
            // The most fa
        arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if(placementPoseIsValid){
            placementPose = hits[0].pose; // remember: the pose is just a DESCRIPTION. It does NOT update anything without other code.

            var cameraForward = Camera.current.transform.forward; // Describes the x,y,and z direction that a camera is pointing
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized; // we dont care how much the camera is pointing towards the ground or sky
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);

        }
    }

    private void UpdatePlacementIndicator(){
        if(placementPoseIsValid){
            placementIndicator.SetActive(true); // makes indicator visible
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);

        }
        else{
            placementIndicator.SetActive(false); // hide indicator
        }
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;

public class ScaleControls : MonoBehaviour
{
    public ARSessionOrigin ARSessionToScale;
    //public GameObject worldObject;
    public GameController gameController;
    private float currentScaleFactor = 1;

    // Start is called before the first frame update
    void Start()
    {
        print("INITIAL position: " + ARSessionToScale.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ScaleUp(){
        Vector3 originalScale = ARSessionToScale.transform.localScale;
        //Vector3 originalPos = gameController.centerpiece.transform.position;

        // To make things bigger, you actually DECREASE the scale of the ARSession. Here, we are increasing the size by 10%
        Vector3 newScale = originalScale * 0.9f;

        currentScaleFactor *= 0.9f;
        print("NEW FACTOR: " + currentScaleFactor);


        //Transform oldTransform = gameController.centerpiece.transform;
        ARSessionToScale.transform.localScale = newScale;

        //Debug.Log("INCREASING----------------------------------------------");

        //ARSessionToScale.MakeContentAppearAt(gameController.centerpiece.transform, gameController.centerpiece.transform.position, gameController.centerpiece.transform.rotation);

        //gameController.centerpiece.transform.position = new Vector3(0,0,0);
        //gameController.centerpiece.transform.rotation = oldTransform.rotation;

    }
    public void ScaleDown(){
        Vector3 originalScale = ARSessionToScale.transform.localScale;

        // To make things smaller, you actually INCREASE the scale of the ARSession. Here, we are decreasing the size by 10%
        Vector3 newScale = originalScale * 1.1f;

        currentScaleFactor *= 1.1f;
        print("NEW FACTOR: " + currentScaleFactor);



        ARSessionToScale.transform.localScale = newScale;
    }

    public void RotateLeft(){

        ARSessionToScale.transform.RotateAround(ARSessionToScale.transform.position, Vector3.up, 20);

    }
    public void RotateRight(){

        ARSessionToScale.transform.RotateAround(ARSessionToScale.transform.position, Vector3.up, -20);

    }

    public void Up(){
        Vector3 pastPosition = ARSessionToScale.transform.position;

        pastPosition.y -= currentScaleFactor / 5;

        ARSessionToScale.transform.position = pastPosition;

        //if (parentContainer != null)
        //{
        //    Vector3 pastPosition = parentContainer.transform.position;

        //    pastPosition.y -= currentScaleFactor / 5;

        //    parentContainer.transform.position = pastPosition;
        //}
    }

    public void Down(){
        Vector3 pastPosition = ARSessionToScale.transform.position;

        pastPosition.y += currentScaleFactor / 5;

        ARSessionToScale.transform.position = pastPosition;

        //if (parentContainer != null)
        //{
        //    Vector3 pastPosition = parentContainer.transform.position;

        //    pastPosition.y += currentScaleFactor / 5;

        //    parentContainer.transform.position = pastPosition;
        //}
    }

    // Reset Scale
    public void resetScale(){

        print("CALLED resetScale");

        Vector3 originalScale = ARSessionToScale.transform.localScale;

        Vector3 newScale = new Vector3(5.0f,5.0f,5.0f);

        currentScaleFactor = 1;

        ARSessionToScale.transform.localScale = newScale;

    }

    // Reset Position
    public void resetPosition(){

        Vector3 pastPosition = ARSessionToScale.transform.position;

        pastPosition.y = 0;
        currentScaleFactor = 1;

       

        ARSessionToScale.transform.position = pastPosition;

    }
}

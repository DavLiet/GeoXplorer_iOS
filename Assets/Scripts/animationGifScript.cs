using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animationGifScript : MonoBehaviour
{

    public Sprite[] animatedImages;
    public Image animateImageObj;
    public float timeStep = .1f;
    float startTime;
    int start = 0;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - startTime >= timeStep)
        {
            animateImageObj.sprite = animatedImages[start % animatedImages.Length];
            start++;
            startTime = Time.time;

        }


    }
}

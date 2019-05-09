using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scalecell : MonoBehaviour
{

    public GameObject container;
   
    // Start is called before the first frame update
    void Update()
    {
        //float width = container.GetComponent<RectTransform>().rect.width;

        if(container.active){
            //Vector2 newSize = new Vector2(255, 300);

            var width = container.GetComponent<RectTransform>().rect.width;
            var height = container.GetComponent<RectTransform>().rect.height;

            Vector2 newSize = new Vector2(width/3, width/3 + 200);

            this.GetComponent<GridLayoutGroup>().cellSize = newSize;

        }




    }

   
}

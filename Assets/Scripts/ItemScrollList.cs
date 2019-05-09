using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item{
    public string itemName;
}


public class ItemScrollList : MonoBehaviour
{

    public List<Item> itemList;
    public Transform contentPanel;
    public ItemScrollList otherScrollList;
    public Text nameDisplay;
    public SimpleObjectPool buttonObjectPool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

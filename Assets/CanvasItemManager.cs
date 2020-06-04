using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasItemManager : MonoBehaviour
{
    public GameObject ObjectList;
    public GameObject Container;
    public GameObject[] Items;
    private float interval;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= Container.transform.childCount; i++)
        {
            Container.transform.GetChild(i - 1).localScale = new Vector3(40, 40, 40);
            Container.transform.GetChild(i - 1).transform.localRotation = new Quaternion(-1 ,0, 0, transform.rotation.w);
            Container.transform.GetChild(i - 1).transform.position = new Vector3(Container.transform.GetChild(i - 1).transform.position.x,Container.transform.GetChild(i - 1).transform.position.y - interval,Container.transform.GetChild(i - 1).transform.position.z);
            interval += 5;
        }
        
        
        for (int i = 1; i <= Container.transform.childCount; i++)
        {
            //Container.transform.GetChild(i - 1).transform.position = new Vector3(0,Container.transform.GetChild(i - 1).transform.position.y,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}

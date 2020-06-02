using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClckRender : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public GameObject Camera;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log (name + "Game Object Down!");
        Camera.GetComponent<GameScreen>().isScroll = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Camera.GetComponent<GameScreen>().isScroll = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PacketObjectManager : MonoBehaviour
{
    public float statelocal;
    public GameObject slider;
    // Start is called before the first frame update
    void Start()
    {
        statelocal = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.GetComponent<Scrollbar>().value > statelocal)
        {
            statelocal = slider.GetComponent<Scrollbar>().value;
            transform.position = Vector3.up * 0.001f * Time.deltaTime;
        }
        else if (slider.GetComponent<Scrollbar>().value < statelocal)
        {
            statelocal = slider.GetComponent<Scrollbar>().value;
            transform.position = Vector3.down * 0.001f * Time.deltaTime;
        }
    }
}

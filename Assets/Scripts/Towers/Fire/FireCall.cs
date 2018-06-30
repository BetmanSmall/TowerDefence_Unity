using System.Collections;
using System.Collections.Generic;
using DigitalRuby.ThunderAndLightning;
using UnityEngine;

public class FireCall : MonoBehaviour {

    // public Animator power;
	// public GameObject Source;
    // public GameObject Destination;

	// Use this for initialization

    private GameObject fire;
    private GameObject firesmall;

	void Start(){
        fire = gameObject.transform.Find("Body").gameObject.transform.Find("Fire").gameObject;
        firesmall = gameObject.transform.Find("Body").gameObject.transform.Find("FireSmall").gameObject;
        fire.SetActive(false);
    }
	 void OnTriggerEnter(Collider other)
    {
      if(other.gameObject.transform.parent.name == "Creeps"){
        Vector3 direction = gameObject.transform.position - other.gameObject.transform.position; 
        float angle = Vector3.Angle(direction,gameObject.transform.forward);
        float Speed = 3.0f;
        Vector3 newDir = -Vector3.RotateTowards(gameObject.transform.forward, direction,angle,0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(newDir);
     
        fire.SetActive(true);
        firesmall.SetActive(false);
			// gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Destination = other.gameObject;
            // gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().enabled = true;

            
            // other.gameObject.GetComponent<Creep>().die(100);
            }
    }

	void OnTriggerExit(Collider other)
    {
        fire.SetActive(false);
        firesmall.SetActive(true);
    //   if(other.gameObject.transform.parent.name == "Creeps"){
    //         gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Source = null;
	// 		gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Destination = null;
    //         gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().enabled = false;
    //         }
    }
		


    void Update(){
       
	}
}


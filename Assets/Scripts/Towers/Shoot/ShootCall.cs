using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.ThunderAndLightning;
using UnityEngine;

public class ShootCall : MonoBehaviour {

    // public Animator power;
	// public GameObject Source;
    // public GameObject Destination;

	// Use this for initialization

    // private GameObject fire;
    // private GameObject firesmall;
    private GameObject cripik;
    public GameObject pula;
    private Animator animator;

    private float speed;
    private DateTime time;
    private TimeSpan NowTime;

	void Start(){
        time = DateTime.Now;
        
        // fire = gameObject.transform.Find("Body").gameObject.transform.Find("Fire").gameObject;
        // firesmall = gameObject.transform.Find("Body").gameObject.transform.Find("FireSmall").gameObject;
        // fire.SetActive(false);
        animator = gameObject.GetComponent<Animator>();
        speed = 5.0f;
    }
	 void OnTriggerEnter(Collider other)
    {
      if(other.gameObject.transform.parent.name == "Creeps"){
        cripik = other.gameObject;
        animator.SetBool("Shoot",true);
        Vector3 direction = gameObject.transform.position - other.gameObject.transform.position; 
        float angle = Vector3.Angle(direction,gameObject.transform.forward);
        float Speed = 3.0f;
        Vector3 newDir = -Vector3.RotateTowards(gameObject.transform.forward, direction,angle,0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(newDir);
        GameObject patron1 = Instantiate(pula, gameObject.transform.Find("Head").gameObject.transform.Find("Cannon_1").gameObject.transform.Find("Right").gameObject.transform.position, Quaternion.identity);
        patron1.GetComponent<Rigidbody>().velocity = gameObject.transform.Find("Head").gameObject.transform.Find("Cannon_1").gameObject.transform.Find("Right").gameObject.transform.forward*speed;
        GameObject patron2 = Instantiate(pula, gameObject.transform.Find("Head").gameObject.transform.Find("Cannon_2").gameObject.transform.Find("Left").gameObject.transform.position, Quaternion.identity);
        patron2.GetComponent<Rigidbody>().velocity = gameObject.transform.Find("Head").gameObject.transform.Find("Cannon_2").gameObject.transform.Find("Left").gameObject.transform.forward*speed;
        
        // fire.SetActive(true);
        // firesmall.SetActive(false);
			// gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Destination = other.gameObject;
            // gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().enabled = true;

            
            // other.gameObject.GetComponent<Creep>().die(100);
            }
    }

    void Pulemet(){
        GameObject patron1 = Instantiate(pula, gameObject.transform.Find("Head").gameObject.transform.Find("Cannon_1").gameObject.transform.Find("Right").gameObject.transform.position, Quaternion.identity);
        patron1.GetComponent<Rigidbody>().velocity = gameObject.transform.Find("Head").gameObject.transform.Find("Cannon_1").gameObject.transform.Find("Right").gameObject.transform.forward*speed;
        GameObject patron2 = Instantiate(pula, gameObject.transform.Find("Head").gameObject.transform.Find("Cannon_2").gameObject.transform.Find("Left").gameObject.transform.position, Quaternion.identity);
        patron2.GetComponent<Rigidbody>().velocity = gameObject.transform.Find("Head").gameObject.transform.Find("Cannon_2").gameObject.transform.Find("Left").gameObject.transform.forward*speed;
        Destroy(patron1,2.0f);
        Destroy(patron2,2.0f);
    }
    
    void OnTriggerStay(Collider other)
    {

         if(other.gameObject.transform.parent.name == "Creeps"){
        animator.SetBool("Shoot",true);
        NowTime = DateTime.Now - time;
        if(NowTime.Seconds/2 ==0){
        Invoke("Pulemet",1.0f);
        }
        // fire.SetActive(true);
        // firesmall.SetActive(false);
			// gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Destination = other.gameObject;
            // gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().enabled = true;

            
            // other.gameObject.GetComponent<Creep>().die(100);
         }
    }


	void OnTriggerExit(Collider other)
    {
         animator.SetBool("Shoot",false);
         
        // fire.SetActive(false);
        // firesmall.SetActive(true);
    //   if(other.gameObject.transform.parent.name == "Creeps"){
    //         gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Source = null;
	// 		gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Destination = null;
    //         gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().enabled = false;
    //         }
    }
		


    void Update(){
       if(cripik == null){
           animator.SetBool("Shoot",false);
	}
	}
}


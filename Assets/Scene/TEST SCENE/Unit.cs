using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour {
      public GameObject Creeps;
    public GameObject Creep1;
	public GameObject Creep2;
	public GameObject Creep3;
	public GameObject Creep4;
	public GameObject Creep5;
	public GameObject Creep6;

	 public NavMeshAgent agent;

	public Camera camera;
    
	// Use this for initialization
	void Start () {
	//	 surface = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();
     Creeps = GameObject.Find("Creeps");
     //   surface.BuildNavMesh();
	}





	
	// Update is called once per frame
	void Update () {
	if (Input.GetMouseButtonDown(0)){
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		
		 RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
        if(hit.transform.gameObject.name == "Cube"){
			    GameObject creepik = (GameObject)Instantiate(Creep1, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity, Creeps.transform);
				creepik.AddComponent<NavMeshAgent>();
				NavMeshAgent dir = creepik.GetComponent<NavMeshAgent>();
				dir.SetDestination(new Vector3(hit.point.x + 30, hit.point.y, hit.point.z));	
			}
		}

			
		}
		
		// if(Physics.Raycast(ray, out hit)){
			
		// 	if(hit.transform.parent.gameObject.transform.parent.name == "Nature"){
		// 		Debug.Log(hit.transform.parent.name);
		// 	//GameObject creepik = Instantiate(Creep1,hit.transform.gameObject.transform.position,Quaternion.identity);
		// }

		}
	}	
	
	


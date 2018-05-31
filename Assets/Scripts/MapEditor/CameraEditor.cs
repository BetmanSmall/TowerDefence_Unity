using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEditor : MonoBehaviour {
    private Camera cameraGame;
    // private InspectCell InCell;

    void Start() {
        cameraGame = GetComponent<Camera>();
    }

    void Update() {
        Ray ray = cameraGame.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            
            bool isMouseDown = Input.GetMouseButtonDown(0);
            bool isMouseUp = Input.GetMouseButtonUp(0);

            // InCell = hit.collider.gameObject.GetComponent<InspectCell>();

            if (hit.collider.gameObject) {
                if (isMouseDown || isMouseUp) {
                    if (isMouseDown) {
                        
                    }
                } 
//                else {
//                    if (curCell != null) {
//                        curCell.MouseExit();
//                    }
//                    click.MouseEnter();
//                    curCell = click;
                }
            }
        }
    }

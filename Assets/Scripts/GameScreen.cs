using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameScreen : MonoBehaviour  {
    // DrawCameraGrid  && WhichCell
    private Material gridLineMaterial; // = Resources.Load<Material>("Materials/GridLineMaterial"); // this not work!=(
    public float drawOnY = 0.4f; // Приподнять отрисовку сетки над GameField'ом
    private GameObject gameFieldObject;
    private GameField gameField;

//    Camera camera;
    public float spaceMouseDetection = 5f;
    public float cameraSpeed = 5.0f;
    public float mouseSensitivity = 0.05f;
    private Vector3 lastPosition;

    void Start() {
//        camera = GetComponent<Camera>();
        print ("GameScreen::Start(); -- Start!");
        gridLineMaterial = Resources.Load<Material>("Materials/GridLineMaterial");
        // print ("GameScreen::Start(); -- gridLineMaterial:" + gridLineMaterial);
        gameFieldObject = GameObject.Find ("GameField");
        print ("GameScreen::Start(); -- gameFieldObject:" + gameFieldObject);
        if (gameFieldObject == null) {
            Debug.LogError ("GameScreen::Start(); -- Not found GameField<GameObject> in hierarchy");
            return;
        } else {
            gameField = gameFieldObject.GetComponent<GameField>();
            print ("GameScreen::Start(); -- gameField:" + gameField);
        }
    }

    void OnPostRender() {
//        print ("GameScreen::OnPostRender(); -- Start!");
        drawGrid();
    }

    void OnDrawGizmos() {
//        print ("GameScreen::OnDrawGizmos(); -- Start!");
        drawGrid();
    }

    void drawGrid() {
//        print ("GameScreen::drawGrid(); -- gameField:" + gameField);
        if (gameField != null) {
            int sizeFieldX = gameField.sizeFieldX;
            int sizeFieldZ = gameField.sizeFieldZ;
            float sizeCellX = gameField.sizeCellX;
            float sizeCellZ = gameField.sizeCellZ;
//            print ("GameScreen::drawGrid(); -- gameField.sizeFieldX:" + gameField.sizeFieldX + " gameField.sizeFieldZ:" + gameField.sizeFieldZ);
//            print ("GameScreen::drawGrid(); -- gameField.sizeCellX:" + gameField.sizeCellX + " gameField.sizeCellZ:" + gameField.sizeCellZ);
            for (int x = 0; x <= sizeFieldX; x++) {
                drawLine (x*sizeCellX, 0, x*sizeCellX, sizeFieldX*sizeCellZ);
            }
            for (int z = 0; z <= sizeFieldZ; z++) {
                drawLine (0, z*sizeCellZ, sizeFieldZ*sizeCellX, z*sizeCellZ);
            }
        }
    }

    void drawLine(float x1, float z1, float x2, float z2) {
        GL.Begin(GL.LINES);
        gridLineMaterial.SetPass(0);
        GL.Vertex3(x1, drawOnY, z1);
        GL.Vertex3(x2, drawOnY, z2);
        GL.End();
    }

    void Update() {
//        Debug.Log ("GameScreen::Update(); -- Start!");
        if (0f < Input.mousePosition.x && Input.mousePosition.x < spaceMouseDetection) {
            transform.position -= new Vector3 (cameraSpeed * Time.deltaTime, 0, 0);
        }
        if ((Screen.width-spaceMouseDetection) < Input.mousePosition.x && Input.mousePosition.x < Screen.width) {
            transform.position += new Vector3 (cameraSpeed * Time.deltaTime, 0, 0);
        }
        if (0f < Input.mousePosition.y && Input.mousePosition.y < spaceMouseDetection) {
            transform.position -= new Vector3 (0, 0, cameraSpeed * Time.deltaTime);
        }
        if ((Screen.height-spaceMouseDetection) < Input.mousePosition.y && Input.mousePosition.y < Screen.height) {
            transform.position += new Vector3 (0, 0, cameraSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            transform.Translate(new Vector3(cameraSpeed*Time.deltaTime, 0, 0));
        }
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            transform.Translate(new Vector3((-cameraSpeed*Time.deltaTime), 0, 0));
        }
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            transform.Translate(new Vector3(0, -(cameraSpeed*Time.deltaTime), 0));
        }
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            transform.Translate(new Vector3(0, cameraSpeed*Time.deltaTime, 0));
        }



//      Управление камерой при нажатой клавиши мыши           -------------------------------------------------------------------------------- 
        if (Input.GetButtonDown("Fire1") || Input.GetMouseButtonDown(0)) { // Работает только вместе
             lastPosition = Input.mousePosition;
             Debug.Log ("Нажали0");
        }
        if (Input.GetButton("Fire1") || Input.GetMouseButton(0)) { // Работает только вместе
            
            Vector3 delta = Input.mousePosition - lastPosition;
//            Debug.Log ("GameScreen::Update(); -- delta:" + delta);
//            transform.Translate(delta.x * mouseSensitivity, 0f, delta.y * mouseSensitivity);
            Vector3 olsPos = transform.position;
            Vector3 newPos = new Vector3 (olsPos.x - (delta.x * mouseSensitivity), olsPos.y, olsPos.z - (delta.y * mouseSensitivity));
            transform.position = newPos;
            lastPosition = Input.mousePosition;
        }
//----------------------------------------------------------------------------------------------------------------------------------------------

        if (Input.GetMouseButtonUp(0)) {
            Debug.Log ("GameScreen::Update(); -- Input.GetMouseButtonUp(1):" + Input.GetMouseButtonUp(0));
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                Debug.Log ("GameScreen::Update(); -- hit.collider.gameObject:" + hit.collider.gameObject);
                Debug.Log ("GameScreen::Update(); -- hit.transform.position:" + hit.transform.position);
                Cell cell = hit.collider.gameObject.GetComponentInParent<Cell>();
                if(cell != null) {
                    Debug.Log ("GameScreen::Update(); -- cell:" + ((!cell.empty) ? ("[" + cell.gameX + ", " + cell.gameZ + "])") : ("cell.empty == true")) );
                    gameField.towerActions(cell.gameX, cell.gameZ);
                }
            }
        }
        if (Input.GetMouseButtonUp(1)) {
            Debug.Log ("GameScreen::Update(); -- Input.GetMouseButtonUp(1):" + Input.GetMouseButtonUp(1));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Debug.Log ("GameScreen::Update(); -- hit.collider.gameObject:" + hit.collider.gameObject);
                Debug.Log ("GameScreen::Update(); -- hit.transform.position:" + hit.transform.position);
                Cell cell = hit.collider.gameObject.GetComponentInParent<Cell>();
                if(cell != null) {
                    Debug.Log ("GameScreen::Update(); -- cell:" + ((!cell.empty) ? ("[" + cell.gameX + ", " + cell.gameZ + "])") : ("cell.empty == true")) );
                    gameField.createCreep(cell.gameX, cell.gameZ);
                }
            }
        }

        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta != 0) {
            Debug.Log ("GameScreen::Update(); -- scrollDelta:" + scrollDelta + " Need implement zoom!");
            transform.position.Scale(transform.position*scrollDelta);
        }
        
//        Vector3 vp = camera.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane));
//        vp.x -= 0.5f;
//        vp.y -= 0.5f;
//        vp.x *= mouseSensitivity;
//        vp.y *= mouseSensitivity;
//        vp.x += 0.5f;
//        vp.y += 0.5f;
//        Vector3 sp = camera.ViewportToScreenPoint(vp);
//         
//        Vector3 v = camera.ScreenToWorldPoint(sp);
//        transform.LookAt(v, Vector3.up);
    }

//     public void OnPointerClick(PointerEventData pointerEventData) {
//         Debug.Log ("GameScreen::OnPointerClick(); -- pointerEventData:" + pointerEventData);
//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         RaycastHit hit;
//         if (Physics.Raycast(ray, out hit)) {
// //            Debug.Log ("GameScreen::Update(); -- hit:" + hit);
//             bool isMouseDown = Input.GetButtonDown("Fire1");
// //            bool isMouseUp = Input.GetMouseButtonUp(0);
// //            ClickCell click = hit.collider.gameObject.GetComponent<ClickCell>();
// //            Debug.Log ("GameScreen::Update(); -- hit.collider:" + hit.collider);
//             if (hit.collider.gameObject) {
//                 if (isMouseDown) {
//                     Debug.Log ("GameScreen::Update(); -- hit.collider.gameObject:" + hit.collider.gameObject);
//                     Debug.Log ("GameScreen::Update(); -- hit.transform.position:" + hit.transform.position);
//                     Cell cell = hit.collider.gameObject.GetComponentInParent<Cell>();
//                     if(cell != null) {
//                         Debug.Log ("GameScreen::Update(); -- cell:" + cell + " cell.setTerrain();");
//                         if(cell.isTerrain()) {
//                             cell.setTerrain(); // need reWrite in future! All codes need reWrite!
//                         } else {
//                             cell.removeTerrain();
//                         }
//                     }
//                 } 
// //                else {
// //                    if (curCell != null) {
// //                        curCell.MouseExit();
// //                    }
// //                    click.MouseEnter();
// //                    curCell = click;
// //                }
//             }
//         }
//     }
}

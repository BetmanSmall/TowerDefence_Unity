using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
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

    public float distanceMin = 5f;
    public float distanceMax = 40f;
    public float zoomSpeed = 100f;

    void Start() {
//        camera = GetComponent<Camera>();
        Debug.Log("GameScreen::Start(); -- Start!");
        gridLineMaterial = Resources.Load<Material>("Materials/GridLineMaterial");
        // Debug.Log("GameScreen::Start(); -- gridLineMaterial:" + gridLineMaterial);
        gameFieldObject = GameObject.Find("GameField");
        Debug.Log("GameScreen::Start(); -- gameFieldObject:" + gameFieldObject);
        if (gameFieldObject == null) {
            Debug.LogError ("GameScreen::Start(); -- Not found GameField<GameObject> in hierarchy");
            return;
        } else {
            gameField = gameFieldObject.GetComponent<GameField>();
            Debug.Log("GameScreen::Start(); -- gameField:" + gameField);
        }
    }

    void OnPostRender() {
        // Debug.Log("GameScreen::OnPostRender(); -- Start!");
        drawGrid();
        // Debug.Log("GameScreen::OnPostRender(); -- test1!");
        drawCreepsPaths();
        // drawWaveAlgorithmNumbers();
    }

    void OnDrawGizmos() {
        // Debug.Log("GameScreen::OnDrawGizmos(); -- Start!");
        drawGrid();
        // Debug.Log("GameScreen::OnDrawGizmos(); -- test1!");
        drawCreepsPaths();
        // drawWaveAlgorithmNumbers();
    }

    void drawGrid() {
    //    Debug.Log("GameScreen::drawGrid(); -- gameField:" + gameField);
        if (gameField != null) {
            int sizeFieldX = gameField.sizeFieldX;
            int sizeFieldZ = gameField.sizeFieldZ;
            float sizeCellX = gameField.sizeCellX;
            float sizeCellZ = gameField.sizeCellZ;
        //    Debug.Log("GameScreen::drawGrid(); -- gameField.sizeFieldX:" + gameField.sizeFieldX + " gameField.sizeFieldZ:" + gameField.sizeFieldZ);
        //    Debug.Log("GameScreen::drawGrid(); -- gameField.sizeCellX:" + gameField.sizeCellX + " gameField.sizeCellZ:" + gameField.sizeCellZ);
            for (int z = 0; z <= sizeFieldZ; z++) {
                drawLine (0, z*sizeCellZ, sizeFieldZ*sizeCellX, z*sizeCellZ);
            }
            for (int x = 0; x <= sizeFieldX; x++) {
                drawLine (x*sizeCellX, 0, x*sizeCellX, sizeFieldX*sizeCellZ);
            }
            for (int z = 0; z <= sizeFieldZ; z++) {
                for (int x = 0; x <= sizeFieldX; x++) {
                    drawLine (0, z*sizeCellZ, sizeFieldZ*sizeCellX, z*sizeCellZ);
                    // drawLine (x*sizeCellX, 0, x*sizeCellX, sizeFieldX*sizeCellZ);
                }
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

    void drawCreepsPaths() {
        if(gameField != null && gameField.creepsManager != null) {
            List<Creep> creeps = gameField.creepsManager.getAllCreeps();
            foreach (Creep creep in creeps) {
                var nav = creep.gameObject.GetComponent<NavMeshAgent>();
                if (nav == null || nav.path == null) {
                    return;
                }
                var line = creep.gameObject.GetComponent<LineRenderer>();
                if (line == null) {
                    line = creep.gameObject.AddComponent<LineRenderer>();
                    line.material = new Material( Shader.Find( "Sprites/Default" ) ) { color = Color.yellow };
                    line.SetWidth(0.5f, 0.5f);
                    line.SetColors(Color.yellow, Color.yellow);
                }
                var path = nav.path;
                line.SetVertexCount(path.corners.Length);
                for( int i = 0; i < path.corners.Length; i++ ) {
                    line.SetPosition(i, path.corners[i]);
                }
                // Debug.Log("GameScreen::OnDrawGizmosSelected(); -- creep:" + creep + " nav:" + nav + " line:" + line);
            }
        }
    }

    void drawWaveAlgorithmNumbers() {
    //    Debug.Log("GameScreen::drawWaveAlgorithmNumbers(); -- gameField:" + gameField);
        if (gameField != null) {
            int sizeFieldX = gameField.sizeFieldX;
            int sizeFieldZ = gameField.sizeFieldZ;
            float sizeCellX = gameField.sizeCellX;
            float sizeCellZ = gameField.sizeCellZ;
            // Debug.Log("GameScreen::drawWaveAlgorithmNumbers(); -- gameField.sizeFieldX:" + gameField.sizeFieldX + " gameField.sizeFieldZ:" + gameField.sizeFieldZ);
            // Debug.Log("GameScreen::drawWaveAlgorithmNumbers(); -- gameField.sizeCellX:" + gameField.sizeCellX + " gameField.sizeCellZ:" + gameField.sizeCellZ);
            for (int x = 0; x < sizeFieldX; x++) {
                for (int z = 0; z < sizeFieldZ; z++) {
                    Cell cell = gameField.field[x, z];
                    if(cell != null) {
                        Vector3 pos = new Vector3(x * sizeCellX + sizeCellX/3f, 0f, z * sizeCellZ + sizeCellZ/1.5f); // все тут нужно понять | magic numbers forever
                        int numberByWaveAlgorithm = gameField.field[x, z].numberByWaveAlgorithm;
                        // string outStr = "(" + x + "," + z + "):" + numberByWaveAlgorithm.ToString();
                        string outStr = numberByWaveAlgorithm.ToString();
                        Handles.Label(pos, outStr);
                    }
                    // Debug.Log("GameScreen::drawWaveAlgorithmNumbers(); -- x:" + x + " z:" + z + " pos:" + pos + " gameField:" + gameField);
                    // Debug.Log("GameScreen::drawWaveAlgorithmNumbers(); -- gameField.field:" + gameField.field);
                    // Debug.Log("GameScreen::drawWaveAlgorithmNumbers(); -- x:" + x + " z:" + z + " pos:" + pos + " cell:" + cell);
                }
            }
        }
    }

    void Update() {
//        Debug.Log("GameScreen::Update(); -- Start!");
        float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollWheel != 0f ) {
            Debug.Log("GameScreen::Update(); -- mouseScrollWheel:" + mouseScrollWheel + " transform.position.y:" + transform.position.y);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit point; Physics.Raycast(ray, out point, 25);
            Vector3 Scrolldirection = ray.GetPoint(5);

            float step = zoomSpeed * Time.deltaTime;
            if (mouseScrollWheel < 0f && transform.position.y < distanceMax) {
                transform.position = Vector3.MoveTowards(transform.position, Scrolldirection, mouseScrollWheel * step);
            } else if (mouseScrollWheel > 0f && transform.position.y > distanceMin) {
                transform.position = Vector3.MoveTowards(transform.position, Scrolldirection, mouseScrollWheel * step);
            }
            Debug.Log("GameScreen::Update(); -- mouseScrollWheel:" + mouseScrollWheel + " transform.position.y:" + transform.position.y);
        }

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
//            Debug.Log("GameScreen::Update(); -- delta:" + delta);
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
                Debug.Log ("GameScreen::Update(); -- cell:" + cell);
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

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour {
    // DrawCameraGrid  && WhichCell
    private Material gridLineMaterial; // = Resources.Load<Material>("Materials/GridLineMaterial"); // this not work!=(
    public float drawOnY = 0.4f; // Приподнять отрисовку сетки над GameField'ом
    private GameObject gameFieldObject;
    private GameField gameField;

//   OLD Camera camera;
    public float spaceMouseDetection = 5f;
    public float cameraSpeed = 5.0f;
    public float mouseSensitivity = 0.05f;
    private Vector3 lastPosition;

    public float distanceMin = 5f;
    public float distanceMax = 40f;
    public float zoomSpeed = 100f;
    public bool isScroll;

    //NEW CAMERA
    float mainSpeed = 100.0f; //regular speed
    float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when holdin gshift
    float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;

    public float rayDistance;
    [SerializeField] Text nameTile;

    [SerializeField] private GameObject previewTileModel;

    public LayerMask layerMaskTileModel;

    void Start() {
//        camera = GetComponent<Camera>();
        Debug.Log("GameScreen::Start(); -- Start!");
        gridLineMaterial = Resources.Load<Material>("Materials/GridLineMaterial");
        // Debug.Log("GameScreen::Start(); -- gridLineMaterial:" + gridLineMaterial);
        gameFieldObject = GameObject.Find("GameField");
        Debug.Log("GameScreen::Start(); -- gameFieldObject:" + gameFieldObject);
        if (gameFieldObject == null) {
            Debug.LogError("GameScreen::Start(); -- Not found GameField<GameObject> in hierarchy");
            return;
        } else {
            gameField = gameFieldObject.GetComponent<GameField>();
            Debug.Log("GameScreen::Start(); -- gameField:" + gameField);
        }

        //layerMaskTileModel = LayerMask.GetMask("FloorTileModel");
    }

    void OnPostRender() {
        // Debug.Log("GameScreen::OnPostRender(); -- Start!");
        // drawGrid();
        // Debug.Log("GameScreen::OnPostRender(); -- test1!");
        drawCreepsPaths();
        // drawWaveAlgorithmNumbers();
    }

    void OnDrawGizmos() {
        // Debug.Log("GameScreen::OnDrawGizmos(); -- Start!");
        // drawGrid();
        // Debug.Log("GameScreen::OnDrawGizmos(); -- test1!");
        drawCreepsPaths();
        // drawWaveAlgorithmNumbers();
    }

    void drawGrid() {
        if (gameField != null) {
            int sizeFieldX = gameField.sizeFieldX;
            int sizeFieldZ = gameField.sizeFieldZ;
            float sizeCellX = gameField.sizeCellX;
            float sizeCellZ = gameField.sizeCellZ;
            for (int z = 0; z <= sizeFieldZ; z++) {
                drawLine(0, z * sizeCellZ, sizeFieldZ * sizeCellX, z * sizeCellZ);
            }
            for (int x = 0; x <= sizeFieldX; x++) {
                drawLine(x * sizeCellX, 0, x * sizeCellX, sizeFieldX * sizeCellZ);
            }
            for (int z = 0; z <= sizeFieldZ; z++) {
                for (int x = 0; x <= sizeFieldX; x++) {
                    drawLine(0, z * sizeCellZ, sizeFieldZ * sizeCellX, z * sizeCellZ);
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
        if (gameField != null && gameField.creepsManager != null) {
            List<Creep> creeps = gameField.creepsManager.getAllCreeps();
            foreach (Creep creep in creeps) {
                var nav = creep.gameObject.GetComponent<NavMeshAgent>();
                if (nav == null || nav.path == null) {
                    return;
                }
                var line = creep.gameObject.GetComponent<LineRenderer>();
                if (line == null) {
                    line = creep.gameObject.AddComponent<LineRenderer>();
                    line.material = new Material(Shader.Find("Sprites/Default")) {color = Color.yellow};
                    line.SetWidth(0.5f, 0.5f);
                    line.SetColors(Color.yellow, Color.yellow);
                }
                var path = nav.path;
                line.SetVertexCount(path.corners.Length);
                for (int i = 0; i < path.corners.Length; i++) {
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
                    if (cell != null) {
                        Vector3 pos = new Vector3(x * sizeCellX + sizeCellX / 3f, 0f, z * sizeCellZ + sizeCellZ / 1.5f); // все тут нужно понять | magic numbers forever
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

    public Vector3 GetBaseInput() {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }

    void Update() {
        if (!isScroll) {
            float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
            if (mouseScrollWheel != 0f) {
                Debug.Log("GameScreen::Update(); -- mouseScrollWheel:" + mouseScrollWheel + " transform.position.y:" + transform.position.y);
                RaycastHit point;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out point, 25);
                Vector3 Scrolldirection = ray.GetPoint(5);
                float step = zoomSpeed * Time.deltaTime;
                if (mouseScrollWheel < 0f && transform.position.y < distanceMax) {
                    transform.position = Vector3.MoveTowards(transform.position, Scrolldirection, mouseScrollWheel * step);
                } else if (mouseScrollWheel > 0f && transform.position.y > distanceMin) {
                    transform.position = Vector3.MoveTowards(transform.position, Scrolldirection, mouseScrollWheel * step);
                }
                Debug.Log("GameScreen::Update(); -- mouseScrollWheel:" + mouseScrollWheel + " transform.position.y:" + transform.position.y);
            }
            if (Input.GetButtonDown("Fire1") || Input.GetMouseButtonDown(0)) {
                lastPosition = Input.mousePosition;
                Debug.Log("lastPosition:" + lastPosition);
            }
            if (Input.GetButton("Fire1") || Input.GetMouseButton(0)) {
                Vector3 delta = Input.mousePosition - lastPosition;
                Vector3 olsPos = transform.position;
                Vector3 newPos = new Vector3(olsPos.x - (delta.x * mouseSensitivity), olsPos.y, olsPos.z - (delta.y * mouseSensitivity));
                transform.position = newPos;
                lastPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(1)) {
                lastMouse = Input.mousePosition - lastMouse;
                lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
                lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
                transform.eulerAngles = lastMouse;
                lastMouse = Input.mousePosition;
                float f = 0.0f;
                Vector3 p = GetBaseInput();
                if (Input.GetKey(KeyCode.LeftShift)) {
                    totalRun += Time.deltaTime;
                    p = p * totalRun * shiftAdd;
                    p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                    p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                    p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
                } else if (Input.GetKey(KeyCode.LeftControl)) {
                    totalRun += Time.deltaTime;
                    p = p * totalRun * 50f;
                    p.x = Mathf.Clamp(p.x, -500, 500);
                    p.y = Mathf.Clamp(p.y, -500, 500);
                    p.z = Mathf.Clamp(p.z, -500, 500);
                } else {
                    totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                    p = p * mainSpeed;
                }
                p = p * Time.deltaTime;
                Vector3 newPosition = transform.position;
                if (Input.GetKey(KeyCode.Space)) {
                    //If player wants to move on X and Z axis only
                    transform.Translate(p);
                    newPosition.x = transform.position.x;
                    newPosition.z = transform.position.z;
                    transform.position = newPosition;
                }
                if (Input.GetKey(KeyCode.Q)) {
                    transform.Translate(Vector3.up * 20 * Time.deltaTime);
                } else if (Input.GetKey(KeyCode.E)) {
                    transform.Translate(Vector3.down * 20 * Time.deltaTime);
                } else {
                    transform.Translate(p);
                }
            }
            if (Input.GetMouseButtonUp(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.Log("GameScreen::Update(); -- ray:" + ray);
                Debug.DrawRay(transform.position, ray.direction * rayDistance);
                if (Physics.Raycast(ray, out RaycastHit raycastHit)) {
                    Debug.Log("GameScreen::Update(); -- raycastHit:" + raycastHit);
                    Debug.Log("GameScreen::Update(); -- raycastHit.collider:" + raycastHit.collider);
                    GameObject raycastHitOb = raycastHit.collider.gameObject;
                    Debug.Log("GameScreen::Update(); -- raycastHitOb:" + raycastHitOb);
                    GameObject parentOb = raycastHitOb.transform.parent.gameObject;
                    Debug.Log("GameScreen::Update(); -- parentOb:" + parentOb);
                    Cell cell = parentOb.GetComponent<Cell>();
                    Debug.Log("GameScreen::Update(); -- cell:" + cell);
                    if (cell != null) {
                        Debug.Log("GameScreen::Update(); -- cell:" + ((!cell.empty)
                                      ? ("[" + cell.gameX + ", " + cell.gameZ + "])")
                                      : ("cell.empty == true")));
                        // gameField.towerActions(cell.gameX, cell.gameZ);
                        Debug.Log("GameScreen::Update(); -- previewTileModel.transform.childCount:" + previewTileModel.transform.childCount);
                        if (previewTileModel.transform.childCount > 0) {
                            Debug.Log("GameScreen::Update(); -- previewTileModel.transform.GetChild(0):" + previewTileModel.transform.GetChild(0));
                            GameObject newObject = previewTileModel.transform.GetChild(0).gameObject;
                            if (newObject != null) {
                                GameObject newCellGameObject = gameField.createTileGameObjectCell(newObject, raycastHitOb.transform.position, cell.gameX, cell.layerY, cell.gameZ);
                                // GameObject newCellGameObject = (GameObject) Instantiate(newObject, raycastHitOb.transform.position, Quaternion.identity, parentOb.transform);
                                newCellGameObject.transform.localScale = raycastHit.transform.localScale;
                                Destroy(raycastHitOb);
                            }
                        }
                    } else {
                        Debug.Log("GameScreen::Update(); -- raycastHitOb:" + raycastHitOb);
                        Debug.Log("GameScreen::Update(); -- raycastHit.transform.position:" + raycastHit.transform.position);
                        Debug.Log("GameScreen::Update(); -- raycastHit.transform.parent.name:" + raycastHit.transform.parent.name);
                        Debug.Log("GameScreen::Update(); -- raycastHit:" + raycastHit);
                        Debug.Log("GameScreen::Update(); -- raycastHit:" + raycastHit.collider);
                        Debug.Log("GameScreen::Update(); -- raycastHit:" + raycastHit.transform);
                        nameTile.text = "Name: " + raycastHit.transform.parent.name;
                        for (int i = 0; i < previewTileModel.transform.childCount; i++) {
                            Destroy(previewTileModel.transform.GetChild(0).gameObject);
                        }
                        GameObject go = Instantiate(parentOb, previewTileModel.transform.position, parentOb.transform.rotation);
                        go.GetComponent<Transform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        go.transform.SetParent(previewTileModel.transform);
                    }
                }
            }
            if (Input.GetMouseButtonUp(1)) {
                Debug.Log("GameScreen::Update(); -- Input.GetMouseButtonUp(1):" + Input.GetMouseButtonUp(1));
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    Debug.Log("GameScreen::Update(); -- hit.collider.gameObject:" + hit.collider.gameObject);
                    Debug.Log("GameScreen::Update(); -- hit.transform.position:" + hit.transform.position);
                    Cell cell = hit.collider.gameObject.GetComponentInParent<Cell>();
                    if (cell != null) {
                        Debug.Log("GameScreen::Update(); -- cell:" + ((!cell.empty)
                                      ? ("[" + cell.gameX + ", " + cell.gameZ + "])")
                                      : ("cell.empty == true")));
                        gameField.createCreep(cell.gameX, cell.gameZ);
                    }
                }
            }
        }
    }
}

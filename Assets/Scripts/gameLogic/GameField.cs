using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour {
    // public WaveManager waveManager; // ALL public for all || we are friendly :)
    // public static CreepsManager creepsManager; // For Shell
    private TowersManager towersManager;
    private FactionsManager factionsManager;
    public string mapPath = "maps/arena0";

    public int sizeFieldX, sizeFieldZ;
    public float sizeCellX=3f, sizeCellY=0.3f, sizeCellZ=3f; // need change, load from map
    public Cell[,] field;

    // GAME INTERFACE ZONE1
    // private WhichCell whichCell;
    public bool gamePaused;
    public float gameSpeed;
    public static int gamerGold = 1000000; // For Shell
    // public int maxOfMissedCreepsForComputer0;
    // public int missedCreepsForComputer0;
    // public int maxOfMissedCreepsForPlayer1;
    // public int missedCreepsForPlayer1;
    // GAME INTERFACE ZONE2

    // Use this for initialization
    void Start() {
        Debug.Log("GameField::Start(); -- Start!");
        
        // waveManager = new WaveManager();
        // creepsManager = new CreepsManager();
        towersManager = new TowersManager();
        factionsManager = new FactionsManager(1f);
        factionsManager.loadFactions();

        Map map = new MapLoader().loadMap(mapPath);

        sizeFieldX = int.Parse(map.properties ["width"]);
        sizeFieldZ = int.Parse(map.properties ["height"]);

        createField (sizeFieldX, sizeFieldZ, map.mapLayers);
        Debug.Log("GameField::Start(); -- End!");
    }

    private void createField(int sizeFieldX, int sizeFieldZ, Dictionary<int, MapLayer> mapLayers) {
    Debug.Log("GameField::createField(" + sizeFieldX + ", " + sizeFieldZ + ", " + mapLayers + "); -- field:" + field);
        if (field == null) {
            field = new Cell[sizeFieldX, sizeFieldZ];
            //foreach (MapLayer mapLayer in mapLayers.Values) {
            for (int layerY = 0; layerY < mapLayers.Count; layerY++) {
                MapLayer mapLayer = mapLayers [layerY];
                Debug.Log ("GameField::Start(); -- mapLayer.opacity:" + mapLayer.opacity);
                for (int z = 0; z < sizeFieldZ; z++) {
                    for (int x = 0; x < sizeFieldX; x++) {
                        TileModel tileModel = mapLayer.tileModels [x, z];
                        if (tileModel != null) {
                            Vector3 graphicCoordinates = new Vector3 (x * sizeCellX + sizeCellX, layerY * sizeCellY, z * sizeCellZ + sizeCellZ); // все тут нужно понять
                            GameObject gameObject = (GameObject)Instantiate(tileModel.modelObject, graphicCoordinates, Quaternion.identity, this.transform); 
                            gameObject.AddComponent<Cell>();  ///УРЯ!
                            //Cell cell = new Cell (x, layerY, z, tileModel, graphicCoordinates);
                            Cell cell = gameObject.GetComponent<Cell>();
                            cell.setBasicValues(x, layerY, z, /*tileModel,*/ graphicCoordinates);
                            if(tileModel.properties.ContainsKey("terrain")) {
                                cell.setTerrain();
                            }
                            field[x, z] = cell;
    //                         MeshRenderer meshRenderer = gameObject.GetComponentInChildren<MeshRenderer> (); // Дикие не понятки со всем этим!
    //                         if (mapLayer.opacity == 0f) {
    //                             meshRenderer.enabled = false;
    //                         } else {
    //                             foreach (Material material in meshRenderer.materials) {
    // //                            Debug.Log("GameField::Start(); -- material:" + material);
    //                                 Color color = material.color;    
    //                                 /// Прозрачность
    //                                 color.a = mapLayer.opacity; // It is not WOKR!=(
    //                                 material.color = color;
    // //                            Debug.Log("GameField::Start(); -- material.color:" + material.color);
    //                             }
    //                         }
                            // gameObject.transform.SetParent (this.transform);
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
//        if (Input.GetMouseButtonDown(0)) {
//            print ("GameField::Update(); -- Input.GetMouseButtonDown(0);");
//            print ("GameField::Update(); -- Input.mousePosition" + Input.mousePosition);
//            Transform[] allTransforms = gameObject.GetComponentsInChildren<Transform>();
//            foreach(Transform childObjects in allTransforms){
//                if(gameObject.transform.IsChildOf(childObjects.transform) == false)
//                    Destroy(childObjects.gameObject);
//            }
//            GenerateField();
//        }
//        if (Input.GetButtonDown("Fire1")) {
//            print ("GameField::Update(); -- Input.GetButtonDown(Fire1);");
//            print ("GameField::Update(); -- Input.mousePosition" + Input.mousePosition);
//            Camera camera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
//            print ("GameField::Update(); -- camera:" + camera);
//            Vector3 worldPos = camera.ScreenToWorldPoint (Input.mousePosition);
//            print ("GameField::Update(); -- worldPos:" + worldPos);
//        }
    }
    
    public void towerActions(int x, int z) {
        Debug.Log("GameField::towerActions(); -- x:" + x + " z:" + z);
        if (field[x, z].isEmpty()) {
            createTower(x, z, factionsManager.getRandomTemplateForTowerFromAllFaction(), 1);
            // rerouteForAllCreeps();
        } else if (field[x, z].getTower() != null) {
            removeTower(x, z);
        }
    }

    public bool createTower(int buildX, int buildZ, TemplateForTower templateForTower, int player) {
        Debug.Log("GameField::createTower(); -- buildX:" + buildX + " buildZ:" + buildZ + " templateForTower:" + templateForTower + " player:" + player);
        if (gamerGold >= templateForTower.cost) {
            int towerSize = templateForTower.size;
            int startX = 0, startZ = 0, finishX = 0, finishZ = 0;
            if (towerSize != 1) {
                // Нижняя карта
                if (towerSize % 2 == 0) {
                    startX = -(towerSize / 2);
                    startZ = -(towerSize / 2);
                    finishX = (towerSize / 2)-1;
                    finishZ = (towerSize / 2)-1;
                } else {
                    startX = -(towerSize / 2);
                    startZ = -(towerSize / 2);
                    finishX = (towerSize / 2);
                    finishZ = (towerSize / 2);
                }
                // Правая карта
//                if (towerSize % 2 == 0) {
//                    startX = -(towerSize / 2);
//                    startZ = -((towerSize / 2) - 1);
//                    finishX = ((towerSize / 2) - 1);
//                    finishZ = (towerSize / 2);
//                } else {
//                    startX = -(towerSize / 2);
//                    startZ = -(towerSize / 2);
//                    finishX = (towerSize / 2);
//                    finishZ = (towerSize / 2);
//                }
            }
        Debug.Log("GameField::createTower(); -- test1");
            for (int tmpX = startX; tmpX <= finishX; tmpX++)
                for (int tmpZ = startZ; tmpZ <= finishZ; tmpZ++)
                    if (!cellIsEmpty(buildX + tmpX, buildZ + tmpZ))
                        return false;

        Debug.Log("GameField::createTower(); -- test2");
            // GOVNO CODE
            Vector2Int position = new Vector2Int(buildX, buildZ);
            Tower tower = towersManager.createTower(position, templateForTower, player);
        Debug.Log("GameField::createTower(); -- test3 tower:" + tower);
            // Debug.Log("GameField::createTower()", "-- templateForTower.towerAttackType:" + templateForTower.towerAttackType);
            // if (templateForTower.towerAttackType != TowerAttackType.Pit) {
                for (int tmpX = startX; tmpX <= finishX; tmpX++) {
                    for (int tmpZ = startZ; tmpZ <= finishZ; tmpZ++) {
                        field[buildX + tmpX, buildZ + tmpZ].setTower(tower);
                        Vector3 pos = field[buildX + tmpX, buildZ + tmpZ].graphicCoordinates;
                        pos.Set(pos.x-1.5f, pos.y, pos.z-1.5f);
                        // Vector3 scal = gameObject.transform.localScale;
                        // gameObject.transform.localScale.Set(scal.x*2f, scal.y*2f, scal.z*2f);
                        GameObject gameObject = (GameObject)Instantiate(tower.getTemplateForTower().modelObject, pos, Quaternion.identity, this.transform); 
                        // pathFinder.nodeMatrix[buildZ + tmpZ][buildX + tmpX].setKey('T');
                        tower.gameObject = gameObject;
                        Debug.Log("GameField::createTower(); -- Instantiate gameObject:" + gameObject);
                    }
                }
            // }
            // GOVNO CODE

//            rerouteForAllCreeps();
            gamerGold -= templateForTower.cost;
            Debug.Log("GameField::createTower(); -- Now gamerGold:" + gamerGold);
            return true;
        } else {
            return false;
        }
    }

    public void removeLastTower() {
//        if(towersManager.amountTowers() > 0) {
            Tower tower = towersManager.getTower(towersManager.amountTowers() - 1);
            Vector2Int pos = tower.getPosition();
            removeTower(pos.x, pos.y); // ?? SUKA UNITY WHY!?!??!  ||1||
//        }
    }

    public void removeTower(int touchX, int touchZ) {
        Tower tower = field[touchX, touchZ].getTower();
        if (tower != null) {
            int x = tower.getPosition().x;
            int z = tower.getPosition().y; // ?? SUKA UNITY WHY!?!??!  ||2||
            int towerSize = tower.getTemplateForTower().size;
            int startX = 0, startZ = 0, finishX = 0, finishZ = 0;
            if (towerSize != 1) {
                if (towerSize % 2 == 0) {
                    startX = -(towerSize / 2);
                    startZ = -(towerSize / 2);
                    finishX = (towerSize / 2)-1;
                    finishZ = (towerSize / 2)-1;
                } else {
                    startX = -(towerSize / 2);
                    startZ = -(towerSize / 2);
                    finishX = towerSize / 2;
                    finishZ = towerSize / 2;
                }
            }

            for (int tmpX = startX; tmpX <= finishX; tmpX++) {
                for (int tmpZ = startZ; tmpZ <= finishZ; tmpZ++) {
                    field[x + tmpX, z + tmpZ].removeTower();
                    // pathFinder.getNodeMatrix()[z + tmpZ][x + tmpX].setKey('.');
                    Destroy(tower.gameObject);
                }
            }
            towersManager.removeTower(tower);
            // rerouteForAllCreeps();
            gamerGold += tower.getTemplateForTower().cost;//*0.5;
        }
    }

    private bool cellIsEmpty(int x, int z) {
        if (x >= 0 && z >= 0) {
            if (x < sizeFieldX && z < sizeFieldZ) {
                return field[x, z].isEmpty();
            }
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameField : MonoBehaviour {

    public GameObject Creeps;
    public GameObject gamefield;

    public NavMeshSurface surface;
    public WaveManager waveManager; // ALL public for all || we are friendly :)
    public CreepsManager creepsManager; // For Shell
    public FactionsManager factionsManager;
    public TowersManager towersManager;
    public string mapPath = "maps/arena0";

    public int sizeFieldX, sizeFieldZ;
    public float sizeCellX=3f, sizeCellY=0.3f, sizeCellZ=3f; // need change, load from map
    public Cell[,] field;

    // GAME INTERFACE ZONE1
    // public WhichCell whichCell;
    public bool gamePaused;
    public float gameSpeed;
    public static int gamerGold = 1000000; // For Shell
    // public int maxOfMissedCreepsForComputer0;
    // public int missedCreepsForComputer0;
    // public int maxOfMissedCreepsForPlayer1;
    // public int missedCreepsForPlayer1;
    // GAME INTERFACE ZONE2
    // LineRenderer lineRenderer;

    // Use this for initialization
    void Start() {
        Debug.Log("GameField::Start(); -- Start!");

        waveManager = new WaveManager();
        creepsManager = new CreepsManager();
        towersManager = new TowersManager();
        factionsManager = new FactionsManager(1f);
        factionsManager.loadFactions();
        gamefield = GameObject.Find("GameField");
        Map map = new MapLoader().loadMap(mapPath);

        sizeFieldX = int.Parse(map.properties ["width"]);
        sizeFieldZ = int.Parse(map.properties ["height"]);

        // lineRenderer = gameObject.AddComponent<LineRenderer>();
        createField(sizeFieldX, sizeFieldZ, map.mapLayers);
        Creeps = new GameObject("Creeps");
        Creeps.transform.position = new Vector3(0,10,0);

        GameObject NavMesh = new GameObject("NavMesh");
        NavMesh.AddComponent<NavMeshSurface>();
     //   var geo = NavMesh.GetComponent<NavMeshSurface>();
      //  geo.overrideTileSize = true;
      //  geo.tileSize = 64;
        //geo.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
        surface = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();

        surface.BuildNavMesh();
        WaveAlgorithm waveAlgorithm = new WaveAlgorithm(sizeFieldX, sizeFieldZ, 30, 30, field);
        Debug.Log("GameField::Start(); -- End!");
    }

    private void createField(int sizeFieldX, int sizeFieldZ, Dictionary<int, MapLayer> mapLayers) {
    Debug.Log("GameField::createField(" + sizeFieldX + ", " + sizeFieldZ + ", " + mapLayers + "); -- field:" + field);
        if (field == null) {
            field = new Cell[sizeFieldX, sizeFieldZ];
            //foreach (MapLayer mapLayer in mapLayers.Values) {
            for (int layerY = 0; layerY < mapLayers.Count; layerY++) {
                MapLayer mapLayer = mapLayers [layerY];
                Debug.Log("GameField::Start(); -- mapLayer.opacity:" + mapLayer.opacity);
                for (int z = 0; z < sizeFieldZ; z++) {
                    for (int x = 0; x < sizeFieldX; x++) {
                        TileModel tileModel = mapLayer.tileModels[x, z];
                        if (tileModel != null) {
                            Vector3 graphicCoordinates = new Vector3 (x * sizeCellX + sizeCellX, layerY * sizeCellY, z * sizeCellZ + sizeCellZ); // все тут нужно понять
                            GameObject newCellGameObject = (GameObject)Instantiate(tileModel.modelObject, graphicCoordinates, Quaternion.identity, this.transform);
                            //Cell cell = new Cell (x, layerY, z, tileModel, graphicCoordinates);
                            Cell cell = newCellGameObject.AddComponent<Cell>();
                            cell.setBasicValues(x, layerY, z, /*tileModel,*/ graphicCoordinates);
                            // cell.setDebugLineRender(lineRenderer);
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

    // ___!0!___ Creeps Section ____!!!___
    public void setExitPoint(int x, int y) {
        Debug.Log("GameField::setExitPoint(); -- ");
        waveManager.setExitPoint(new Vector2Int(x, y));
        // rerouteForAllCreeps(new Vector2Int(x, y));
    }

    // ___!1!___ Creeps Section ____!!!___
    public void spawnCreepFromUser(TemplateForUnit templateForUnit) {
        Debug.Log("GameField::spawnCreepFromUser(); -- templateForUnit:" + templateForUnit);
        if (gamerGold >= templateForUnit.cost) {
            gamerGold -= templateForUnit.cost;
            foreach (Wave wave in waveManager.wavesForUser) {
                createCreep(wave.spawnPoint, templateForUnit, wave.exitPoint, 1); // create Player1 Creep
            }
        }
    }

    private void spawnCreeps(float delta) {
        List<WaveManager.TemplateNameAndPoints> allCreepsForSpawn = waveManager.getAllCreepsForSpawn(delta);
        foreach (WaveManager.TemplateNameAndPoints templateNameAndPoints in allCreepsForSpawn) {
            spawnCreep(templateNameAndPoints);
        }
    }

    private void spawnCreep(WaveManager.TemplateNameAndPoints templateNameAndPoints) {
        if (templateNameAndPoints != null) {
            TemplateForUnit templateForUnit = factionsManager.getTemplateForUnitByName(templateNameAndPoints.templateName);
            if (templateForUnit != null) {
                createCreep(templateNameAndPoints.spawnPoint, templateForUnit, templateNameAndPoints.exitPoint, 0); // create Computer0 Creep
            } else {
                Debug.Log("GameField::spawnCreep(); -- templateForUnit == null | templateName:" + templateNameAndPoints.templateName);
            }
        }
    }

    public void createCreep(int x, int z) {
        Debug.Log("GameField::createCreep(); -- x:" + x + " z:" + z);
        createCreep(new Vector2Int(x, z), factionsManager.getRandomTemplateForUnitFromAllFaction(), Vector2Int.zero, 0); // create computer0 Creep
        // rerouteForAllCreeps(); // BAD need Another! i don't know, need this OR not?
    }

    private void createCreep(Vector2Int spawnPoint, TemplateForUnit templateForUnit, Vector2Int exitPoint, int player) {
        Debug.Log("GameField::createCreep(" + spawnPoint + ", " + templateForUnit.toString() + ", " + exitPoint + ", " + player + "); -- ");
        if (exitPoint == Vector2Int.zero) {
            // exitPoint = waveManager.lastExitPoint;
            exitPoint = spawnPoint; // need change in future! TODO
        }
        if (spawnPoint != Vector2Int.zero && exitPoint != Vector2Int.zero) { // && pathFinder != null) {
//            pathFinder.loadCharMatrix(getCharMatrix());
            // List<Vector2Int> route = pathFinder.route(spawnPoint.x, spawnPoint.y, exitPoint.x, exitPoint.y);
            List<Vector2Int> route = new List<Vector2Int>();
            route.Add(new Vector2Int(spawnPoint.x, spawnPoint.y));
            route.Add(new Vector2Int(exitPoint.x, exitPoint.y));
            Debug.Log("GameField::createCreep(); -- route:" + route);
            if (route != null) {
                Vector3 pos = field[spawnPoint.x, spawnPoint.y].graphicCoordinates;
                Debug.Log("GameField::createCreep(); -- templateForUnit.modelObject:" + templateForUnit.modelGameObject + " templateForUnit:" + templateForUnit.toString());
                GameObject gameObject = (GameObject)Instantiate(templateForUnit.modelGameObject, pos, Quaternion.identity, Creeps.transform);
                gameObject.name = templateForUnit.toString(); // mb comment!
             NavMeshAgent agent = gameObject.AddComponent<NavMeshAgent>();
                Creep creep = gameObject.AddComponent<Creep>();
                creepsManager.addCreep(creep, agent, route, templateForUnit, player);
                // Creep creep = creepsManager.createCreep(route, templateForUnit, player);
                field[spawnPoint.x, spawnPoint.y].setCreep(creep); // TODO field maybe out array | NO, we have WaveManager.validationPoints()
                pos.Set(pos.x-1.5f, pos.y+0.5f, pos.z-1.5f);
                creep.setGameObjectAndAnimation(gameObject);   
                //agent.SetDestination(new Vector3(96,0,96));
                Debug.Log("GameField::createCreep(); -- Instantiate gameObject:" + gameObject + " 4_ThisCrep:" + creep);
            } else {
                Debug.Log("GameField::createCreep(); -- Not found route for createCreep!");
                if(towersManager.amountTowers() > 0) {
                    Debug.Log("GameField::createCreep(); -- Remove one last tower! And retry call createCreep()");
                    removeLastTower();
                    createCreep(spawnPoint, templateForUnit, exitPoint, player);
                }
            }
        } else {
            Debug.Log("GameField::createCreep(); -- Bad spawnPoint:" + spawnPoint + " || exitPoint:" + exitPoint); //  + " || pathFinder:" + pathFinder);
        }
    }
    // ___!2!___ Creeps Section ____!!!___

    // ___!1!___ Towers Section ____!!!___
//     public void buildTowersWithUnderConstruction(int x, int y) {
//         if (underConstruction != null) {
//             underConstruction.setEndCoors(x, y);
//             createTower(underConstruction.startX, underConstruction.startY, underConstruction.templateForTower, 1);
//             for (int k = 0; k < underConstruction.coorsX.size; k++) {
// //            for(int k = underConstruction.coorsX.size-1; k >= 0; k--) {
//                 createTower(underConstruction.coorsX.get(k), underConstruction.coorsY.get(k), underConstruction.templateForTower, 1);
//             }
//             underConstruction.clearStartCoors();
//             rerouteForAllCreeps();
//         }
//     }

    public void towerActions(int x, int z) {
        Debug.Log("GameField::towerActions(); -- x:" + x + " z:" + z);
        if (field[x, z].isEmpty()) {
            createTower(x, z, factionsManager.getRandomTemplateForTowerFromAllFaction(), 1);
            // rerouteForAllCreeps();
        } else if (field[x, z].getTower() != null) {
            removeTower(x, z);
            // rerouteForAllCreeps();
        }
        rerouteForAllCreeps(); // bad idea MB???
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
        // Debug.Log("GameField::createTower(); -- test1");
            for (int tmpX = startX; tmpX <= finishX; tmpX++)
                for (int tmpZ = startZ; tmpZ <= finishZ; tmpZ++)
                    if (!cellIsEmpty(buildX + tmpX, buildZ + tmpZ))
                        return false;

        // Debug.Log("GameField::createTower(); -- test2");
            // GOVNO CODE
            Vector2Int position = new Vector2Int(buildX, buildZ);
            Tower tower = towersManager.createTower(position, templateForTower, player);
        // Debug.Log("GameField::createTower(); -- test3 tower:" + tower);
            // Debug.Log("GameField::createTower()", "-- templateForTower.towerAttackType:" + templateForTower.towerAttackType);
            // if (templateForTower.towerAttackType != TowerAttackType.Pit) {
                for (int tmpX = startX; tmpX <= finishX; tmpX++) {
                    for (int tmpZ = startZ; tmpZ <= finishZ; tmpZ++) {
                        Cell cell = field[buildX + tmpX, buildZ + tmpZ];
                        cell.setTower(tower);
                        Vector3 pos = field[buildX + tmpX, buildZ + tmpZ].graphicCoordinates;
                        pos.Set(pos.x-1.5f, pos.y, pos.z-1.5f);
                        // Vector3 scal = gameObject.transform.localScale;
                        // gameObject.transform.localScale.Set(scal.x*2f, scal.y*2f, scal.z*2f);
                        GameObject gameObject = (GameObject)Instantiate(tower.getTemplateForTower().modelObject, pos, Quaternion.identity, cell.transform);
                        gameObject.transform.localScale = new Vector3(3.0f,3.0f,3.0f);
                        // pathFinder.nodeMatrix[buildZ + tmpZ][buildX + tmpX].setKey('T');
                        tower.gameObject = gameObject;
                        // surface.BuildNavMesh();
                        foreach(var agents in  Creeps.GetComponentsInChildren<NavMeshAgent>()) {
                            agents.SetDestination(new Vector3(96,0,96));
                        }
                        Debug.Log("GameField::createTower(); -- Instantiate gameObject:" + gameObject);
                    }
                }
            // }
            // GOVNO CODE

            // rerouteForAllCreeps();
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
                    Debug.Log("GameField::removeTower(); -Destroy- tower.gameObject:" + tower.gameObject);
                    Destroy(tower.gameObject);
                }
            }
            towersManager.removeTower(tower);
            // rerouteForAllCreeps();
            gamerGold += tower.getTemplateForTower().cost;//*0.5;
        }
    }
    // ___!2!___ Towers Section ____!!!___

    private void rerouteForAllCreeps() {
        Debug.Log("GameField::rerouteForAllCreeps(); -- ");
        // if (surface != null) {
            surface.BuildNavMesh();
        // }
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

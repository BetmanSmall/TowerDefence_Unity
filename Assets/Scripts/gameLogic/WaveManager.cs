using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager {

    public class TemplateNameAndPoints {
        public string templateName;
        public Vector2Int spawnPoint;
        public Vector2Int exitPoint;

        public TemplateNameAndPoints(string templateName, Vector2Int spawnPoint, Vector2Int exitPoint) {
            this.templateName = templateName;
            this.spawnPoint = spawnPoint;
            this.exitPoint = exitPoint;
        }
    }

    public List<Wave> waves;
    public Vector2Int lastExitPoint;
    public List<Wave> wavesForUser;
    public float waitForNextSpawnCreep;

    public WaveManager() {
        this.waves = new List<Wave>();
        this.wavesForUser = new List<Wave>();
//        this.waitForNextSpawnCreep =
    }

    public void addWave(Wave wave) {
        this.waves.Add(wave);
    }

    public List<TemplateNameAndPoints> getAllCreepsForSpawn(float delta) {
        waitForNextSpawnCreep -= delta;
        List<TemplateNameAndPoints> allCreepsForSpawn = new List<TemplateNameAndPoints>();
        foreach (Wave wave in waves) {
            if(wave.actions.Count != 0) {
                string templateName = wave.getTemplateNameForSpawn(delta);
                if (templateName != null) {
                    if (templateName.Contains("wait")) {
                        waitForNextSpawnCreep = float.Parse(templateName.Substring(templateName.IndexOf("=") + 1, templateName.Length));// GOVNE GODE parseFloat3
                        // bitch naxyui =( || but work mb =)
                    } else {
                        allCreepsForSpawn.Add(new TemplateNameAndPoints(templateName, wave.spawnPoint, wave.exitPoint));
                    }
                }
            } else {
                waves.Remove(wave);
            }
        }
        return allCreepsForSpawn;
    }

    public List<Vector2Int> getAllSpawnPoint() {
        List<Vector2Int> points = new List<Vector2Int>();
        foreach (Wave wave in waves) {
            points.Add(wave.spawnPoint);
        }
        foreach (Wave wave in wavesForUser) {
            points.Add(wave.spawnPoint);
        }
        return points;
    }

    public List<Vector2Int> getAllExitPoint() {
        List<Vector2Int> points = new List<Vector2Int>();
        foreach (Wave wave in waves) {
            points.Add(wave.exitPoint);
        }
        foreach (Wave wave in wavesForUser) {
            points.Add(wave.exitPoint);
        }
        if (lastExitPoint != Vector2Int.zero) {
            points.Add(lastExitPoint);
        }
        return points;
    }

    public bool setExitPoint(Vector2Int exitPoint) {
        this.lastExitPoint = exitPoint;
        if (waves.Count != 0) {
            waves[0].exitPoint = exitPoint;
            return true;
        }
        return false;
    }

    public int getNumberOfActions() {
        int actions = 0;
        foreach (Wave wave in waves) {
            actions += wave.actions.Count;
        }
        return actions;
    }

//    public int getNumberOfCreeps() // need implement

    // public void validationPoints(Cell[][] field) {
    //     Gdx.app.log("WaveManager::validationPoints(" + field + ")", "--");
    //     if(field != null) {
    //         int sizeFieldX = field.length;
    //         int sizeFieldY = field[0].length;
    //         int wavesSize = waves.size;
    //         Gdx.app.log("WaveManager::validationPoints()", "-- sizeField:(" + sizeFieldX + ", " + sizeFieldY + ") waves:(" + wavesSize + ":" + waves.size + ")");
    //         for (int w = 0; w < waves.size; w++) {
    //             Wave wave = waves.get(w);
    //             GridPoint2 spawnPoint = wave.spawnPoint;
    //             GridPoint2 exitPoint = wave.exitPoint;
    //             Gdx.app.log("WaveManager::validationPoints()", "-- spawnPoint:" + spawnPoint + " exitPoint:" + exitPoint + " wave:" + wave);
    //             if (spawnPoint == null || spawnPoint.x < 0 || spawnPoint.x >= sizeFieldX || spawnPoint.y < 0 || spawnPoint.y >= sizeFieldY || !field[spawnPoint.x][spawnPoint.y].isPassable()) {
    //                 Gdx.app.log("WaveManager::validationPoints()", "-- SpawnPoint bad:" + spawnPoint + " wave:" + wave);
    //                 waves.removeValue(wave, true);
    //                 w--;
    //             } else if (exitPoint == null || exitPoint.x < 0 || exitPoint.x >= sizeFieldX || exitPoint.y < 0 || exitPoint.y >= sizeFieldY || !field[exitPoint.x][exitPoint.y].isPassable()) {
    //                 Gdx.app.log("WaveManager::validationPoints()", "-- ExitPoint bad:" + exitPoint + " wave:" + wave);
    //                 waves.removeValue(wave, true);
    //                 w--;
    //             }
    //         }
    //         Gdx.app.log("WaveManager::validationPoints()", "-- sizeField:(" + sizeFieldX + ", " + sizeFieldY + ") waves:(" + wavesSize + ":" + waves.size + ")");
    //         int wavesForUserSize = waves.size;
    //         Gdx.app.log("WaveManager::validationPoints()", "-- sizeField:(" + sizeFieldX + ", " + sizeFieldY + ") wavesForUser:(" + wavesForUserSize + ":" + wavesForUser.size + ")");
    //         for (int w = 0; w < wavesForUser.size; w++) {
    //             Wave wave = wavesForUser.get(w);
    //             GridPoint2 spawnPoint = wave.spawnPoint;
    //             GridPoint2 exitPoint = wave.exitPoint;
    //             Gdx.app.log("WaveManager::validationPoints()", "-- spawnPoint:" + spawnPoint + " exitPoint:" + exitPoint + " wave:" + wave);
    //             if (spawnPoint == null || spawnPoint.x < 0 || spawnPoint.x >= sizeFieldX || spawnPoint.y < 0 || spawnPoint.y >= sizeFieldY || !field[spawnPoint.x][spawnPoint.y].isPassable()) {
    //                 Gdx.app.log("WaveManager::validationPoints()", "-- SpawnPoint bad:" + spawnPoint + " wave:" + wave);
    //                 wavesForUser.removeValue(wave, true);
    //                 w--;
    //             } else if (exitPoint == null || exitPoint.x < 0 || exitPoint.x >= sizeFieldX || exitPoint.y < 0 || exitPoint.y >= sizeFieldY || !field[exitPoint.x][exitPoint.y].isPassable()) {
    //                 Gdx.app.log("WaveManager::validationPoints()", "-- ExitPoint bad:" + exitPoint + " wave:" + wave);
    //                 wavesForUser.removeValue(wave, true);
    //                 w--;
    //             }
    //         }
    //         Gdx.app.log("WaveManager::validationPoints()", "-- sizeField:(" + sizeFieldX + ", " + sizeFieldY + ") wavesForUser:(" + wavesForUserSize + ":" + wavesForUser.size + ")");
    //     }
    // }

    // public void checkRoutes(PathFinder pathFinder) {
    //     Gdx.app.log("WaveManager::checkRoutes(" + pathFinder + ")", "--");
    //     if(pathFinder != null) {
    //         int wavesSize = waves.size;
    //         Gdx.app.log("WaveManager::checkRoutes()", "-- waves:(" + wavesSize + ":" + waves.size + ")");
    //         for (int w = 0; w < waves.size; w++) {
    //             Wave wave = waves.get(w);
    //             GridPoint2 spawnPoint = wave.spawnPoint;
    //             GridPoint2 exitPoint = wave.exitPoint;
    //             Gdx.app.log("WaveManager::checkRoutes()", "-- spawnPoint:" + spawnPoint + " exitPoint:" + exitPoint);
    //             List<Node> route = pathFinder.route(spawnPoint.x, spawnPoint.y, exitPoint.x, exitPoint.y);
    //             if (route == null) {
    //                 Gdx.app.log("WaveManager::checkRoutes()", "-- Not found route for this points | Remove wave:" + wave);
    //                 waves.removeValue(wave, true);
    //                 w--;
    //             } else {
    //                 wave.route = route;
    //             }
    //         }
    //         Gdx.app.log("WaveManager::checkRoutes()", "-- waves:(" + wavesSize + ":" + waves.size + ")");
    //         int wavesForUserSize = wavesForUser.size;
    //         Gdx.app.log("WaveManager::checkRoutes()", "-- wavesForUser:(" + wavesForUserSize + ":" + waves.size + ")");
    //         for (int w = 0; w < wavesForUser.size; w++) {
    //             Wave wave = wavesForUser.get(w);
    //             GridPoint2 spawnPoint = wave.spawnPoint;
    //             GridPoint2 exitPoint = wave.exitPoint;
    //             Gdx.app.log("WaveManager::checkRoutes()", "-- spawnPoint:" + spawnPoint + " exitPoint:" + exitPoint);
    //             List<Node> route = pathFinder.route(spawnPoint.x, spawnPoint.y, exitPoint.x, exitPoint.y);
    //             if (route == null) {
    //                 Gdx.app.log("WaveManager::checkRoutes()", "-- Not found route for this points | Remove wave:" + wave);
    //                 wavesForUser.removeValue(wave, true);
    //                 w--;
    //             } else {
    //                 wave.route = route;
    //             }
    //         }
    //         Gdx.app.log("WaveManager::checkRoutes()", "-- wavesForUser:(" + wavesForUserSize + ":" + waves.size + ")");
    //     } else {
    //         Gdx.app.log("WaveManager::checkRoutes()", "-- pathFinder == null");
    //     }
    // }

    // Need fix bag with this
//    public void turnRight() {
//        if(sizeFieldX == sizeFieldY) {
//            Cell[][] newCells = new Cell[sizeFieldX][sizeFieldY];
//            for(int y = 0; y < sizeFieldY; y++) {
//                for(int x = 0; x < sizeFieldX; x++) {
//                    newCells[sizeFieldX-y-1][x] = field[x][y];
//                    newCells[sizeFieldX-y-1][x].setGraphicCoordinates(sizeFieldX-y-1, x, halfSizeCellX, halfSizeCellY);
//                }
//            }
//            field = newCells;
//        } else {
//            Gdx.app.log("GameField::turnRight()", "-- Not work || Work, but mb not Good!");
//            int oldWidth = sizeFieldX;
//            int oldHeight = sizeFieldY;
//            sizeFieldX = sizeFieldY;
//            sizeFieldY = oldWidth;
//            Cell[][] newCells = new Cell[sizeFieldX][sizeFieldY];
//            for(int y = 0; y < oldHeight; y++) {
//                for(int x = 0; x < oldWidth; x++) {
//                    newCells[sizeFieldX-y-1][x] = field[x][y];
//                    newCells[sizeFieldX-y-1][x].setGraphicCoordinates(sizeFieldX-y-1, x, halfSizeCellX, halfSizeCellY);
//                }
//            }
//            field = newCells;
//        }
//    }
//
//    public void turnLeft() {
//        if(sizeFieldX == sizeFieldY) {
//            Cell[][] newCells = new Cell[sizeFieldX][sizeFieldY];
//            for(int y = 0; y < sizeFieldY; y++) {
//                for(int x = 0; x < sizeFieldX; x++) {
//                    newCells[y][sizeFieldY-x-1] = field[x][y];
//                    newCells[y][sizeFieldY-x-1].setGraphicCoordinates(y, sizeFieldY-x-1, halfSizeCellX, halfSizeCellY);
//                }
//            }
//            field = newCells;
//        } else {
//            Gdx.app.log("GameField::turnLeft()", "-- Not work || Work, but mb not Good!");
//            int oldWidth = sizeFieldX;
//            int oldHeight = sizeFieldY;
//            sizeFieldX = sizeFieldY;
//            sizeFieldY = oldWidth;
//            Cell[][] newCells = new Cell[sizeFieldX][sizeFieldY];
//            for(int y = 0; y < oldHeight; y++) {
//                for(int x = 0; x < oldWidth; x++) {
//                    newCells[y][sizeFieldY-x-1] = field[x][y];
//                    newCells[y][sizeFieldY-x-1].setGraphicCoordinates(y, sizeFieldY-x-1, halfSizeCellX, halfSizeCellY);
//                }
//            }
//            field = newCells;
//        }
//    }
//
//    public void flipX() {
//        Cell[][] newCells = new Cell[sizeFieldX][sizeFieldY];
//        for (int y = 0; y < sizeFieldY; y++) {
//            for (int x = 0; x < sizeFieldX; x++) {
//                newCells[sizeFieldX-x-1][y] = field[x][y];
//                newCells[sizeFieldX-x-1][y].setGraphicCoordinates(sizeFieldX-x-1, y, halfSizeCellX, halfSizeCellY);
//            }
//        }
//        field = newCells;
//    }
//
//    public void flipY() {
//        Cell[][] newCells = new Cell[sizeFieldX][sizeFieldY];
//        for(int y = 0; y < sizeFieldY; y++) {
//            for(int x = 0; x < sizeFieldX; x++) {
//                newCells[x][sizeFieldY-y-1] = field[x][y];
//                newCells[x][sizeFieldY-y-1].setGraphicCoordinates(x, sizeFieldY-y-1, halfSizeCellX, halfSizeCellY);
//            }
//        }
//        field = newCells;
//    }

// OLD TOWER MANAGER CODE
    // private List<Tower> towers;

    // public WaveManager() {
    //     towers = new List<Tower>();
    // }

    // public Tower createTower(Vector2Int position, TemplateForTower templateForTower, int player) {
    //     Tower tower = new Tower(position, templateForTower, player);
    //     towers.Add(tower);
    //     return tower;
    // }

    // public void removeTower(Tower tower) {
    //     towers.Remove(tower);
    // }

    // public void removeTower(Vector2 position) {
    //     towers.Remove(getTower(position));
    // }

    // public Tower getTower(int id) {
    //     if(id < towers.Count) {
    //         return towers[id];
    //     } else {
    //         return null;
    //     }
    // }

    // public Tower getTower(Vector2 position) {
    //     for(int i=0; i < towers.Count; i++) {
    //         Vector2 towerPosition = towers[i].getPosition();
    //         if(towerPosition.Equals(position)) {
    //             return towers[i];
    //         }
    //     }
    //     return null;
    // }

    // public List<Tower> getAllTowers() {
    //     return towers;
    // }

    // public int amountTowers() {
    //     return towers.Count;
    // }
// OLD TOWER MANAGER CODE
}

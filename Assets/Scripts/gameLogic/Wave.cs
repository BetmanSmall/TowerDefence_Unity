using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave {
    public List<string> actions;
    public Vector2Int spawnPoint;
    public Vector2Int exitPoint;
    public List<Vector2Int> route;
    private float intervalForSpawn;
    private float elapsedTime;

    public Wave(Vector2Int spawnPoint, Vector2Int exitPoint, float startToMove) {
        this.actions = new List<string>();
        this.spawnPoint = spawnPoint;
        this.exitPoint = exitPoint;
        this.intervalForSpawn = startToMove;
        this.elapsedTime = 0f;
    }

    public string getTemplateNameForSpawn(float delta) {
        elapsedTime += delta;
        if (elapsedTime >= intervalForSpawn) {
            elapsedTime = 0f;
            string action = actions[0];
            actions.RemoveAt(0);
            if (action == null) {
                return null;
            } else if (action.Contains("delay")) {
                intervalForSpawn = float.Parse(action.Substring(action.IndexOf("=") + 1, action.Length)); // GOVNE GODE parseFloat1
//                Gdx.app.log("Wave::getNextNameTemplateForUnitForSpawnCreep()", "-- Delay after wave:" + intervalForSpawn + " sec.");
                return "wait=" + intervalForSpawn;
            } else if (action.Contains("interval")) {
                intervalForSpawn = float.Parse(action.Substring(action.IndexOf("=") + 1, action.Length)); // GOVNE GODE parseFloat2
//                Gdx.app.log("Wave::getNextNameTemplateForUnitForSpawnCreep()", "-- Next creep spawn after:" + intervalForSpawn + " sec.");
                return "wait=" + intervalForSpawn;
            } else { // string contain templateName.
                intervalForSpawn = 0f;
                return action;
            }
        }
        return "wait=" + (intervalForSpawn-elapsedTime);
    }

    public void addAction(string action) {
        this.actions.Add(action);
    }

    // public string toString() {
    //     StringBuilder sb = new StringBuilder();
    //     sb.append("Wave[");
    //     sb.append("spawnPoint:" + spawnPoint);
    //     sb.append("," + "exitPoint:" + exitPoint);
    //     sb.append("," + "elapsedTime:" + elapsedTime);
    //     sb.append("," + "intervalForSpawn:" + intervalForSpawn);
    //     sb.append("," + "actions:" + actions);
    //     sb.append("," + "route:" + route);
    //     sb.append("]");
    //     return sb.toString();
    // }
}

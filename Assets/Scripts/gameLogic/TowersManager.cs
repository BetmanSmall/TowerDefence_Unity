using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersManager {
    private List<Tower> towers;

    public TowersManager() {
        towers = new List<Tower>();
    }

    public Tower createTower(Vector2Int position, TemplateForTower templateForTower, int player) {
        Tower tower = new Tower(position, templateForTower, player);
        towers.Add(tower);
        return tower;
    }

    public void removeTower(Tower tower) {
        towers.Remove(tower);
    }

    public void removeTower(Vector2 position) {
        towers.Remove(getTower(position));
    }

    public Tower getTower(int id) {
        if(id < towers.Count) {
            return towers[id];
        } else {
            return null;
        }
    }

    public Tower getTower(Vector2 position) {
        for(int i=0; i < towers.Count; i++) {
            Vector2 towerPosition = towers[i].getPosition();
            if(towerPosition.Equals(position)) {
                return towers[i];
            }
        }
        return null;
    }

    public List<Tower> getAllTowers() {
        return towers;
    }

    public int amountTowers() {
        return towers.Count;
    }
}

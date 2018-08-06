using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreepsManager {
    private List<Creep> creeps;

    public CreepsManager() {
        creeps = new List<Creep>();
    }
    
    public Creep addCreep(Creep creep, NavMeshAgent agent, List<Vector2Int> route, TemplateForUnit templateForUnit, int player) {
        Debug.Log("CreepsManager::addCreep(" + creep + ", " + route + ", " + templateForUnit + ", " + player + "); -- ");
        creep.setCreepInfo(agent, route, templateForUnit, player);
        creeps.Add(creep);
        return creep;
    }

    // @Depricate
    // public Creep createCreep(List<Vector2Int> route, TemplateForUnit templateForUnit, int player) {
        // Debug.Log("CreepsManager::createCreep(" + route + ", " + templateForUnit + ", " + player + "); -- ");
        // Creep newCreep = new Creep(route, templateForUnit, player);
        // creeps.Add(newCreep);
        // return newCreep;
    // }

    public List<Creep> getAllCreeps() {
        return creeps;
    }

    public int amountCreeps() {
        return creeps.Count;
    }
    public int getCreep(Creep creep) {
        return creeps.IndexOf(creep);
    }

    public Creep getCreep(int id) {
        return creeps[id];
    }

    public Creep getCreep(Vector2Int position) {
        for (int i = 0; i < creeps.Count; i++) {
            Vector2Int creepPosition = creeps[i].getNewPosition();
            if (creepPosition.Equals(position)) {
                return creeps[i];
            }
        }
        return null;
    }

    public void removeCreep(Creep creep) {
        creeps.Remove(creep);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction {
    private string name;

    // private Array<TemplateForUnit> templateForUnits;
    private List<TemplateForTower> templateForTowers;

    public Faction(string name) {
        this.name = name;
        // this.templateForUnits = new Array<TemplateForUnit>();
        this.templateForTowers = new List<TemplateForTower>();
    }

    public string getName() {
        return name;
    }

    // public Array<TemplateForUnit> getTemplateForUnits() {
    //     return templateForUnits;
    // }

    public List<TemplateForTower> getTemplateForTowers() {
        return templateForTowers;
    }
}

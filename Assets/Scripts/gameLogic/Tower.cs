using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower {
    private Vector2 position;
    private float elapsedReloadTime;
    private TemplateForTower templateForTower;

    public int player; // In Future need change to enumPlayers {Computer0, Player1, Player2} and etc
    // public int capacity;
    // public Array<Shell> shells;
    // public Circle radiusDetectionСircle;
    // public Circle radiusFlyShellСircle;

    public Tower(Vector2 position, TemplateForTower templateForTower, int player){
        Debug.Log("Tower::Tower(" + position + ", " + templateForTower + "); -- ");
        this.position = position;
        this.elapsedReloadTime = templateForTower.reloadTime;
        this.templateForTower = templateForTower;

        this.player = player;
        // this.capacity = (templateForTower.capacity != null) ? templateForTower.capacity : 0;
        // this.shells = new Array<Shell>();
        // this.radiusDetectionСircle = new Circle(getCenterGraphicCoord(1), (templateForTower.radiusDetection == null) ? 0f : templateForTower.radiusDetection); // AlexGor
        // if(templateForTower.shellAttackType == ShellAttackType.FirstTarget && templateForTower.radiusFlyShell != null && templateForTower.radiusFlyShell >= templateForTower.radiusDetection) {
            // this.radiusFlyShellСircle = new Circle(getCenterGraphicCoord(1), templateForTower.radiusFlyShell);
        // }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}

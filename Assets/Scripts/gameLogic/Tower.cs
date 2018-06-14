using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower {
    private Vector2Int position;
    // private float elapsedReloadTime;
    private TemplateForTower templateForTower;

    public int player; // In Future need change to enumPlayers {Computer0, Player1, Player2} and etc
    // public int capacity;
    // public Array<Shell> shells;
    // public Circle radiusDetectionСircle;
    // public Circle radiusFlyShellСircle;

    public GameObject gameObject;

    public Tower(Vector2Int position, TemplateForTower templateForTower, int player){
        Debug.Log("Tower::Tower(" + position + ", " + templateForTower + "); -- ");
        this.position = position;
        // this.elapsedReloadTime = templateForTower.reloadTime;
        this.templateForTower = templateForTower;

        this.player = player;
        // this.capacity = (templateForTower.capacity != null) ? templateForTower.capacity : 0;
        // this.shells = new Array<Shell>();
        // this.radiusDetectionСircle = new Circle(getCenterGraphicCoord(1), (templateForTower.radiusDetection == null) ? 0f : templateForTower.radiusDetection); // AlexGor
        // if(templateForTower.shellAttackType == ShellAttackType.FirstTarget && templateForTower.radiusFlyShell != null && templateForTower.radiusFlyShell >= templateForTower.radiusDetection) {
            // this.radiusFlyShellСircle = new Circle(getCenterGraphicCoord(1), templateForTower.radiusFlyShell);
        // }
    }
    
    // // Update is called once per frame
    // void Update () {
        
    // }
    

    public bool recharge(float delta) {
        // elapsedReloadTime += delta;
        // if(elapsedReloadTime >= templateForTower.reloadTime) {
        //     return true;
        // }
        return false;
    }

    // public bool shoot(Creep creep) {
    //     if(elapsedReloadTime >= templateForTower.reloadTime) {
    //         if (templateForTower.shellAttackType == ShellAttackType.MassAddEffect) {
    //             bool effect = false;
    //             for (ShellEffectType shellEffectType : creep.shellEffectTypes) {
    //                 if (shellEffectType.shellEffectEnum == ShellEffectType.ShellEffectEnum.FreezeEffect) {
    //                     effect = true;
    //                     break;
    //                 }
    //             }
    //             if (!effect) {
    //                 creep.shellEffectTypes.add(new ShellEffectType(templateForTower.shellEffectType));
    //             }
    //         } else {
    //             shells.add(new Shell(templateForTower, creep, getCenterGraphicCoord())); // AlexGor
    //         }
    //         elapsedReloadTime = 0f;
    //         return true;
    //     }
    //     return false;
    // }

    // public void moveAllShells(float delta) {
    //     for(Shell shell : shells) {
    //         if(radiusFlyShellСircle == null) {
    //             moveShell(delta, shell);
    //         } else if(Intersector.overlaps(shell.circle, radiusFlyShellСircle)) {
    //             moveShell(delta, shell);
    //         } else {
    //             shell.dispose();
    //             shells.removeValue(shell, false);
    //         }
    //     }
    // }

//     private void moveShell(float delta, Shell shell) {
//         switch (shell.flightOfShell(delta)) {
//             case 0:
// //                if(shell.creep.die(damage)) {
// //                    GameField.gamerGold += shell.creep.getTemplateForUnit().bounty;
// //                }
// //                break;
//             case -1:
//                 shell.dispose();
//                 shells.removeValue(shell, false);
//         }
//     }

//     public Vector2 getCenterGraphicCoord() {
//         return getCenterGraphicCoord(GameField.isDrawableTowers);
//     }

    public Vector2Int getPosition() {
        return position;
    }

    // public Circle getRadiusDetectionСircle() {
    //     return radiusDetectionСircle;
    // } //AlexGor

//    public void setDamage(int damage) {
//        this.damage = damage;
//    }
    public int getDamage() {
        return templateForTower.damage;
    }

//    public void setRadiusDetection(int radiusDetection) {
//        this.radiusDetection = radiusDetection;
//    }
    // public int getRadiusDetection() {
    //     return templateForTower.radiusDetection;
    // }

//    public void setReloadTime(float reloadTime) {
//        this.reloadTime = reloadTime;
//    }
//    public float getReloadTime() {
//        return reloadTime;
//    }

    public TemplateForTower getTemplateForTower() {
        return templateForTower;
    }

    // public TextureRegion getCurentFrame() {
    //     return templateForTower.idleTile.getTextureRegion();
    // }
}

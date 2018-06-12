using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Creep {
    private List<Vector2Int> route;
    private Vector2Int oldPosition;
    private Vector2Int newPosition;
    private float hp;
    private float speed;
    private float stepsInTime;
    private float deathElapsedTime;

    public int player; // In Future need change to enumPlayers {Computer0, Player1, Player2} and etc
    // public Vector3 currentPoint;
    // public Vector3 backStepPoint;// AlexGor
    // public Circle circle1;
    // public Circle circle2;
    // public Circle circle3;
    // public Circle circle4;
    public Vector2 velocity;
    public Vector2 displacement;
    public TemplateForUnit templateForUnit;

    // public Direction direction;
    // private Animation animation;
    // public Array<ShellEffectType> shellEffectTypes;
    public GameObject gameObject;
    // public CreepAnimatorClipInfo сreepAnimatorClipInfo;

    public Creep(List<Vector2Int> route, TemplateForUnit templateForUnit, int player) {
        if(route != null) {
            this.route = route;
            this.oldPosition = route[0];
            route.RemoveAt(0);
            this.newPosition = oldPosition;
            this.hp = templateForUnit.healthPoints;
            this.speed = templateForUnit.speed;
            this.stepsInTime = 0;//templateForUnit.speed; // need respawn animation
            this.deathElapsedTime = 0;

            this.player = player;
            // this.currentPoint = new Vector3(newPosition.getX(), newPosition.getY());
            // this.backStepPoint = new Vector3(oldPosition.getX(), oldPosition.getY());
            // this.circle1 = new Circle();
            // this.circle2 = new Circle();
            // this.circle3 = new Circle();
            // this.circle4 = new Circle();

            this.templateForUnit = templateForUnit;

            // this.direction = Direction.UP;
            // setAnimation("walk_");
            // this.shellEffectTypes = new Array<ShellEffectType>();
        } else {
            Debug.LogError("Creep::Creep(); -- route == null");
        }
    }

    public bool setGameObjectAndAnimation(GameObject gameObject) {//, RuntimeAnimatorController runtimeAnimatorController) {
        // Debug.Log("Creep::setGameObjectAndAnimation(); -- gameObject:" + gameObject + " runtimeAnimatorController:" + runtimeAnimatorController);
        if(this.gameObject == null) {
            this.gameObject = gameObject;
            // Animation animation = gameObject.AddComponent<Animation>();
            Animation animation = gameObject.GetComponent<Animation>();
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath("Assets/Resources/Creeps/skeletonsPack/Skeleton_footman.fbx");
            int size = objs.Length;
            int count = 0;
            string[] animationsNameTMP = new string[size];
            Debug.Log("Creep::setGameObjectAndAnimation(); -- size:" + size);
            foreach (Object o in objs) {
                if (o.GetType() == typeof(AnimationClip)) {
                    AnimationClip clip = o as AnimationClip;
                    animation.AddClip(clip, o.name.ToString());
                    Debug.Log("Creep::setGameObjectAndAnimation(); -- animationsName[" + count + "]:" + o.name.ToString());
                    animationsNameTMP[count] = o.name.ToString();
                    count++;
                }
            }
            Debug.Log("Creep::setGameObjectAndAnimation(); -- animationsNameTMP.Length:" + animationsNameTMP.Length);
            // animationsName.Length = count;
            Debug.Log("Creep::setGameObjectAndAnimation(); -- count:" + count);
            string[] animationsName = new string[count];
            // foreach (string str in animationsNameTMP) {
            for (int k = 0; k < animationsName.Length; k++) {
                string str = animationsNameTMP[k];
                animationsName[k] = str;
            }
            Debug.Log("Creep::setGameObjectAndAnimation(); -- animationsName.Length:" + animationsName.Length);

            // AnimationClip[] animations1 = AnimationUtility.GetAnimationClips(gameObject);
            // Debug.Log("Creep::setGameObjectAndAnimation(); -1- animations1:" + animations1 + " animations1.Length:" + animations1.Length);
            //Animator animator = gameObject.GetComponent<Animator>();
            // Debug.Log("Creep::setGameObjectAndAnimation(); -- animator:" + animator);
            // animator.runtimeAnimatorController = runtimeAnimatorController;
            // Debug.Log("Creep::setGameObjectAndAnimation(); -1- animator.runtimeAnimatorController:" + animator.runtimeAnimatorController);
            // AnimationClip[] animations2 = AnimationUtility.GetAnimationClips(gameObject);
            // Debug.Log("Creep::setGameObjectAndAnimation(); -2- animations2:" + animations2 + " animations2.Length:" + animations2.Length);
            // AnimationClip animationClip0 = templateForUnit.animationClips[0];
            // Debug.Log("Creep::setGameObjectAndAnimation(); -- animationClip0:" + animationClip0);
            // if(animationClip0) {
                // animator.runtimeAnimatorController = new RuntimeAnimatorController();//emplateForUnit.animationClips;
                // AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
                // Debug.Log("Creep::setGameObjectAndAnimation(); -1- aoc.clips.Length:" + aoc.clips.Length);
                // var newAnimsList = new List<KeyValuePair<AnimationClip, AnimationClip>>();
                // Debug.Log("Creep::setGameObjectAndAnimation(); -1- newAnimsList.Count):" + newAnimsList.Count);
                // // foreach (AnimationClip animationClip in aoc.animationClips) {
                // Debug.Log("Creep::setGameObjectAndAnimation(); -- templateForUnit.animationClips.Length:" + templateForUnit.animationClips.Length);
                // foreach (AnimationClip animationClip in templateForUnit.animationClips) {
                //     Debug.Log("Creep::setGameObjectAndAnimation(); -- animationClip:" + animationClip);
                //     newAnimsList.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip, animationClip));
                // }
                // Debug.Log("Creep::setGameObjectAndAnimation(); -2- newAnimsList.Count:" + newAnimsList.Count);
                // aoc.ApplyOverrides(newAnimsList);
                // aoc.GetOverrides(newAnimsList);
                // Debug.Log("Creep::setGameObjectAndAnimation(); -2.5- newAnimsList.Count:" + newAnimsList.Count);
                // Debug.Log("Creep::setGameObjectAndAnimation(); -2- aoc.clips.Length:" + aoc.clips.Length);
                // animator.runtimeAnimatorController = aoc;
                // Debug.Log("Creep::setGameObjectAndAnimation(); -2- animator.runtimeAnimatorController:" + animator.runtimeAnimatorController);
                // gameObject.SetComponent<Animator>(animator);
            // }

            // Animation animation = gameObject.GetComponent<Animation>();
            Debug.Log("Creep::setGameObjectAndAnimation(); -1- animation:" + animation);
            // if(animation == null) {
                // animation = gameObject.AddComponent<Animation>();
                // animator = gameObject.AddComponent<Animator>();
            // }
            // foreach (AnimationClip animationClip in templateForUnit.animationClips) {
                // animationClip.legacy = true;
                // animation.AddClip(animationClip, animationClip.name);
            // }
            // Debug.Log("Creep::setGameObjectAndAnimation(); -2- animation:" + animation);

            // this.сreepAnimatorClipInfo = new CreepAnimatorClipInfo(gameObject);
            // AnimationClip[] animations = AnimationUtility.GetAnimationClips(gameObject);
          
            // Debug.Log("Creep::setGameObjectAndAnimation(); -3- animations:" + animations + " animations.Length:" + animations.Length);
            // foreach (AnimationClip animationClip in animation) {
            //     Debug.Log("Creep::setGameObjectAndAnimation(); -- animationClip:" + animationClip);
            // }
            Debug.Log("Creep::setGameObjectAndAnimation(); -- animation.GetClipCount():" + animation.GetClipCount());
            Debug.Log("Creep::setGameObjectAndAnimation(); -- animationsName.Length:" + animationsName.Length);
            int randomInt = Random.Range(0, animationsName.Length);
            Debug.Log("Creep::setGameObjectAndAnimation(); -- randomInt:" + randomInt);
            string randomString = animationsName[randomInt];
            Debug.Log("Creep::setGameObjectAndAnimation(); -- randomString:" + randomString);
            AnimationClip animationClip = animation.GetClip(randomString);
            Debug.Log("Creep::setGameObjectAndAnimation(); -- animationClip:" + (animationClip == true) );
            Debug.Log("Creep::setGameObjectAndAnimation(); -- animationClip.ToString():" + animationClip.ToString());
            Debug.Log("Creep::setGameObjectAndAnimation(); -- animationClip.name:" + animationClip.name);
            Debug.Log("Creep::setGameObjectAndAnimation(); -- animationClip.name.ToString():" + animationClip.name.ToString());
            string playAnimCurr = animationClip.name.ToString();
            Debug.Log("Creep::setGameObjectAndAnimation(); -- playAnimCurr:" + playAnimCurr + " randomInt:" + randomInt);
            // animator.Play(playAnimCurr);
            animation.Play(playAnimCurr);
            return true;
        }
        return false;
    }

    public void dispose() {
        route = null;
        oldPosition = Vector2Int.zero; // ?? LOL
        newPosition = Vector2Int.zero; // WTF?
        templateForUnit = null;
        // direction = null;
        // animation = null;
    }
    
    // // Update is called once per frame
    // void Update () {
    // }

    // что бы ефекты не стакались на крипах
    public Vector2Int move(float delta) {
//        Gdx.app.log("Creep", "move(); -- Creep status:" + this.toString());
        if(route != null && route.Count != 0) {
//             for(ShellEffectType shellEffectType : shellEffectTypes) {
//                 if(!shellEffectType.used) {
// //                    Gdx.app.log("Creep", "move(); -- Active shellEffectType:" + shellEffectType);
//                     shellEffectType.used = true;
//                     if(shellEffectType.shellEffectEnum == ShellEffectType.ShellEffectEnum.FreezeEffect) {
//                         float smallSpeed = speed/100f;
//                         float percentSteps = stepsInTime/smallSpeed;
//                         speed += shellEffectType.speed;
//                         smallSpeed = speed/100f;
//                         stepsInTime = smallSpeed*percentSteps;
//                     } else if(shellEffectType.shellEffectEnum == ShellEffectType.ShellEffectEnum.FireEffect) {
//                         hp -= shellEffectType.damage;
// //                        if(die(shellEffectType.damage, null)) {
// //                            GameField.gamerGold += templateForUnit.bounty;
// //                        }
//                     }
//                 } else {
//                     if(shellEffectType.shellEffectEnum == ShellEffectType.ShellEffectEnum.FireEffect) {
//                         hp -= shellEffectType.damage;
// //                        if(die(shellEffectType.damage, null)) {
// //                            GameField.gamerGold += templateForUnit.bounty;
// //                        }
//                     }
//                 }
//                 shellEffectType.elapsedTime += delta;
//                 if(shellEffectType.elapsedTime >= shellEffectType.time) {
// //                    Gdx.app.log("Creep", "move(); -- Remove shellEffectType:" + shellEffectType);
//                     if(shellEffectType.shellEffectEnum == ShellEffectType.ShellEffectEnum.FreezeEffect) {
//                         float smallSpeed = speed/100f;
//                         float percentSteps = stepsInTime/smallSpeed;
//                         speed = speed-shellEffectType.speed;
//                         smallSpeed = speed/100f;
//                         stepsInTime = smallSpeed*percentSteps;
//                     }
//                     // shellEffectTypes.removeValue(shellEffectType, true);
//                 }
            // }
            stepsInTime += delta;
            if (stepsInTime >= speed) {
                stepsInTime = 0f;
                oldPosition = newPosition;
                newPosition = route[0];
                route.RemoveAt(0); // MB not Safety code! throws Exception!
            }
            /* || In JAVA work with ANIMATION  ||
            //  || .......................... || 
            //    || ................... .. ||   
            //      || .................. ||     
            //        || .............. ||       
            //          || .......... ||         
            //            || ...... ||           
            //              || ...||             
            //               ||__||            */
            // backStepPoint = currentPoint;
            // currentPoint.set(fVx, fVy);

            // velocity = new Vector2(backStepPoint.x - currentPoint.x,
            //         backStepPoint.y - currentPoint.y).nor().scl(Math.min(currentPoint.dst(backStepPoint.x,
            //         backStepPoint.y), speed));
            // displacement = new Vector2(velocity.x * delta, velocity.y * delta);

//            Gdx.app.log("Creep::move()", "-- direction:" + direction + " oldDirection:" + oldDirection);
            // if(!direction.equals(oldDirection)) {
            //     setAnimation("walk_");
            // }
            return newPosition;
        } else {
            dispose();
            return Vector2Int.zero;
        }
    }
    
    public bool die(float damage) { //, ShellEffectType shellEffectType) {
        if(hp > 0) {
            hp -= damage;
            // addEffect(shellEffectType);
            if(hp <= 0) {
                deathElapsedTime = 0;
                // setAnimation("death_");
                Debug.Log("Creep::die(" + damage + "); -- setAnimation.(death_)");
                return true;
            }
            return false;
        }
        return false;
    }

    // private bool addEffect(ShellEffectType shellEffectType) {
    //     if(shellEffectType != null){
    //         if(!shellEffectTypes.contains(shellEffectType, false)) {
    //             shellEffectTypes.add(new ShellEffectType(shellEffectType));
    //         }
    //     }
    //     return true;
    // }

    public bool changeDeathFrame(float delta) {
        if(hp <= 0) {
            if(deathElapsedTime >= speed) {
//                dispose();
                return false;
            } else {
                deathElapsedTime += delta;
            }
            return true;
        }
        return false;
    }

    public Vector2Int getOldPosition() {
        return oldPosition;
    }
    public Vector2Int getNewPosition() {
        return newPosition;
    }

    public void setHp(int hp) {
        this.hp = hp;
    }
    public int getHp() {
        return (int)hp;
    }
    public bool isAlive() {
        if(gameObject == null) { // TODO Не верно, нужно исправить. / in java was if(animation == null)
            return false;
        }
        return hp > 0 ? true : false;
    }

    public void setSpeed(float speed) {
        this.speed = speed;
    }
    public float getSpeed() {
        return speed;
    }

    public void setStepsInTime(float stepsInTime) {
        this.stepsInTime = stepsInTime;
    }
    public float getStepsInTime() {
        return stepsInTime;
    }

    public void setRoute(List<Vector2Int> route) {
        this.route = route;
    }
    public List<Vector2Int> getRoute() {
        return route;
    }

    // public GameObject getGameObject() {
        // return animation.getKeyFrame(stepsInTime, true);
    // }

    // public TextureRegion getCurrentDeathFrame() {
    //     return animation.getKeyFrame(deathElapsedTime, true);
    // }

//     public String toString() {
//         StringBuilder sb = new StringBuilder();
//         sb.append("Creep[");
// //        sb.append("route:" + route + ",");
//         sb.append("oldPosition:" + oldPosition + ",");
//         sb.append("newPosition:" + newPosition + ",");
//         sb.append("hp:" + hp + ",");
//         sb.append("speed:" + speed + ",");
//         sb.append("stepsInTime:" + stepsInTime + ",");
//         sb.append("deathElapsedTime:" + deathElapsedTime + ",");
//         sb.append("circle1:" + circle1 + ",");
//         sb.append("circle2:" + circle2 + ",");
//         sb.append("templateForUnit:" + templateForUnit + ",");
//         sb.append("direction:" + direction + ",");
//         sb.append("animation:" + animation + ",");
//         sb.append("shellEffectTypes:" + shellEffectTypes + ",");
//         sb.append("]");
//         return sb.toString();
//     }
}

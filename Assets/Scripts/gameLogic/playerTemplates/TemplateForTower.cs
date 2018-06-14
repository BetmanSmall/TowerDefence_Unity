using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class TemplateForTower {
    public Faction faction;
    public string templateName;

    // public Dictionary<string, string> properties; // mb? why not?
    public string   factionName;
    public string   name;
    // public int      radiusDetection;
    // public float    radiusFlyShell;
    public int      damage = int.MinValue;
    public int      size = int.MinValue;
    public int      cost = int.MinValue;
    // public float    ammoSize = float.MinValue;
    // public float    ammoSpeed = float.MinValue;
    // public float    reloadTime = float.MinValue;
    // public TowerAttackType towerAttackType;
    // public ShellAttackType shellAttackType;
    // public ShellEffectType shellEffectType;
    // public int      capacity == int.MinValue;
//    public int ammoDistance;

    // public TiledMapTile idleTile;
    public GameObject modelObject;
    // public ObjectMap<string, TiledMapTile> ammunitionPictures;

    public TemplateForTower(string templateFilePath) /*throws Exception*/ {
        Debug.Log("TemplateForTower::TemplateForTower(" + templateFilePath + "); -- ");
        TextAsset textAsset = Resources.Load<TextAsset>(templateFilePath); // Не может загрузить TextAsset с расширением tmx только xml и другое гавно!
        Debug.Log("TemplateForTower::TemplateForTower(); -- textAsset:" + textAsset);
        if (textAsset == null) {
            Debug.Log("TemplateForTower::TemplateForTower(); -- Can't load:" + templateFilePath);
            throw new System.Exception("TemplateForTower::TemplateForTower(); -- Can't load:" + templateFilePath);
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);
        try {
            XmlNode templateORtileset = xmlDoc.ChildNodes[1];
            this.templateName = (templateORtileset.Attributes["name"]!=null) ? templateORtileset.Attributes["name"].Value : null;

            XmlNodeList templateNodeList = templateORtileset.ChildNodes;
            foreach(XmlNode templateNodeChild in templateNodeList) {
                // XmlReader.Element properties = templateORtileset.getChildByName("properties");
                if(templateNodeChild.Name.Equals("properties")) {
                // if (properties != null) {
                    XmlNodeList propertiesNodeList = templateNodeChild.ChildNodes;
                    // for (XmlReader.Element property : properties.getChildrenByName("property")) {
                    foreach(XmlNode propertiesChildNode in propertiesNodeList) {
                        if(propertiesChildNode.Name.Equals("property")) {
                            string key = (propertiesChildNode.Attributes["name"]!=null)?propertiesChildNode.Attributes["name"].Value:"BAD";
                            string value = (propertiesChildNode.Attributes["value"]!=null)?propertiesChildNode.Attributes["value"].Value:"BAD";
                            if(key.Equals("BAD") || value.Equals("BAD")) {
                                throw new System.Exception("TemplateForTower::TemplateForTower(); -- Bad property:" + propertiesChildNode);
                            } else if(key.Equals("factionName")) {
                                this.factionName = value;
                            } else if (key.Equals("name")) {
                                this.name = value;
                            // } else if (key.Equals("radiusDetection")) {
                            //     this.radiusDetection = int.Parse(value);
                            // } else if (key.Equals("radiusFlyShell")) {
                            //     this.radiusFlyShell = float.Parse(value);
                            } else if (key.Equals("damage")) {
                                this.damage = int.Parse(value);
                            } else if (key.Equals("size")) {
                                this.size = int.Parse(value);
                            } else if (key.Equals("cost")) {
                                this.cost = int.Parse(value);
                            // } else if (key.Equals("ammoSize")) {
                            //     this.ammoSize = float.Parse(value);
                            // } else if (key.Equals("ammoSpeed")) {
                            //     this.ammoSpeed = float.Parse(value);
                            // } else if (key.Equals("reloadTime")) {
                            //     this.reloadTime = float.Parse(value);
                            // } else if (key.Equals("towerAttackType")) {
                            //     this.towerAttackType = TowerAttackType.getType(value);
                            // } else if (key.Equals("shellAttackType")) {
                            //     this.shellAttackType = ShellAttackType.getType(value);
                            // } else if (key.Equals("shellEffectType")) {
                            //     ShellEffectType.ShellEffectEnum shellEffectEnum = ShellEffectType.ShellEffectEnum.getType(value);
                            //     this.shellEffectType = new ShellEffectType(shellEffectEnum);
                            // } else if (key.Equals("shellEffectType_time")) {
                            //     if(shellEffectType != null) {
                            //         shellEffectType.time = float.Parse(value);
                            //     }
                            // } else if (key.Equals("shellEffectType_damage")) {
                            //     if(shellEffectType != null) {
                            //         shellEffectType.damage = float.Parse(value);
                            //     }
                            // } else if (key.Equals("shellEffectType_speed")) {
                            //     if(shellEffectType != null) {
                            //         shellEffectType.speed = float.Parse(value);
                            //     }
                            }
                        } else {
                            Debug.Log("TemplateForTower::TemplateForTower(); -- In properties node bad node:" + propertiesChildNode.Name);
                            // return;
                        }
                    }
                } else if(templateNodeChild.Name.Equals("model")) {
                    Debug.Log("TemplateForTower::TemplateForTower(); -- templateFilePath:" + templateFilePath);
                    string relativeModelSource = (templateNodeChild.Attributes["source"]!=null)?templateNodeChild.Attributes["source"].Value:null;
                    Debug.Log("TemplateForTower::TemplateForTower(); -- relativeModelSource:" + relativeModelSource);
                    if(relativeModelSource != null) {
                        string modelSource = MapLoader.findFile(templateFilePath, relativeModelSource);
                        Debug.Log("TemplateForTower::TemplateForTower(); -- modelSource:" + modelSource);
                        modelObject = Resources.Load(modelSource) as GameObject;
                        modelObject.AddComponent<Animation>();
                        Animation animationClip = modelObject.GetComponent<Animation>();       
                        Debug.Log("TemplateForTower::TemplateForTower(); -- modelObject:" + modelObject);
                    }
                }
            }

            validate();
        } catch (System.Exception exp) {
            Debug.Log("TemplateForTower::TemplateForTower(); -- Could not load TemplateForTower from " + templateFilePath + " Exp:" + exp);
            throw new System.Exception("TemplateForTower::TemplateForTower(); -- Could not load TemplateForTower from " + templateFilePath + " Exp:" + exp);
        }
    }

    private void validate() {
        // Need check range values
        if(this.templateName != null)
            Debug.Log("TemplateForTower::validate(); -- Load TemplateForTower: " + this.templateName);
        if(this.factionName == null)
            Debug.Log("TemplateForTower::validate(); -- Can't get 'factionName'! Check the file");
        else if(this.name == null)
            Debug.Log("TemplateForTower::validate(); -- Can't get 'name'! Check the file");
        // else if(this.radiusDetection == null && this.towerAttackType != TowerAttackType.Pit)
        //     Debug.Log("TemplateForTower::validate(); -- Can't get 'radiusDetection'! Check the file");
        // else if(this.radiusFlyShell == null && this.shellAttackType != ShellAttackType.FirstTarget)
        //     Debug.Log("TemplateForTower::validate(); -- Can't get 'radiusFlyShell'! Check the file");
//            this.radiusFlyShell = 0f;
        else if(this.damage == int.MinValue)
            Debug.Log("TemplateForTower::validate(); -- Can't get 'damage'! Check the file");
        else if(this.size == int.MinValue)
            Debug.Log("TemplateForTower::validate(); -- Can't get 'size'! Check the file");
        else if(this.cost == int.MinValue)
            Debug.Log("TemplateForTower::validate(); -- Can't get 'cost'! Check the file");
        // else if(this.ammoSize == float.MinValue)
        //     Debug.Log("TemplateForTower::validate(); -- Can't get 'ammoSize'! Check the file");
        // else if(this.ammoSpeed == float.MinValue)
        //     Debug.Log("TemplateForTower::validate(); -- Can't get 'ammoSpeed'! Check the file");
        // else if(this.reloadTime == float.MinValue)
        //     Debug.Log("TemplateForTower::validate(); -- Can't get 'reloadTime'! Check the file");
        // else if(this.towerAttackType == null)
        //     Debug.Log("TemplateForTower::validate(); -- Can't get 'towerAttackType'! Check the file");
        // else if(this.shellAttackType == null && this.towerAttackType != TowerAttackType.Pit)
        //     Debug.Log("TemplateForTower::validate(); -- Can't get 'shellAttackType'! Check the file");
//        else if(this.shellEffectType == null)
//            Debug.Log("TemplateForTower::validate(); -- Can't get 'shellEffectEnum'! Check the file");
        // else if(this.towerAttackType == TowerAttackType.Pit && this.capacity == int.MinValue)
        //     Debug.Log("TemplateForTower::validate(); -- Can't get 'capacity'! When towerAttackType==Pit");

        // if(idleTile == null)
        //     Debug.Log("TemplateForTower::validate(); -- Can't get 'idleTile'! Check the file");
        // else if(ammunitionPictures.size == 0)
        //     Debug.Log("TemplateForTower::validate(); -- Can't get 'ammo'! Check the file");
    }

    public string toString() {
        return toString(false);
    }

    public string toString(bool full) {
        string sb = "";
        sb += ("TemplateForTower[");
        sb += ("templateName:" + templateName);
        if(full) {
            sb += ("," + "factionName:" + factionName);
            sb += ("," + "name:" + name);
            // sb += ("," + "radiusDetection:" + radiusDetection);
            // sb += ("," + "radiusFlyShell:" + radiusFlyShell);
            sb += ("," + "damage:" + damage);
            sb += ("," + "size:" + size);
            sb += ("," + "cost:" + cost);
            // sb += ("," + "ammoSize:" + ammoSize);
            // sb += ("," + "ammoSpeed:" + ammoSpeed);
            // sb += ("," + "reloadTime:" + reloadTime);
            // sb += ("," + "towerAttackType:" + towerAttackType);
            // sb += ("," + "shellAttackType:" + shellAttackType);
            // sb += ("," + "shellEffectEnum:" + shellEffectType);
            // sb += ("," + "capacity:" + capacity);
        }
        sb += ("]");
        return sb;
    }

    public void setFaction(Faction faction) {
        this.faction = faction;
    }

    public string getFactionName() {
        return factionName;
    }
}
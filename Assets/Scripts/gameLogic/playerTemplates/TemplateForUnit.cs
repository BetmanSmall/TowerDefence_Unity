using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEditor;

/**
 * Created by betmansmall on 09.02.2016.
 */
public class TemplateForUnit {
    public Faction faction;
    public string templateName;

    // public Dictionary<string, string> properties; // mb? why not?
    public string   factionName;
    public string   name;
    public float    speed;
    public int      healthPoints;
    public int      cost;
    public int      bounty;
    public string   type;
    public string   modelSource;
    public GameObject modelGameObject;
    public AnimationClip[] animationClips;
    public Animation[] animations;
    public Motion[] motions;
    public Object[] allObjects;

    public TemplateForUnit(string templateFilePath) {
        Debug.Log("TemplateForUnit::TemplateForUnit(" + templateFilePath + "); -- ");
        TextAsset textAsset = Resources.Load<TextAsset>(templateFilePath); // Не может загрузить TextAsset с расширением tmx только xml и другое гавно! /Э/ И еще ужаснто работает вообще Resources
        Debug.Log("TemplateForUnit::TemplateForUnit(); -- textAsset:" + textAsset);
        if (textAsset == null) {
            Debug.Log("TemplateForUnit::TemplateForUnit(); -- Can't load:" + templateFilePath);
            throw new System.Exception("TemplateForUnit::TemplateForUnit(); -- Can't load:" + templateFilePath);
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);
        try {
            // --!-- JAVA OLD CODE--1--
            // XmlReader xmlReader = new XmlReader();
            // Element templateORtileset = xmlReader.parse(templateFilePath);
            // this.templateName = templateORtileset.getAttribute("name");

            // int firstgid = templateORtileset.getIntAttribute("firstgid", 0);
            // int tilewidth = templateORtileset.getIntAttribute("tilewidth", 0);
            // int tileheight = templateORtileset.getIntAttribute("tileheight", 0);
            // int spacing = templateORtileset.getIntAttribute("spacing", 0);
            // int margin = templateORtileset.getIntAttribute("margin", 0);

            // Element properties = templateORtileset.getChildByName("properties");
            // if (properties != null) {
            //     for (Element property : properties.getChildrenByName("property")) {
            //         String key = property.getAttribute("name");
            //         String value = property.getAttribute("value");
            //         if (key.equals("bounty")) {
            //             this.bounty = Integer.parseInt(value);
            //         } else if (key.equals("cost")) {
            //             this.cost = Integer.parseInt(value);
            //         } else if (key.equals("factionName")) {
            //             this.factionName = value;
            //         } else if (key.equals("healthPoints")) {
            //             this.healthPoints = Integer.parseInt(value);
            //         } else if (key.equals("name")) {
            //             this.name = value;
            //         } else if (key.equals("speed")) {
            //             this.speed = Float.parseFloat(value);
            //         } else if (key.equals("type")) {
            //             this.type = value;
            //         }
            //     }
            // }
            // --!-- JAVA OLD CODE--2--
            
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
                                throw new System.Exception("TemplateForUnit::TemplateForUnit(); -- Bad property:" + propertiesChildNode);
                            } else if(key.Equals("factionName")) {
                                this.factionName = value;
                            } else if (key.Equals("name")) {
                                this.name = value;
                            } else if (key.Equals("speed")) {
                                this.speed = float.Parse(value);
                            } else if (key.Equals("healthPoints")) {
                                this.healthPoints = int.Parse(value);
                            } else if (key.Equals("cost")) {
                                this.cost = int.Parse(value);
                            } else if (key.Equals("bounty")) {
                                this.bounty = int.Parse(value);
                            } else if (key.Equals("type")) {
                                this.type = value;
                            }
                        } else {
                            Debug.Log("TemplateForUnit::TemplateForUnit(); -- In properties node bad node:" + propertiesChildNode.Name);
                            // return;
                        }
                    }
                } else if(templateNodeChild.Name.Equals("model")) {
                    Debug.Log("TemplateForUnit::TemplateForUnit(); -- templateFilePath:" + templateFilePath);
                    string relativeModelSource = (templateNodeChild.Attributes["source"]!=null)?templateNodeChild.Attributes["source"].Value:null;
                    Debug.Log("TemplateForUnit::TemplateForUnit(); -- relativeModelSource:" + relativeModelSource);
                    if(relativeModelSource != null) {
                        string modelSource = MapLoader.findFile(templateFilePath, relativeModelSource);
                        this.modelSource = modelSource;
                        Debug.Log("TemplateForUnit::TemplateForUnit(); -- modelSource:" + modelSource);

                        allObjects = AssetDatabase.LoadAllAssetsAtPath("Assets/Resources/" + modelSource + ".fbx");
                        Debug.Log("TemplateForUnit::TemplateForUnit(); -- allObjects.Length:" + allObjects.Length);

                        modelGameObject = Resources.Load<GameObject>(modelSource); // or GameObject?
                        Debug.Log("TemplateForUnit::TemplateForUnit(); -- modelObject:" + modelGameObject);
                        // modelGameObject.

                        animationClips = Resources.LoadAll<AnimationClip>(modelSource);
                        Debug.Log("TemplateForUnit::TemplateForUnit(); -- animationClips.Length:" + animationClips.Length);
                        foreach(AnimationClip animationClip in animationClips) {
                            Debug.Log("TemplateForUnit::TemplateForUnit(); -- animationClip:" + animationClip.name);

                            // Debug.Log("TemplateForUnit::TemplateForUnit(); -- animationClip.ToString():" + animationClip.);
                        }
                        // Resources.LoadAssetAtPath(modelSource, typeof(Motion[]));

                        foreach (Object oneObject in allObjects) {
                            Debug.Log("TemplateForUnit::TemplateForUnit(); -- oneObject.name:" + oneObject.name);
                            if (oneObject.GetType() == typeof(AnimationClip)) {
                        //         // animationClips.
                            }
                        }

                        motions = Resources.LoadAll<Motion>(modelSource);
                        Debug.Log("TemplateForUnit::TemplateForUnit(); -- motions.Length:" + motions.Length);
                    }
                }
            }

            validate();
        } catch (System.Exception exp) {
            Debug.LogError("TemplateForUnit::TemplateForUnit(); -- Could not load TemplateForUnit from " + templateFilePath + " Exp:" + exp);
            throw new System.Exception("TemplateForUnit::TemplateForUnit(); -- Could not load TemplateForUnit from " + templateFilePath + " Exp:" + exp);
        }
    }

    private void validate() {
        // Need check range values || In Futeriii!1!|!
        if(this.templateName != null)
            Debug.Log("TemplateForUnit::validate(); -- Load TemplateForUnit: " + this.templateName);
        if(this.factionName == null)
            Debug.Log("TemplateForUnit::validate(); -- Can't get 'factionName'! Check the file");
        else if(this.name == null)
            Debug.Log("TemplateForUnit::validate(); -- Can't get 'name'! Check the file");
        else if(this.speed == null)
            Debug.Log("TemplateForUnit::validate(); -- Can't get 'speed'! Check the file");
        else if(this.healthPoints == null)
            Debug.Log("TemplateForUnit::validate(); -- Can't get 'healthPoints'! Check the file");
        else if(this.cost == null)
            Debug.Log("TemplateForUnit::validate(); -- Can't get 'cost'! Check the file");
        else if(this.bounty == null)
            Debug.Log("TemplateForUnit::validate(); -- Can't get 'bounty'! Check the file");
        else if(this.type == null)
            Debug.Log("TemplateForUnit::validate(); -- Can't get 'type'! Check the file");
    }

    public void setFaction(Faction faction) {
        this.faction = faction;
    }

    public string getFactionName() {
        return factionName;
    }

    public string toString() {
        return toString(false);
    }

    public string toString(bool full) {
        string sb = "";
        sb += ("TemplateForUnit[");
        sb += ("templateName:" + templateName);
        if(full) {
            sb += ("," + "bounty:" + bounty);
            sb += ("," + "cost:" + cost);
            sb += ("," + "factionName:" + factionName);
            sb += ("," + "healthPoints:" + healthPoints);
            sb += ("," + "name:" + name);
            sb += ("," + "speed:" + speed);
            sb += ("," + "type:" + type);
        }
        sb += ("]");
        return sb;
    }
}
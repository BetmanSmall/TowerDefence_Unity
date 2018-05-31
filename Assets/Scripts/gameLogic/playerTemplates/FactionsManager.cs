using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class FactionsManager {
    private List<Faction> factions;
    private float levelOfDifficulty;

    public FactionsManager(float levelOfDifficulty) {
        this.factions = new List<Faction>();
        this.levelOfDifficulty = levelOfDifficulty;
    }

    // public void addUnitToFaction(TemplateForUnit unit) {
    //     String newFactionName = unit.getFactionName();
    //     for (Faction faction : factions) {
    //         if (faction.getName().equals(newFactionName)) {
    //             faction.getTemplateForUnits().add(unit);
    //             unit.setFaction(faction);
    //             return;
    //         }
    //     }
    //     Faction faction = new Faction(newFactionName);
    //     faction.getTemplateForUnits().add(unit);
    //     unit.setFaction(faction);
    //     factions.add(faction);
    // }

    public void addTowerToFaction(TemplateForTower tower) {
        Debug.Log("FactionsManager::addTowerToFaction(); -- Tower name:" + tower.name);
        string newFactionName = tower.getFactionName();
        foreach (Faction faction in factions) {
            if (faction.getName().Equals(newFactionName)) {
                faction.getTemplateForTowers().Add(tower);
                tower.setFaction(faction);
                return;
            }
        }
        Faction newFaction = new Faction(newFactionName);
        newFaction.getTemplateForTowers().Add(tower);
        tower.setFaction(newFaction);
        factions.Add(newFaction);
    }

    // public TemplateForUnit getRandomTemplateForUnitFromFirstFaction() {
    //     Faction faction = factions.first();
    //     if (faction != null) {
    //         TemplateForUnit templateForUnit = faction.getTemplateForUnits().random();
    //         if (templateForUnit != null) {
    //             return templateForUnit;
    //         }
    //     }
    //     return null;
    // }

    public TemplateForTower getRandomTemplateForTowerFromFirstFaction() {
        Faction faction = factions[0];
        if (faction != null) {
            List<TemplateForTower> templateForTowers = faction.getTemplateForTowers();
            TemplateForTower templateForTower = templateForTowers[Random.Range(0, templateForTowers.Count-1)];
            if (templateForTower != null) {
                return templateForTower;
            }
        }
        return null;
    }

    public TemplateForTower getRandomTemplateForTowerFromAllFaction() {
        List<TemplateForTower> allTowers = getAllTemplateForTowers();
    //    int r = 0 + (int) (Math.random() * allTowers.size);
    //    return allTowers.get(r);
        TemplateForTower template = allTowers[Random.Range(0, allTowers.Count-1)];
        Debug.Log("FactionsManager::getRandomTemplateForTowerFromAllFaction(); -- template" + template);
        return template;
    }
    // public TemplateForUnit getTemplateForUnitFromFirstFactionByIndex(int index) {
    //     Faction faction = factions.first();
    //     if (faction != null) {
    //         TemplateForUnit templateForUnit = faction.getTemplateForUnits().get(index);
    //         if (templateForUnit != null) {
    //             return templateForUnit;
    //         }
    //     }
    //     return null;
    // }

    // public TemplateForUnit getTemplateForUnitFromFirstFactionByName(String templateName) {
    //     Faction faction = factions.first();
    //     if (faction != null) {
    //         for (TemplateForUnit templateForUnit : faction.getTemplateForUnits()) {
    //             if (templateForUnit != null) {
    //                 if (templateForUnit.getTemplateName().equals(templateName)) {
    //                     return templateForUnit;
    //                 }
    //             }
    //         }
    //     }
    //     return null;
    // }

    // public TemplateForUnit getTemplateForUnitByName(String templateName) {
    //     for (Faction faction : factions) {
    //         if (faction != null) {
    //             for (TemplateForUnit templateForUnit : faction.getTemplateForUnits()) {
    //                 if (templateForUnit != null) {
    //                     if (templateForUnit.getTemplateName().equals(templateName)) {
    //                         return templateForUnit;
    //                     }
    //                 }
    //             }
    //         }
    //     }
    //     return null;
    // }

    public List<TemplateForTower> getAllFirstTowersFromFirstFaction() {
        return factions[0].getTemplateForTowers();
    }

    public List<TemplateForTower> getAllTemplateForTowers() {
        List<TemplateForTower> allTowers = new List<TemplateForTower>();
        foreach (Faction faction in factions) {
            foreach (TemplateForTower template in faction.getTemplateForTowers()) {
                allTowers.Add(template);
            }
        }
        return allTowers;
    }

    // public Array<TemplateForUnit> getAllTemplateForUnits() {
    //     Array<TemplateForUnit> allTowers = new Array<TemplateForUnit>();
    //     for (Faction faction : factions) {
    //         for (TemplateForUnit template : faction.getTemplateForUnits()) {
    //             allTowers.add(template);
    //         }
    //     }
    //     return allTowers;
    // }

    public void loadFactions() {
        List<FileInfo> factions = new List<FileInfo>();
        DirectoryInfo info = new DirectoryInfo("Assets/Resources/maps/factions");
        FileInfo[] fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo) {
            Debug.Log("FactionsManager::loadFactions(); -- file:" + file);
            factions.Add(file);
        }
        Debug.Log("FactionsManager::loadFactions(); -- factions.Count:" + factions.Count);
        foreach (FileInfo factionFileInfo in factions) {
        Debug.Log("FactionsManager::loadFactions(); -- factionFileInfo.Extension:" + factionFileInfo.Extension);
            if (factionFileInfo.Extension.Equals(".xml")) { // need Equals("fac")
                loadFaction(factionFileInfo);
            }
        }
    }

    private string subPathToResources(string filePath) {
        Debug.Log("FactionsManager::subPathToResources(); -- filePath:" + filePath);
        string exitStr = "";
        string[] strs = filePath.Split('\\');
        for(int k = 0; k < strs.Length; k++) {
            string str = strs[k];
            if(str.Equals("Resources")) {
                do {
                    str = strs[++k];
                    exitStr = exitStr + "/" + str;
                } while(k!=strs.Length-1);
            }
        }
        exitStr = exitStr.Substring(1);
        exitStr = exitStr.Substring(0, exitStr.LastIndexOf("."));
        Debug.Log("FactionsManager::subPathToResources(); -- exitStr:" + exitStr);
        return exitStr;
    }

    private void loadFaction(FileInfo factionFile) {
        if(factionFile != null/* && !factionFile.isDirectory()*/) {
            Debug.Log("FactionsManager::loadFaction(" + factionFile + "); -- ");
            try {
                string factionFilePath = subPathToResources(factionFile.FullName);
                TextAsset textAsset = Resources.Load<TextAsset>(factionFilePath); // Не может загрузить TextAsset с расширением tmx только xml и другое гавно!
                Debug.Log("FactionsManager::loadFaction(); -- textAsset:" + textAsset);
                if (textAsset == null) {
                    Debug.Log("FactionsManager::loadFaction(); -- Can't load faction:" + factionFilePath);
                    return;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(textAsset.text);
                XmlElement root = (XmlElement)xmlDoc.FirstChild.NextSibling;
                string factionName = (root.Attributes["name"]!=null)?root.Attributes["name"].Value:null;
                if (factionName != null) {
                    Faction faction = new Faction(factionName);
                    // XmlNodeList templateForUnitElements = root.GetElementsByTagName("templateForUnit");
                    // foreach (XmlElement templateForUnitElement in templateForUnitElements) {
                    //     string source = (templateForUnitElement.Attributes["source"]!=null)?templateForUnitElement.Attributes["source"].Value:null;
                    //     if (source != null) {
                    //         FileHandle templateFile = getRelativeFileHandle(factionFile, source);
                    //         TemplateForUnit templateForUnit = new TemplateForUnit(templateFile);
                    //         templateForUnit.setFaction(faction);
                    //         templateForUnit.healthPoints = (int)(templateForUnit.healthPoints*levelOfDifficulty); // simple level of difficulty
                    //         faction.getTemplateForUnits().add(templateForUnit);
                    //     }
                    // }
                    XmlNodeList templateForTowerElements = root.GetElementsByTagName("templateForTower");
                    foreach (XmlElement templateForTowerElement in templateForTowerElements) {
                        string source = (templateForTowerElement.Attributes["source"]!=null)?templateForTowerElement.Attributes["source"].Value:null;
                        if (source != null) {
                            string templateFile = MapLoader.findFile(factionFilePath, source);
                            TemplateForTower templateForTower = new TemplateForTower(templateFile);
                            templateForTower.setFaction(faction);
                            Debug.Log("FactionsManager::loadFaction(); -1- faction.getTemplateForTowers().Count:" + faction.getTemplateForTowers().Count);
                            faction.getTemplateForTowers().Add(templateForTower);
                            Debug.Log("FactionsManager::loadFaction(); -2- faction.getTemplateForTowers().Count:" + faction.getTemplateForTowers().Count);
                        }
                    }
                    Debug.Log("FactionsManager::loadFaction(); -- faction:" + faction);
                    factions.Add(faction);
                }
            } catch (System.Exception exp) {
                Debug.LogError("FactionsManager::loadFaction(); -- Could not load Faction! Exp:" + exp);
            }
        } else {
            Debug.LogError("FactionsManager::loadFaction(); -- Could not load Faction! (factionFile == null) or (factionFile.isDirectory() == true)");
        }
    }
}

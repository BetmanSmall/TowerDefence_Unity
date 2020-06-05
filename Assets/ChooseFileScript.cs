using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ChooseFileScript : MonoBehaviour
{
    [SerializeField] 
    string[] mapsLinkList;
    string[] mapsName;
    [SerializeField] 
    private GameObject DropDown;
    [SerializeField] 
    private GameObject GF;
    private Dropdown M_DropDown;
    [SerializeField] 
    private Text FullPath;

    [SerializeField]
    GameObject MapListsInUnity;

    public void Start()
    {
        //Fetch the Dropdown GameObject
        //Add listener for when the value of the Dropdown changes, to take action
        M_DropDown = DropDown.GetComponent<Dropdown>();
        M_DropDown.onValueChanged.AddListener(delegate
            {
                DropdownValueChanged(M_DropDown);
            });

        //Initialise the Text to say the first value of the Dropdown
    }

    // Update is called once per frame
    public void GetItemList()
    {
        M_DropDown.options.Clear();
        mapsLinkList = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Assets/Resources/maps/", "*.tmx",
            SearchOption.AllDirectories);
        mapsName = new String[mapsLinkList.Length];

        for (int i = 0; i < mapsLinkList.Length; i++)
        {
            mapsName[i] = Path.GetFileNameWithoutExtension(mapsLinkList[i]);
        }

        DropDown.GetComponentInChildren<Text>().text = mapsName[0];
        for (int i = 0; i < mapsName.Length; i++) {
            M_DropDown.options.Add(new Dropdown.OptionData(mapsName[i]));
            FullPath.text = "Full Path: " + mapsLinkList[M_DropDown.value];
        }
        
    }
    public void StartEditing()
    {
        GF.GetComponent<GameField>().MapLoad(mapsName[M_DropDown.value]);
        LoadTileSet();
    }
    
    public void LoadTileSet() {
        float interval = 0;
        GameObject goGameField = GameObject.Find("GameField");
        GameObject Container = GameObject.Find("Container");
        if (goGameField != null) {
            Debug.Log("Container.transform:" + Container.transform);
            Debug.Log("Container.transform.childCount:" + Container.transform.childCount);
            for (int i = 0; i < Container.transform.childCount; i++) {
                Destroy(Container.gameObject.transform.GetChild(i).gameObject);
            }
            
            GameField gameField = goGameField.GetComponent<GameField>();
            foreach (KeyValuePair<int,TileSetOrModelsSet> valuePair in gameField.map.tileSetsOrModelsSets) {
                TileSetOrModelsSet tileSetOrModelsSet = valuePair.Value;
                if (tileSetOrModelsSet != null) {
                    foreach (KeyValuePair<int, TileModel> keyValuePair in tileSetOrModelsSet.tileModels) {
                        TileModel tileModel = keyValuePair.Value;
                        if (tileModel != null && tileModel.sprite != null) {
                            GameObject newGameObject = (GameObject)Instantiate(tileModel.modelObject, Container.transform.position, Quaternion.identity);
                            // GameObject newGameObject = new GameObject(tileModel.id + "");
                            // SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
                            // spriteRenderer.sprite = tileModel.sprite;
                            newGameObject.transform.SetParent(Container.transform, true);
                            newGameObject.transform.localScale = new Vector3(40, 40, 40);
                            newGameObject.transform.localRotation = new Quaternion(-1, 0, 0, Container.transform.rotation.w);
                            newGameObject.transform.position = new Vector3(newGameObject.transform.position.x, newGameObject.transform.position.y - interval, newGameObject.transform.position.z);
                            interval += 5;
                        }
                    }
                }
            }
        }
    }
    
    public void Back()
    {
        while (true)
        {
            for (int i = 0; i <= GF.transform.childCount; i++)
            {
                GF.gameObject.transform.GetChild(i).SetParent(MapListsInUnity.transform);
            }

            if (Time.time > 10)
            {
                break;
            }
        }
    }

    void DropdownValueChanged(Dropdown change)
    {
        FullPath.text =  "Full Path: " + mapsLinkList[change.value];
    }
}

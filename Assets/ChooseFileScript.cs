using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ChooseFileScript : MonoBehaviour {
    [SerializeField] string[] mapsLinkList;
    string[] mapsName;
    [SerializeField] private GameObject DropDown;
    [SerializeField] private GameObject GF;
    private Dropdown M_DropDown;
    [SerializeField] private Text FullPath;

    [SerializeField] GameObject MapListsInUnity;
    [SerializeField] private GameObject DropDownLayers;

    public void Start() {
        M_DropDown = DropDown.GetComponent<Dropdown>();
        M_DropDown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(M_DropDown);
        });
    }

    public void GetItemList() {
        M_DropDown.options.Clear();
        DropDownLayers.GetComponent<Dropdown>().options.Clear();
        mapsLinkList = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Assets/Resources/maps/", "*.tmx", SearchOption.AllDirectories);
        mapsName = new String[mapsLinkList.Length];

        for (int i = 0; i < mapsLinkList.Length; i++) {
            mapsName[i] = Path.GetFileNameWithoutExtension(mapsLinkList[i]);
        }

        DropDown.GetComponentInChildren<Text>().text = mapsName[0];
        for (int i = 0; i < mapsName.Length; i++) {
            M_DropDown.options.Add(new Dropdown.OptionData(mapsName[i]));
            FullPath.text = "Full Path: " + mapsLinkList[M_DropDown.value];
        }
    }

    public void StartEditing() {
        GF.GetComponent<GameField>().MapLoad(mapsName[M_DropDown.value]);
        LoadTileSet();
    }

    public void LoadTileSet() {
        float objectSize = 80f;
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
            Scrollbar scrollbar = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();
            scrollbar.value = 1;
            RectTransform rectTransform = Container.gameObject.GetComponent<RectTransform>();
            if (rectTransform != null) {
                rectTransform.sizeDelta = new Vector2(138, (objectSize * 3f) * gameField.map.tileSetsOrModelsSets[0].tileModels.Count);
            }

            float containerHalfHeight = ((objectSize * 3f) * gameField.map.tileSetsOrModelsSets[0].tileModels.Count) / 2f;

            foreach (KeyValuePair<int, TileSetOrModelsSet> valuePair in gameField.map.tileSetsOrModelsSets) {
                TileSetOrModelsSet tileSetOrModelsSet = valuePair.Value;
                if (tileSetOrModelsSet != null) {
                    foreach (KeyValuePair<int, TileModel> keyValuePair in tileSetOrModelsSet.tileModels) {
                        TileModel tileModel = keyValuePair.Value;
                        if (tileModel != null && tileModel.sprite != null && tileModel.modelObject != null) {
                            // GameObject newGameObject = new GameObject(tileModel.id + "");
                            // SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
                            // spriteRenderer.sprite = tileModel.sprite;
                            GameObject newGameObject = (GameObject) Instantiate(tileModel.modelObject, Container.transform.position, Quaternion.identity);
                            if (newGameObject.transform.GetChild(0) != null) {
                                newGameObject.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
                            }

                            newGameObject.layer = 8;
                            newGameObject.transform.GetChild(0).gameObject.layer = 8;
                            newGameObject.transform.SetParent(Container.transform, true);
                            newGameObject.transform.localScale = new Vector3(objectSize / 3, objectSize / 3, objectSize / 3);
                            newGameObject.transform.localRotation = new Quaternion(-1, 0, 0, Container.transform.rotation.w);
                            newGameObject.transform.localPosition = new Vector3(0, interval + containerHalfHeight, 0);
                            MeshRenderer meshRenderer = gameObject.GetComponentsInChildren<MeshRenderer>()[0];
                            if (meshRenderer != null) {
                                interval -= meshRenderer.bounds.size.z * (objectSize*3f);
                            }
                        } else {
                            Debug.Log("tileModel:" + tileModel);
                            Debug.Log("tileModel.sprite:" + tileModel.sprite);
                            Debug.Log("tileModel.modelObject:" + tileModel.modelObject);
                        }
                    }
                }
            }
            foreach (KeyValuePair<int, MapLayer> layer in GF.GetComponent<GameField>().map.mapLayers) {
                Debug.Log("MapLoader::loadMap(); -- property.Key[" + layer.Key + "]:" + layer.Value + " ForEach");
                DropDownLayers.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(layer.Value.name));
            }
        }
    }

    public void Back() {
        for (int b = 0; b < 1000; b++) {
            for (int i = 0; i <= GF.transform.childCount; i++) {
                Destroy(GF.gameObject.transform.GetChild(i));
            }
        }
    }

    void DropdownValueChanged(Dropdown change) {
        FullPath.text = "Full Path: " + mapsLinkList[change.value];
    }
}

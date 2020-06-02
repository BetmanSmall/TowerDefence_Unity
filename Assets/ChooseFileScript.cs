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
        for (int i = 0; i <= mapsName.Length; i++) {
            M_DropDown.options.Add(new Dropdown.OptionData(mapsName[i]));
            FullPath.text = "Full Path: " + mapsLinkList[M_DropDown.value];
        }
        
    }
    public void StartEditing()
    {
        GF.GetComponent<GameField>().MapLoad(mapsName[M_DropDown.value]);
    }
    public void Back()
    {
        while (true)
        {
            for (int i = 0; i <= GF.transform.GetChildCount(); i++)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using UnityEngine.UI;
using Unity.Services.Core;
using TMPro;
public class LostStorageAdd : MonoBehaviour
{
    
    public TMP_InputField status;
    public TMP_InputField inpf;
    public TMP_Dropdown itemcategory;
    public TMP_Dropdown losttime;
    public TMP_Dropdown place;
    public TMP_InputField desc;

    public async void Start() 
    {
        await UnityServices.InitializeAsync();
    }

    public async void SaveData() 
    {
        var data = new Dictionary<string, object> { {"ItemCategory",itemcategory.options[itemcategory.value].text}, {"LostTime",losttime.options[losttime.value].text}, {"Place", place.options[place.value].text}, {"Description", desc.text}  };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }


    public async void LoadData() {

       Dictionary<string ,string> serverData=  await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "firstData" });

        if (serverData.ContainsKey("firstData"))
        {
            inpf.text = serverData["firstData"];
        }
        else
        {
            print("Key not found!!");
        }

       
    }

    public async void DeleteKey() {

        await CloudSaveService.Instance.Data.ForceDeleteAsync("firstData");
    }

    public async void RetriveAllKeys() {

        List<string> allKeys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();

        for (int i = 0; i < allKeys.Count; i++)
        {
            print(allKeys[i]);
        }
    }

}

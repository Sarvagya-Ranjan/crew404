using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using UnityEngine.UI;
using Unity.Services.Core;
using TMPro;
using System.IO;
public class LostStorageAdd : MonoBehaviour
{   public TMP_InputField status;
    public TMP_InputField inpf;
    public TMP_Dropdown itemcategory;
    public TMP_Dropdown losttime;
    public TMP_Dropdown place;
    public TMP_InputField desc;

    public Button uploadImageButton;
    public RawImage imageDisplay; // Image component to display the selected image
    private Texture2D uploadedImage;

    public async void Start() 
    {
        await UnityServices.InitializeAsync();
    }

    private IEnumerator LoadImage(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        uploadedImage = new Texture2D(2, 2);
        uploadedImage.LoadImage(fileData); // Load the image from file

        // Assign the loaded image to the UI component (RawImage or Image)
        imageDisplay.texture = uploadedImage;
        imageDisplay.SetNativeSize();
        yield return null;
    }

    public async void SaveData() 
    {
        // Generate a unique key based on timestamp (or any unique ID method)
        string uniqueKey = "LostItem_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss");

         // Convert the image to a byte array
        byte[] imageBytes = uploadedImage.EncodeToJPG();

        // Create a dictionary with the data
        var data = new Dictionary<string, object>
        { 
            {"ItemCategory", itemcategory.options[itemcategory.value].text}, 
            {"LostTime", losttime.options[losttime.value].text}, 
            {"Place", place.options[place.value].text}, 
            {"Description", desc.text},
            { "Image", System.Convert.ToBase64String(imageBytes) } // Store image as Base64 string
        };  
        

        // Save with a unique key
        await CloudSaveService.Instance.Data.ForceSaveAsync(new Dictionary<string, object> { {uniqueKey, data} });
        Debug.Log("Data saved with key: " + uniqueKey);
    }

    private void OpenImagePicker()
    {
        // Open file picker using Native File Picker or any file picker you prefer
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path != null)
            {
                // Load the image as a Texture2D from the file path
                StartCoroutine(LoadImage(path));
            }
            else
            {
                Debug.Log("File path is null.");
            }
        }, new string[] { "image/*" });

        if (permission == NativeFilePicker.Permission.Denied)
        {
            Debug.Log("Permission denied.");
        }
    }

    public async void LoadData() 
    {
        // Load all saved data
        Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "firstData" });

        if (serverData.ContainsKey("firstData"))
        {
            inpf.text = serverData["firstData"];
        }
        else
        {
            print("Key not found!!");
        }
    }

    public async void DeleteKey(string key) 
    {
        await CloudSaveService.Instance.Data.ForceDeleteAsync(key);
        Debug.Log("Data deleted with key: " + key);
    }

    public async void RetrieveAllKeys() 
    {
        List<string> allKeys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();

        foreach (var key in allKeys)
        {
            Debug.Log("Key: " + key);
            Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { key });

            if (serverData.ContainsKey(key))
            {
                Debug.Log("Value for " + key + ": " + serverData[key]);
            }
        }
    }
}

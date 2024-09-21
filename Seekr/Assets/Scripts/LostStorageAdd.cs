using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using UnityEngine.UI;
using Unity.Services.Core;
using TMPro;
using System.IO;
using SimpleFileBrowser;
using Unity.Services.Authentication;


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
        
         // Authenticate the user
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await SignInAnonymously();
        }
        
        if (uploadImageButton)
        {
        uploadImageButton.onClick.AddListener(OpenImagePicker);
        }
        RetrieveAllKeys();
    }

    async void CreateNewUser()
{
    await AuthenticationService.Instance.SignInAnonymouslyAsync();
    Debug.Log("New User ID: " + AuthenticationService.Instance.PlayerId);
}

    // Sign in the user anonymously
    private async System.Threading.Tasks.Task SignInAnonymously()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Signed in successfully!");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError("Failed to sign in: " + ex.Message);
        }
    }

    async void SignOutUser()
{
    await AuthenticationService.Instance.SignOutAsync();
    Debug.Log("User signed out.");
}

// Coroutine to handle the file browser interaction
    IEnumerator ShowLoadDialogCoroutine()
{
    // Use PickMode.Files to select files
    yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, "Select Image", "Select");

    // If the user has selected a file, process it
    if (FileBrowser.Success)
    {
        string filePath = FileBrowser.Result[0]; // Get the selected file path

        // Load the image from the file path
        byte[] fileData = File.ReadAllBytes(filePath);
        uploadedImage = new Texture2D(2, 2);
        uploadedImage.LoadImage(fileData);

        // Display the image in the RawImage UI component
        //imageDisplay.texture = uploadedImage;
        //imageDisplay.SetNativeSize();
    }
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
        SignOutUser();
        CreateNewUser();
    }

    private void OpenImagePicker()
    {
     // Show a file picker for images
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".png", ".jpg", ".jpeg"));
        FileBrowser.SetDefaultFilter(".png");
        
        // Coroutine to handle the file selection
        StartCoroutine(ShowLoadDialogCoroutine());
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

//{"Description":"","Image":"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","ItemCategory":"clothes","LostTime":"Morning (7am-12pm)","Place":"Academic Block"}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScenes : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("homepage");
    }

    public void SceneLoader(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

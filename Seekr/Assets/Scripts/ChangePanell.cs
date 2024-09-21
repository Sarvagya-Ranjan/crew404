using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePanell : MonoBehaviour
{
    public GameObject regPanel;
    public GameObject loginPanel;

    public void RegisterPanel()
    {
        regPanel.SetActive(false);
        loginPanel.SetActive(true);
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

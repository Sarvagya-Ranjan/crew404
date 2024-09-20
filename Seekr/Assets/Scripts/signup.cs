using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signup : MonoBehaviour
{
    public GameObject RegPanel;
    public GameObject LoginPanel;

    void SignUpOpen()
    {
        RegPanel.SetActive(true);
        LoginPanel.SetActive(false);
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

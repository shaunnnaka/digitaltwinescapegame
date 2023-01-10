using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSetting : MonoBehaviour
{
    string ipaddress;
    public Text hasIp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.SetString("IP", hasIp.text);
            PlayerPrefs.Save();
            SceneManager.LoadScene("MyHome");
        }
    }
}

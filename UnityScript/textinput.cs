using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class textinput : MonoBehaviour
{
    public Text text;
    private TouchScreenKeyboard overlayKeyboard;
    public OVRInput.Controller controller;
    public static string inputText = "";
    // Start is called before the first frame update
    void Start()
    {
        overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.RawButton.X))
            overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        if (overlayKeyboard != null)
        {
            text.text = overlayKeyboard.text;
            PlayerPrefs.SetString("textinput", overlayKeyboard.text);
            PlayerPrefs.Save();
        }
        
        

    }
}

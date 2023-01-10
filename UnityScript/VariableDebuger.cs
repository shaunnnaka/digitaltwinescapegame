using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class VariableDebuger : MonoBehaviour
{
    public Text textObj;
    private int _cntFlame;
    // Start is called before the first frame update
    void Start()
    {
        textObj.text = "Variable Debuger";
        _cntFlame = 0;
        PlayerPrefs.SetString("Debug", PlayerPrefs.GetInt("ssi").ToString());
        PlayerPrefs.SetString("Debug1", "void start");
        PlayerPrefs.SetString("Debug2", "void start");
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
        //60フレームに一度Jsonを取ってくる
        if (_cntFlame == 60)
        {
            string _debug = PlayerPrefs.GetString("Debug");
            string _debug1 = PlayerPrefs.GetString("Debug1");
            //calibration
            string _debug2 = "bombisdefusing" + PlayerPrefs.GetInt("bombisdefusing").ToString() + ",chair" + PlayerPrefs.GetInt("Chair0") ;
            textObj.text = _debug + "\n" + _debug1 + "\n" + _debug2;

            _cntFlame = 0;
        }
        _cntFlame++;
    }
}

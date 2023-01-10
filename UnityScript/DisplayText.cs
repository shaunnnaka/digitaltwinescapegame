using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayText : MonoBehaviour
{
    public Text text;
    public WebRequest wr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text =wr.jsonInfo.timer.ToString();
    }
}

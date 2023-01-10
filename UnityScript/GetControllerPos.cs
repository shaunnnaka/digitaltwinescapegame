using System.Collections;
using System.Collections.Generic;
using OVRTouchSample;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class GetControllerPos : MonoBehaviour
{
    //textにアタッチする
    public Text textObj;
    public GameObject hand;
    public OVRInput.Controller controller;

    private Vector3 handPosition;
    // Start is called before the first frame update
    void Start()
    {
        textObj.text = "Contoroller Position";
        
    }

    // Update is called once per frame
    void Update()
    {
        //ワールド座標を取得
        handPosition = hand.transform.position;
        textObj.text = string.Format("lx:{0:f4}, ly:{1:f4}, lz:{2:f4}\n", handPosition.x, handPosition.y, handPosition.z);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using WebSocketSharp;
using System.Text;
using System;
public class WebSocketScript : MonoBehaviour
{
    private WebSocket ws;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _scale = 1.0f;
    [SerializeField] private float _sleep = 0.2f;
    private byte[] bytes;
    // Start is called before the first frame update
    void Start()
    {
        string _ip = PlayerPrefs.GetString("IP");
        ws = new WebSocket("ws://"+ _ip+":8080/");
        ws.OnOpen += (sender, e) => {
            Debug.Log("WebSocket Open");
        };

        ws.OnMessage += (sender, e) => {
            Debug.Log("WebSocket Message Type:, Data: " + e.Data);
        };

        ws.OnError += (sender, e) => {
            Debug.Log("WebSocket Error Message: " + e.Message);
        };

        ws.OnClose += (sender, e) => {
            Debug.Log("WebSocket Close");
        };
        ws.Connect();
        StartCoroutine(CaptureScreenShot(Application.dataPath + "/" + "CameraScreenShot.png"));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ws.Send("Test Message");
        }
    }
    IEnumerator CaptureScreenShot(string filePath)
    {
        while (true)
        {
            var width = (int)(_camera.pixelWidth * _scale);
            var height = (int)(_camera.pixelHeight * _scale);
            var rt = new RenderTexture(width, height, 24);
            var prev = _camera.targetTexture;
            _camera.targetTexture = rt;
            _camera.Render();
            _camera.targetTexture = prev;
            RenderTexture.active = rt;

            var screenShot = new Texture2D(
                width,
                height,
                TextureFormat.RGB24,
                false);
            screenShot.ReadPixels(new Rect(0, 0, screenShot.width, screenShot.height), 0, 0);
            screenShot.Apply();

            bytes = screenShot.EncodeToPNG();
            Destroy(screenShot);
            string str = Convert.ToBase64String(bytes);
            //str= "data:image/jpg;base64," + str;
            ws.Send(str);
            //Debug.Log(str);
            yield return new WaitForSeconds(_sleep);
        }
    }
    void OnDestroy()
    {
        ws.Close();
        ws = null;
    }
}

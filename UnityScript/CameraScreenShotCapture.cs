using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 指定されたカメラの内容をキャプチャするサンプル
/// </summary>
public class CameraScreenShotCapture : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public static CameraScreenShotCapture CaptureScript;

    public byte[] bytes;
    void Start()
    {

        StartCoroutine(CaptureScreenShot(Application.dataPath + "/" + "CameraScreenShot.png"));
    }
    void Update()
    {

    }
    // カメラのスクリーンショットを保存する
    IEnumerator CaptureScreenShot(string filePath)
    {
        while(true){ 
            var rt = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, 24);
            var prev = _camera.targetTexture;
            _camera.targetTexture = rt;
            _camera.Render();
            _camera.targetTexture = prev;
            RenderTexture.active = rt;

            var screenShot = new Texture2D(
                _camera.pixelWidth,
                _camera.pixelHeight,
                TextureFormat.RGB24,
                false);
            screenShot.ReadPixels(new Rect(0, 0, screenShot.width, screenShot.height), 0, 0);
            screenShot.Apply();

            bytes = screenShot.EncodeToPNG();
            Destroy(screenShot);

            File.WriteAllBytes(filePath, bytes);
            Debug.Log("CaptureScreenShot:Done!");
            yield return new WaitForSeconds(10);
        }
    }

}

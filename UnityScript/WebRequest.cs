using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class JsonInfo
{
    public int storystateindex;

    //1->locked, 0->unlocked
    public int boxislocked;
    public int chair0;
    public int timer;
}

/*
Webサーバーから情報を取り出し
Jsonパース
*/
public class WebRequest : MonoBehaviour
{
    private string _resText;
    public JsonInfo jsonInfo = new JsonInfo();
    private string _ipaddress;
    private string _url;
    private bool _postTriger;

    //実行用フレームカウント
    private int _cntFlame;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(JsonRequest());
        //PC Debug
        //PlayerPrefs.SetString("IP", "192.168.17.76");
        //PlayerPrefs.Save();

        _ipaddress = PlayerPrefs.GetString("IP");
       
        _url = "http://" + _ipaddress + ":11000/json";
        //_url = "http://192.168.11.6:11000/json";
        _cntFlame = 0;
        _postTriger = false;
        
         StartCoroutine(PostToServer());

    }

    // Update is called once per frame
    void Update()
    {
        //60フレームに一度Jsonを取ってくる
        if(_cntFlame == 60)
        {
            StartCoroutine(JsonRequest());
            StartCoroutine(PostToServer());

            _cntFlame = 0;

        }
        else
        {
            _cntFlame++;
        }
    }

    //UnityWebRequest
    IEnumerator JsonRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(_url);
        // データをJSONで受け取りたいのでHeaderをセット
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            // エラーが起きた場合はエラー内容を表示
            Debug.Log("request.error" + request.error);
        }
        else
        {
            _resText = request.downloadHandler.text;
            jsonInfo = JsonUtility.FromJson<JsonInfo>(_resText);
            PlayerPrefs.SetInt("Chair0", (int)(jsonInfo.chair0));
            PlayerPrefs.Save();
        }
        yield break;
    }
    IEnumerator PostToServer()
    {
        int _ssi = PlayerPrefs.GetInt("ssi");
        int _bumbisdefusing = PlayerPrefs.GetInt("bumbisdefusing");
        int _boxislocked = PlayerPrefs.GetInt("boxislocked");
        WWWForm form = new WWWForm();
        form.AddField("storystateindex", _ssi);
        form.AddField("bumbisdefusing", _bumbisdefusing);
        form.AddField("boxislocked", _boxislocked);

        //UnityWebRequest www = UnityWebRequest.Post(_url, form);
        UnityWebRequest www = UnityWebRequest.Post(_url, form);
        yield return www.SendWebRequest();
        if (www.isHttpError)
        {
            // レスポンスコードを見て処理
            Debug.Log($"[Error]Response Code : {www.responseCode}");
            Debug.Log($"[Error]Response Code : {www.error}");
        }
        else if (www.isNetworkError)
        {
            // エラーメッセージを見て処理
            Debug.Log($"[Error]Message : {www.error}");
        }
        else
        {
            // 成功したときの処理
            Debug.Log($"[Success]");
        }
        yield break;

    }
}

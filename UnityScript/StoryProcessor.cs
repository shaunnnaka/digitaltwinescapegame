using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

using System.IO;

public class JsonStory
{
    public string idle_text;
    public string prologue_text;
    public string game_text;
    public string true_end_text;
    public string bad_end_text;
    public string endsearch1_text;
    public string search2_text;
    public string endsearch2_text;
    
}

public class StoryProcessor : MonoBehaviour
{
    /*ストーリー分岐制御用
     * idle-> prologue -> introduction0 -> introduction1 -> Search1 -> EndOfSearch1
     * -> Search2 -> EndOfSearch2 -> GotHMD 
     * -> Catharsis-> FinalAction -> Ending -> Ended
    */
    public Text BoardText;
    public WebRequest webRequest;
    int _StoryStateIndex;
    List<string>[]  StoryStateTexts;
    List<string> _texts = new List<string>();
    private List<string> _idle_texts = new List<string>();
    private List<string> _prologue_texts = new List<string>();
    private List<string> _game_texts = new List<string>();
    private List<string> _true_end_texts = new List<string>();
    private List<string> _bad_end_texts = new List<string>();
    private List<string> _endsearch1_texts = new List<string>();
    private List<string> _search2_texts = new List<string>();
    private List<string> _endsearch2_texts = new List<string>();
    private int _pressi = -1;
    int _currentLineNum = 0;
    int _currentCharNum = 0;
    int _textInterval = 0;

    private string _ipaddress;  
    private string _url;
    private string _resText;
    public JsonStory jsonStory = new JsonStory();
    IEnumerator LoadText(string ipaddress)
    {
        enabled = false;
        
        UnityWebRequest request = UnityWebRequest.Get(_url);
        // データをJSONで受け取りたいのでHeaderをセット
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            // エラーが起きた場合はエラー内容を表示
            Debug.Log(request.error);
            PlayerPrefs.SetString("Debug", "err get story");
            PlayerPrefs.Save();
        }
        else
        {
            // レスポンスをテキストで表示
            //Debug.Log(request.downloadHandler.text);
            _resText = request.downloadHandler.text;
            jsonStory = JsonUtility.FromJson<JsonStory>(_resText);
            char _shpn = '\n';
            _idle_texts.AddRange(jsonStory.idle_text.Split(_shpn));
            _prologue_texts.AddRange(jsonStory.prologue_text.Split(_shpn));
            _game_texts.AddRange(jsonStory.game_text.Split(_shpn));
            _true_end_texts.AddRange(jsonStory.true_end_text.Split(_shpn));
            _bad_end_texts.AddRange(jsonStory.bad_end_text.Split(_shpn));
            _endsearch1_texts.AddRange(jsonStory.endsearch1_text.Split(_shpn));
            _search2_texts.AddRange(jsonStory.search2_text.Split(_shpn));
            _endsearch2_texts.AddRange(jsonStory.endsearch2_text.Split(_shpn));

            StoryStateTexts = new List<string>[] {_idle_texts,_prologue_texts, _game_texts, _true_end_texts, _bad_end_texts, _endsearch1_texts,_search2_texts,_endsearch2_texts};
        }
        enabled = true;

        yield return new WaitForSeconds(1);
        yield break;

    }

    // Start is called before the first frame update
    void Start()
    {
        //PCデバッグ専用
        PlayerPrefs.SetString("IP","192.168.17.76");
        
        BoardText.text = "hello";

        _StoryStateIndex = 0;
        _ipaddress = PlayerPrefs.GetString("IP");
        Debug.Log("storyprocessor" + _ipaddress);
        _url = "http://" + _ipaddress + ":11000/json_story";
        StartCoroutine(LoadText(_ipaddress));
        _currentLineNum = 0;
        _currentCharNum = 0;
        //PlayerPref初期化
        PlayerPrefs.SetInt("ssi", 0);
        PlayerPrefs.SetString("Debug1", "あ");
        PlayerPrefs.SetInt("boxislocked", 1); //0->unlocked, 1->locked 
        //0 -> 解除中，1->解除, 2->失敗
        PlayerPrefs.SetInt("bombisdefusing", 0);
        PlayerPrefs.SetInt("password", 0);
        //ゲーム終了なら1
        PlayerPrefs.SetString("textinput", "start");
        PlayerPrefs.Save();
        StartCoroutine(SSIControler());
    }
    
    void DisplayText()
    {
        
        if (_textInterval == 0)
        {
            
            BoardText.text += StoryStateTexts[_StoryStateIndex][_currentLineNum][_currentCharNum];
            _currentCharNum++;
            _textInterval = 3;
        }
        else _textInterval--;
    }
    // Update is called once per frame
    void Update()
    {

        if (_pressi != PlayerPrefs.GetInt("ssi"))
        {
            _pressi = _StoryStateIndex;
            _StoryStateIndex = PlayerPrefs.GetInt("ssi");
            _currentLineNum = 0;
            _currentCharNum = 0;
            BoardText.text = "";
        }
        if (_currentCharNum < StoryStateTexts[_StoryStateIndex][_currentLineNum].Length)
        {
            DisplayText();
        }
        else
        {
            //ページ送り
            //if (OVRInput.GetDown(OVRInput.RawButton.A) && _currentLineNum < _texts.Count)
            if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
                Debug.Log("RawButton.A");
                if (_currentLineNum < StoryStateTexts[_StoryStateIndex].Count - 1)
                {
                    _currentLineNum++;
                    _currentCharNum = 0;
                    BoardText.text = "";
                }
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.B))
            {
                Debug.Log("RawButton.B");
                if (_currentLineNum > 0)
                {
                    _currentLineNum--;
                    _currentCharNum = 0;
                    BoardText.text = "";
                }
            }
            //for debug
            if (Input.GetKeyDown(KeyCode.RightArrow)&& _currentLineNum < StoryStateTexts[_StoryStateIndex].Count - 1)
            {
                _currentLineNum++;
                _currentCharNum = 0;
                BoardText.text = "";
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && _currentLineNum > 0)
            {
                _currentLineNum--;
                _currentCharNum = 0;
                BoardText.text = "";
            }
        }

    }
    IEnumerator SSIControler()
    {
        //prologue
        Debug.Log("ssictrl" + PlayerPrefs.GetInt("ssi").ToString() );
        PlayerPrefs.SetString("Debug1", "SSIControler:0");
        PlayerPrefs.Save();
        //すべてページをめくるまで待つ
        yield return new WaitUntil(() => PlayerPrefs.GetInt("ssi")==0  && (OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.RightArrow)) );
        //1
        Debug.Log("ssictrl1" + PlayerPrefs.GetInt("ssi").ToString());
        PlayerPrefs.SetString("Debug1", "SSIControler:1");
        PlayerPrefs.SetInt("ssi", 1);
        PlayerPrefs.Save();
        _currentLineNum = 0;
        _currentCharNum = 0;
        BoardText.text = "";
        //Start
        yield return new WaitUntil(() => PlayerPrefs.GetInt("ssi") == 1 && PlayerPrefs.GetString("textinput") == "start");
        //2
        PlayerPrefs.SetInt("ssi", 2);
        PlayerPrefs.SetString("Debug1", "SSIControler:2");
        PlayerPrefs.Save();
        _currentLineNum = 0;
        _currentCharNum = 0;
        BoardText.text = "";

        //ending
        yield return new WaitUntil(() => PlayerPrefs.GetInt("ssi") == 2 && (PlayerPrefs.GetInt("bombisdefusing") == 1 || PlayerPrefs.GetInt("bombisdefusing") == 2));
        //3
        //ゲームクリア
        if(PlayerPrefs.GetInt("bombisdefusing") == 1)
        {
            PlayerPrefs.SetInt("ssi", 3);
            PlayerPrefs.SetString("Debug1", "SSIControler:3");
        }
        //ゲームオーバー
        else
        {
            PlayerPrefs.SetInt("ssi", 4);
            PlayerPrefs.SetString("Debug1", "SSIControler:4");
        }
        PlayerPrefs.Save();

        _currentLineNum = 0;
        _currentCharNum = 0;
        BoardText.text = "";

    }


}

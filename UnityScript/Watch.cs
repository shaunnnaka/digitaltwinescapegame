using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Watch : MonoBehaviour
{
    public WebRequest webRequest;
    public Text WatchText;
    //story state index
    /*ストーリー分岐制御用
     * idle-> prologue -> introduction0 -> introduction1 -> Search1 -> EndOfSearch1
     * -> Search2 -> EndOfSearch2 -> GotHMD 
     * -> Catharsis-> FinalAction -> Ending -> Ended
    */
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (webRequest.jsonInfo.storystateindex)
        {
            case 0:
                WatchText.text = "プロローグ";
                break;
            case 1:
                WatchText.text = "イントロダクション";
                break;
            case 2:
                WatchText.text = "イントロダクション";
                break;
            case 3:
                WatchText.text = "探索";
                WatchText.text += "\n "+ webRequest.jsonInfo.timer.ToString();
                break;
            case 4:
                WatchText.text = "探索終了";
                break;
            default:
                WatchText.text = "default";

                break;
        }
    }
}

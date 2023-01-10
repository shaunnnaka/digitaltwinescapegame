using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteControlObj : MonoBehaviour
{
    [SerializeField] private string _PlayerPlefsKey;
    [SerializeField] private Vector3 _AfterPosition;
    private int _flameCnt;
    private Vector3 _prePosition;

    public AudioClip sound1;
    AudioSource audioSource;
    //false =prev, true =after
    private bool pre_state = false;

    // Start is called before the first frame update
    void Start()
    {
        
        _flameCnt = 0;
        _prePosition = transform.localPosition;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_flameCnt == 30)
        {
            if(1 == PlayerPrefs.GetInt(_PlayerPlefsKey))
            {
                if(pre_state == false)
                {
                    audioSource.PlayOneShot(sound1);
                    pre_state = true;
                }
                transform.localPosition = _AfterPosition;
            }
            else
            {
                if (pre_state == true)
                {
                    audioSource.PlayOneShot(sound1);
                    pre_state = false;
                }
                transform.localPosition = _prePosition;
            }
            _flameCnt = 0;
        }
        _flameCnt++;
    }
}

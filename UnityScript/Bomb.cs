using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    //Bombの最上層にアタッチ
    private int _cnt;
    //[SerializeField] private int _boxislocked;
    [SerializeField] public float timer;
    public GameObject Char_ka;
    public GameObject Char_ni;
    private int _bombisdefusing;
    public Text text;
    private bool ka;
    private bool ni;
    private int _FlameCnt;

    //audio
    [SerializeField] private AudioSource audioSource_canon;
    public AudioClip sound_canon;
    private bool is_sound_canon;
    [SerializeField] private AudioSource audioSource_clock;
    public AudioClip sound_clock;
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        //使わない
        //_boxislocked = 1;
        //var Grabbable = GetComponent<OVRGrabbable>();
        //Grabbable.enabled = false;
        _bombisdefusing = 0;
        ka = false;
        ni = false;
        _FlameCnt = 0;

        is_sound_canon = false;
        audioSource_clock.mute = true;
        audioSource_clock.loop = true;

        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //timer
        text.text = timer.ToString("F1");
        if (_bombisdefusing == 0 && PlayerPrefs.GetInt("ssi") == 2)
        {
            if (0 < timer)
            {
                timer -= Time.deltaTime;
                audioSource_clock.mute = false;
            }
            else if (timer < 0)
            {
                timer = 0.0f;
                //lose
                PlayerPrefs.SetInt("bombisdefusing", 2);
                PlayerPrefs.Save();
                audioSource_clock.mute = true;
                //音(sound1)を鳴らす
                if (is_sound_canon == false)
                {
                    audioSource_canon.PlayOneShot(sound_canon);
                    is_sound_canon = true;
                }


            }
        }
        else if (PlayerPrefs.GetInt("ssi") == 3 || PlayerPrefs.GetInt("ssi") == 4)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

/*
        if (_FlameCnt == 10)
        {


            if (ka)
            {
                Char_ka.transform.position = transform.position;
            }
            if (ni)
            {
                Char_ni.transform.position = transform.position;
            }

            _FlameCnt = 0;
        }
        else
        {
            _FlameCnt++;
        }*/

    }

    private void OnCollisionStay(Collision collision)
    {
        if (_cnt == 30)
        {//PlayerPrefs.GetString("textinput") == "438" &&
            if(collision.gameObject.name == "Chair")
            {
                if (( PlayerPrefs.GetInt("Chair0") == 1  && PlayerPrefs.GetString("textinput") == "438") || PlayerPrefs.GetString("textinput") == "999")
                {
                    _bombisdefusing = 1;
                    PlayerPrefs.SetInt("bombisdefusing", 1);
                    PlayerPrefs.Save();
                }
            }
            /*if (collision.gameObject.name == "Character_ka" && ka == false)
            {
                collision.gameObject.transform.position = transform.position;
                collision.gameObject.transform.rotation = transform.rotation;
                Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                ka = true;
            }
            if (collision.gameObject.name == "Character_ni" && ni == false)
            {
                collision.gameObject.transform.position = transform.position;
                collision.gameObject.transform.rotation = transform.rotation;
                Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                ni = true;
            }*/
        }
        else
        {
            _cnt++;
        }
    }
}

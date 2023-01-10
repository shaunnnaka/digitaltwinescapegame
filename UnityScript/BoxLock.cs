using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLock : MonoBehaviour
{
    //doorにアタッチする
    private int _cnt;
    [SerializeField] private Quaternion _AfterQuaternion;
    [SerializeField] private float x;
    [SerializeField] private float z;
    private Quaternion _preQuaternion;
    private int _flameCnt;

    public AudioClip sound1;
    AudioSource audioSource;
    private bool is_sound;
    // Start is called before the first frame update
    void Start()
    {
        _preQuaternion = transform.localRotation;
        _flameCnt = 0;
        audioSource = GetComponent<AudioSource>();
        is_sound = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_flameCnt > 15)
        {
            _flameCnt = 0;
        }
        _flameCnt++;
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.name == "Key" && _flameCnt == 10)
        {
            if(is_sound == false)
            {
                audioSource.PlayOneShot(sound1);
                is_sound = true;
            }


            transform.localRotation = _AfterQuaternion;
            PlayerPrefs.SetInt("boxislocked", 0);
            Debug.Log("LOG OPEN");
            collision.gameObject.transform.position = transform.position + new Vector3(x, 0f, z);
            collision.gameObject.transform.rotation= transform.rotation;
            collision.gameObject.transform.Rotate(0f, 90f, 0f);
            Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}

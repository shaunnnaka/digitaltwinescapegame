using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CrossWord : MonoBehaviour
{
    public GameObject AnswerChar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionStay(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if(collision.gameObject.name == AnswerChar.name)
        {
            collision.gameObject.transform.position = transform.position;
            collision.gameObject.transform.rotation = transform.rotation;
            Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        }
    }
}

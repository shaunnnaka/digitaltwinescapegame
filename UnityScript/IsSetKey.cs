using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsSetKey : MonoBehaviour
{
    public bool isSetKey;

    public GameObject KeyHall;
    private Collider col;
    // Start is called before the first frame update
    void Start()
    {
        isSetKey = false;
        col = this.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSetKey == true)
        {
            transform.position = KeyHall.transform.position;
            transform.rotation = Quaternion.Euler(0f, 90f, -90f);
            col.enabled = false;

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "KeyHall")
        {
            isSetKey = true;
        }
    }

}

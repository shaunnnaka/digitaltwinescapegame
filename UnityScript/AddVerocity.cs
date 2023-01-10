using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddVerocity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();  // rigidbodyを取得
        Vector3 force = new Vector3(Random.value *0.5f -0.25f , Random.value * 0.5f - 0.25f, Random.value * 0.5f - 0.25f);  // 力を設定
        rb.AddForce(force, ForceMode.Impulse);
    }
}

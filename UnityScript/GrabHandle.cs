using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandle : MonoBehaviour
{
    GameObject MoveDrawerObj;
    public  IsSetKey IsSetKey;
    public float StartStop;
    public float EndStop;
    public Vector3 DefaultPos;
    public Vector3 MaxPos;
    private bool IsDrawerOpend;
    // Start is called before the first frame update
    void Start()
    {
        MoveDrawerObj = transform.parent.gameObject;
        IsDrawerOpend = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSetKey.isSetKey && MoveDrawerObj.transform.localPosition.x  < 0.85f) {
            MoveDrawerObj.transform.Translate(0.02f, 0f, 0f);
            IsDrawerOpend = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        /*if(collision.gameObject.name == "CustomHandLeft" && IsSetKey.isSetKey && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger,OVRInput.Controller.LHand))
        {
            //if (MoveDrawerObj.transform.localPosition.x < StartStop)

                MoveDrawerObj.transform.position = new Vector3(collision.transform.position.x, MoveDrawerObj.transform.position.y, MoveDrawerObj.transform.position.z);

        }
        if (collision.gameObject.name == "CustomHandRight" && IsSetKey.isSetKey && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RHand))
        {

                MoveDrawerObj.transform.position = new Vector3(collision.transform.position.x, MoveDrawerObj.transform.position.y, MoveDrawerObj.transform.position.z);

        }*/
    }
}

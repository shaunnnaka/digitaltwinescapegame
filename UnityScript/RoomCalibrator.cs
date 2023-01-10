using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCalibrator : MonoBehaviour
{
    public GameObject Room;
    public GameObject Hand;
    public Vector3 Point1;
    public Vector3 Point2;
    public Vector3 Point3;
    public GameObject RoomPoint1;
    public GameObject RoomPoint2;
    public GameObject RoomPoint3;

    public float eps = 0.01f;
    public int max = 100;
    public float coef;

    //calibrationで使う
    private int i = 0;
    private float dot;
    private Vector3 temp;
    private Vector3 normPoint1;
    private string _calibLog;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if( (OVRInput.GetDown(OVRInput.RawButton.Y) || Input.GetKey(KeyCode.C)) && PlayerPrefs.GetString("textinput")=="cal" )
        {
            Debug.Log("LOG Calibration");
            StartCoroutine(Calibration());
        }

    }
    IEnumerator Calibration()
    {
        //get point1
        yield return new WaitUntil(() => (OVRInput.GetDown(OVRInput.RawButton.A) == true || Input.GetKey(KeyCode.C)));
        Point1 = Hand.transform.position;
        _calibLog = string.Format("Calibration:Point1={0:f4},{1:f4},{2:f4},\nPlease set controller to point2", Point1.x, Point1.y, Point1.z);
        Debug.Log("LOG"+_calibLog);
        PlayerPrefs.SetString("Debug2", _calibLog);
        PlayerPrefs.Save();
        yield return new WaitForSeconds(3);
        //get point2
        yield return new WaitUntil(() => (OVRInput.GetDown(OVRInput.RawButton.A) == true || Input.GetKey(KeyCode.C)));
        Point2 = Hand.transform.position;
        _calibLog = string.Format("Calibration:Point2={0:f4},{1:f4},{2:f4},\nPlease set controller to point3", Point2.x, Point2.y, Point2.z);
        Debug.Log("LOG" + _calibLog);
        PlayerPrefs.SetString("Debug2", _calibLog);
        PlayerPrefs.Save();
        yield return new WaitForSeconds(3);
        //get point3
        yield return new WaitUntil(() => (OVRInput.GetDown(OVRInput.RawButton.A) == true || Input.GetKey(KeyCode.C)));
        Point3 = Hand.transform.position;
        yield return new WaitUntil(() => (OVRInput.GetDown(OVRInput.RawButton.A) == true || Input.GetKey(KeyCode.C)));
        _calibLog = "Calibration:Start";
        Debug.Log("LOG" + _calibLog);
        PlayerPrefs.SetString("Debug2", _calibLog);
        PlayerPrefs.Save();

        //原点を変更
        Vector3 _vecRoomPoint2toPoint2 = Point2 - RoomPoint2.transform.position;
        Room.transform.position = Room.transform.position + _vecRoomPoint2toPoint2;

        //部屋の角の内積が0になるまで角度を更新
        temp = Point1 - Point2;
        temp.y = 0f;
        Vector3 normPoint1 = temp.normalized;
        temp = RoomPoint3.transform.position - Point2;
        temp.y = 0f;
        Vector3 normRPonit3 = temp.normalized;
        dot = Vector3.Dot(normPoint1, normRPonit3);

        while (Mathf.Abs(dot) > eps)
        {
            if (max < i)
            {
                _calibLog = "Calibration:Max and interapt";
                Debug.Log("LOG" + _calibLog);
                PlayerPrefs.SetString("Debug2", _calibLog);
                PlayerPrefs.Save();
                break;
            }
            //回転
            Room.transform.rotation *= Quaternion.Euler(0f, -1f * coef * dot, 0f);
            //原点を変更
            _vecRoomPoint2toPoint2 = Point2 - RoomPoint2.transform.position;
            Room.transform.position = new Vector3 (Room.transform.position.x + _vecRoomPoint2toPoint2.x , Room.transform.position.y, Room.transform.position.z + _vecRoomPoint2toPoint2.z);

            temp = RoomPoint3.transform.position - Point2;
            temp.y = 0f;
            normRPonit3 = temp.normalized;
            dot = Vector3.Dot(normPoint1, normRPonit3);
            Debug.Log("LOG" + "Calibration: i= " + i.ToString() + ": dot= " + dot.ToString());
            i++;
        }
        _calibLog = "Calibration:FIN";
        Debug.Log("LOG" + _calibLog);
        PlayerPrefs.SetString("Debug2", _calibLog);
        PlayerPrefs.Save();
        yield break;


    }

}

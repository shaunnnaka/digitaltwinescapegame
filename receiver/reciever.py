import serial
import time
import pymongo
import time
def create_mongodb_connection():  # [3]
  client = pymongo.MongoClient('localhost',27017)
  db = client['DigitalTwinEscapeGame']
  return db

#センサの閾値
eps = 0.5
ser_rp = serial.Serial("COM7", 9600, timeout = 0.1)
ser_servo = serial.Serial("COM8", 9600, timeout = 0.1)
time.sleep(2)           # Arduino側との接続のための待ち時間が必要
box_is_lock = 1
ser_servo.write(b'0')
try:
  while(True):
    db = create_mongodb_connection()
    res_json = db.PlayerState.find_one({'tag':'status'})
    tmp=ser_rp.read()  #0->prev , 1->after
    print(tmp)
    if(tmp == b'1'  and res_json["data"]["chair0"] == 0): #prev -> after
      db.PlayerState.update_one({'tag':'status'}, {'$set':{
          'data':{
            'storystateindex':res_json["data"]["storystateindex"],
            'boxislocked':res_json["data"]["boxislocked"],
            'chair0':1,
            'bombdefusing':res_json["data"]['bombdefusing'],
            'timer': res_json["data"]['timer']
          }
          }})
      print("chair: -> after")
    elif (tmp == b'0'  and res_json["data"]["chair0"] == 1): #after -> prev
      db.PlayerState.update_one({'tag':'status'}, {'$set':{
          'data':{
            'storystateindex':res_json["data"]["storystateindex"],
            'boxislocked':res_json["data"]["boxislocked"],
            'chair0':0,
            'bombdefusing':res_json["data"]['bombdefusing'],
            'timer': res_json["data"]['timer']
          }
          }})
      print("chair: -> prev")
    #解錠の瞬間
    if(res_json["data"]["boxislocked"] == 0 ):#and box_is_lock  == 1
      if(box_is_lock == 1):
        print("unlock")
      ser_servo.write(b'1')
      box_is_lock = 0
    elif (res_json["data"]["boxislocked"] == 1):#and box_is_lock  == 0
      if(box_is_lock == 0):
        print("lock")
      ser_servo.write(b'0')
      box_is_lock = 1
    
    time.sleep(0.1)
except KeyboardInterrupt:
  ser_rp.close()
  ser_servo.close()
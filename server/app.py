from flask import *
import pymongo
from PIL import Image 
import cv2
import io
import base64
import numpy as np
from gevent import pywsgi
from geventwebsocket.handler import WebSocketHandler
def create_mongodb_connection():  # [3]
  client = pymongo.MongoClient('localhost',27017)
  db = client['DigitalTwinEscapeGame']
  return db



app = Flask(__name__,  static_folder="static")  
app.config["JSON_AS_ASCII"] = False
style = "/static/style/style.css"
direct_json= "json/"
@app.route("/")  
def index():
  return render_template('index.html')

@app.route("/json", methods=['GET', 'POST']) 
#http://192.168.11.6:11000/json
def serve_json():
  db = create_mongodb_connection()
  res_json = db.PlayerState.find_one({'tag':'status'})["data"]
  # リクエストがポストかどうかの判別
  
  if request.method == 'POST':
    #update
    res_json["storystateindex"] = int(request.form["storystateindex"])
    res_json["boxislocked"] = int(request.form["boxislocked"])
    res_json["bumbisdefusing"] = int(request.form["bumbisdefusing"])
    db.PlayerState.update_one({'tag':'status'}, {'$set':{
        'data':{
          'storystateindex':res_json["storystateindex"],
          'boxislocked':res_json["boxislocked"],
          'chair0':res_json['chair0'],
          'bombdefusing':res_json['bumbisdefusing'],
          'timer': 100
        }
        }})
    
  return jsonify(res_json)
# {
#   "boxislocked": 2,
#   "storystateindex": 3,
#   'byte_img_base64':binary
#   "timer": 1
# }
#https://github.com/iruka-man/sample2/blob/master/app.py
@app.route("/json_story") 
#http://192.168.11.6:11000/json
def serve_json_story():
  # JSONファイルを読み込みます
  with open(direct_json+"story.json", "r", encoding="UTF-8") as file:
    dict_story = json.load(file)
  return jsonify(dict_story)

@app.route("/no1")
def no1answer():
  no1_model_answer = 8888
  if request.method == 'POST':
    print(request.form)
    res = int(request.form["no1ans"])
    if(res == no1_model_answer):
      redirect('pass.html', mes = "第1問クリア！" , href = "no2.html")
  return render_template('no1.html') 

@app.route("/no1")
def no2answer():
  no2_model_answer = 8888
  if request.method == 'POST':
    print(request.form)
    res = int(request.form["no2ans"])
    if(res == no2_model_answer):
      redirect('pass.html', mes = "第2問クリア！" , href = "ending.html")
  return render_template('no2.html') 

if __name__ == "__main__":
  server = pywsgi.WSGIServer(('localhost', 8000), app, handler_class=WebSocketHandler)
  app.run(debug=False, host='0.0.0.0', port=11000)

#https://qiita.com/nagataaaas/items/24e68a9c736aec31948e
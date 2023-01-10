var WebSocket = require('ws');
var ws = WebSocket.Server;
var wss = new ws({port: 8080});
// var http = require("http"); 
// var fs = require('fs');

// var server = http.createServer(function (req, res) {
//   //サーバー起動
// var imgdata;
//   var url = '.'+(req.url.endsWith("/") ? req.url + "index.html" : req.url);
//   //URLの最後が/ならindex.htmlを表示し、それ以外はそのURLのファイル名を変数へ。

//   console.log(url);
//   //htmlでリクエストされるコンテンツをコンソールに出力してます

//   if (fs.existsSync(url)) {
//     fs.readFile(url, (err, data) => {
//       if (!err) {
//         res.writeHead(200, {"Content-Type":"image/png"});   
//         res.end(imgdata, "base64");
//       }
//     });
//   }
// }).listen(8081);


console.log(wss.clients);

wss.brodcast = function (data) {
    this.clients.forEach(function each(client) {
        if (client.readyState === WebSocket.OPEN) {
            client.send(data);
        }
    });
};

wss.on('connection', function (ws) {
    ws.on('message', function (data) {
        console.log(' Received');
        //dataの5文字がタグかどうか
        tag = "data"
        //6文字目以降をブラウザで表示
        if (tag == "STORY"){

        }
        //base64ファイル
        else{ 
            //const imgElement = document.getElementById('imageViewer');
            var str ="data:image/png;base64," + data.toString("utf-8");
            
            
            //document.write('<img src="'+ str +'" alt="画像の解説文">');
            //debug 
            wss.clients.forEach(client => {
              client.send(str);
            });
        }
    });
});
wss.on('listening', ()=>{
  console.log('server is listning on port 8080');
})
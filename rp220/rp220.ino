//シリアル通信でセンサの値を送信->DBを更新→Unity に反映
//********************************************************************
//* フォトリフレクタからの入力を表示するプログラム
//********************************************************************
int pin = 0; //
int a[] = {0,0,0,0,0};
int eps = 700;
void setup() {
Serial.begin(9600) ;

}
void loop() {
  for(int i = 0; i < 5 ;i++){
    a[i] = analogRead(pin) ; //アナログ1番ピンからセンサ値を読み込み
    //Serial.print(a[i]) ; // シリアルモニターへ表示
    //Serial.print(",") ; // シリアルモニターへ表示
    delay(100) ; // 100ms待つ
  }
  //Serial.print("\n") ; // シリアルモニターへ表示

  // バブルソートのアルゴリズム
  for (int i = 0; i < 5; i++) {
    for (int j = 4; j > i; j--) {
      if (a[j] < a[j - 1]) {
        // 配列aのj番目の要素とj-1番目の要素を交換
        int temp = a[j];
        a[j] = a[j - 1];
        a[j - 1] = temp;
      }
    }
  }
  //Serial.println(a[2]) ;
  delay(500) ; 
  if(a[2] > eps){
    Serial.println(1);
  }
  else{
    Serial.println(0);
  }
}

#include <Servo.h>
Servo servo5;
int  val;
void setup() {
  // put your setup code here, to run once:
  servo5.attach(5);
  Serial.begin(9600) ;
  pinMode(13, OUTPUT);
  digitalWrite(13, HIGH);
  delay(500);
  digitalWrite(13, LOW);
  delay(500);
  digitalWrite(13, HIGH);
  delay(500);
  digitalWrite(13, LOW);
  delay(500);
}

void loop() {
  // put your main code here, to run repeatedly:
  val = Serial.read() - '0' ; // シリアルモニターへ表示
  
  if(val == 1){
    servo5.write(160);
    digitalWrite(13, HIGH);
    Serial.println(val, DEC);
  
  }
  else if(val == 0){
    servo5.write(0);
    digitalWrite(13, LOW);
    Serial.println(val, DEC);
  }
  delay(50) ;
}

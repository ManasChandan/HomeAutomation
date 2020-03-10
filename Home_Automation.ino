


//FirebaseESP8266.h must be included before ESP8266WiFi.h
#include "FirebaseESP8266.h"
#include <ESP8266WiFi.h>

#define FIREBASE_HOST "https://sensorrunner.firebaseio.com/"
#define FIREBASE_AUTH "E7QjG8A4wpwQLChu4DcHY32d6xutP7BPWmGoKlfc"
#define WIFI_SSID "SECURITY BREACH 2.0"
#define WIFI_PASSWORD "manas7890"

//Define FirebaseESP8266 data object
FirebaseData firebaseData;

FirebaseJson json;
int bulb1 = 16;
int bulb2 = 05;
int potentiometer = 04;
int a=0,b=0,c=0;

void setup()
{

  Serial.begin(115200);

  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
  Serial.print("Connecting to Wi-Fi");
  while (WiFi.status() != WL_CONNECTED)
  {
    Serial.print(".");
    delay(300);
  }
  Serial.println();
  Serial.print("Connected with IP: ");
  Serial.println(WiFi.localIP());
  Serial.println();

  Firebase.begin(FIREBASE_HOST, FIREBASE_AUTH);
  Firebase.reconnectWiFi(true);

  //Set database read timeout to 1 minute (max 15 minutes)
  Firebase.setReadTimeout(firebaseData, 1000 * 60);
  //tiny, small, medium, large and unlimited.
  //Size and its write timeout e.g. tiny (1s), small (10s), medium (30s) and large (60s).
  Firebase.setwriteSizeLimit(firebaseData, "tiny");
  pinMode(bulb1 , OUTPUT)  ;
  pinMode(bulb2  , OUTPUT) ; 
  pinMode(potentiometer , OUTPUT)  ;
}

void loop()
{
 Firebase.getInt(firebaseData,"Bulb1",a);
 Firebase.getInt(firebaseData,"Bulb2",b);
 Firebase.getInt(firebaseData,"Potentiometer",c);
 Serial.println(a) ; 
 Serial.println(b)  ;
 Serial.println(c)  ;
 if(a==0)
 {
  digitalWrite(bulb1,LOW) ; 
 } 
 else if (a==1)
 {
  digitalWrite(bulb1,HIGH) ; 
 }
 if(b==0)
 {
  digitalWrite(bulb2,LOW) ; 
 } 
 else if (b==1)
 {
  digitalWrite(bulb2,HIGH) ; 
 }
 delay(50) ; 
 analogWrite(potentiometer,c) ; 
 delay(50) ; 
}

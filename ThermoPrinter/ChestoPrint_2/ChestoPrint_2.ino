#include "SoftwareSerial.h"
#include "Adafruit_Thermal.h"
#include "chesto.h"

//thermal printer
const int printer_RX_Pin = 7;  // this is the green wire
const int printer_TX_Pin = 6;  // this is the yellow wire
Adafruit_Thermal printer(printer_RX_Pin, printer_TX_Pin);


//printer settings (see thermal library for explanations)
int heatTime = 192;
int maxChunkHeight = 255;

//print settings
int numberOfPrints = 1;
int whitespace = 3;

//helpers
boolean stringRead = 0;
String incomingByte; // a variable to read incoming serial data into
String letter;
String player;
float score, diff;
float minWage = 579.54;



void setup(){
  Serial.begin(9600);
  delay(500);  
  printer.begin(heatTime);   
  delay(600);
  printer.justify('C');
}

void loop() {
  
    serialReader();

  if (stringRead) {
    Serial.println(letter.substring(0, 3));
    if (letter.substring(0, 3).equals("/pl")) {
      player = checkString(letter.substring(3));
    }
    else if (letter.substring(0, 3).equals("/sc")) {
      String temp  = (checkString(letter.substring(3)));
      score = temp.toFloat();
    }
    delay(500);
    if (score != 0) printTicket();
    else Serial.println("no score");
    stringRead = false;
  }

}





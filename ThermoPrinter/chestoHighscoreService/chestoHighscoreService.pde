/**
 * ControlP5 Textfield
 *
 *
 * find a list of public methods available for the Textfield Controller
 * at the bottom of this sketch.
 *
 * by Andreas Schlegel, 2012
 * www.sojamo.de/libraries/controlp5
 *
 */


import controlP5.*;
ControlP5 cp5;

import processing.serial.*; 
Serial port;

String player = "";
String score = "";

void setup() {
  size(700, 400);
  frameRate(10);
  frame.setTitle("Chesto Highscore Service");

  println(Serial.list());
  port = new Serial(this, Serial.list()[9], 9600);
  port.bufferUntil('\n'); 

  PFont font = createFont("arial", 20);

  cp5 = new ControlP5(this);

  cp5.addTextfield("Player")
    .setPosition(width/2-100, 100)
      .setSize(200, 40)
        .setFont(font)
          .setFocus(true)
            .setColor(color(255, 0, 0))
              ;

  cp5.addTextfield("Highscore")
    .setPosition(width/2-100, 170)
      .setSize(200, 40)
        .setFont(createFont("arial", 20))
          .setAutoClear(false)
            ;

  cp5.addBang("print")
    .setPosition(width/2+100-80, 240)
      .setSize(80, 40)
        .getCaptionLabel().align(ControlP5.CENTER, ControlP5.CENTER)
          ;    

  textFont(font);
}

void draw() {
  background(0);
  fill(255);
}

public void print() {
  player = cp5.get(Textfield.class, "Player").getText();
  score = cp5.get(Textfield.class, "Highscore").getText();  
  cp5.get(Textfield.class, "Highscore").clear();
  cp5.get(Textfield.class, "Player").clear(); 

  port.write("/pl" + player);  
  port.write(13);   
  delay(100);
  port.write("/sc" + score);  
  port.write(13);
}

void controlEvent(ControlEvent theEvent) {
  if (theEvent.isAssignableFrom(Textfield.class)) {
    println("controlEvent: accessing a string from controller '"
      +theEvent.getName()+"': "
      +theEvent.getStringValue()
      );
  }
}


public void input(String theText) {
  // automatically receives results from controller input
  println("a textfield event for controller 'input' : "+theText);
}


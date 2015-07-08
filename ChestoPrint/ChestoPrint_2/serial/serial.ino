void serialReader(){
    int makeSerialStringPosition;
    int inByte;
    char serialReadString[100];
    const int terminatingChar = 13; //Terminate lines with CR

    inByte = Serial.read();
    makeSerialStringPosition=0;

    if (inByte > 0 && inByte != terminatingChar) { //If we see data (inByte > 0) and that data isn't a carriage return
//      lockState = true;
      delay(100); //Allow serial data time to collect (I think. All I know is it doesn't work without this.)

      while (inByte != terminatingChar && Serial.available() > 0){ // As long as EOL not found and there's more to read, keep reading
        serialReadString[makeSerialStringPosition] = inByte; // Save the data in a character array
        
        makeSerialStringPosition++; //Increment position in array
        //if (inByte > 0) Serial.println(inByte); // Debug line that prints the charcodes one per line for everything recieved over serial
        inByte = Serial.read(); // Read next byte

      }

      if (inByte == terminatingChar) //If we terminated properly
      {
        serialReadString[makeSerialStringPosition] = 0; //Null terminate the serialReadString (Overwrites last position char (terminating char) with 0
        letter = String(serialReadString);
        //      Serial.println(letter);      
        stringRead = 1;      
//        lockState = false;
      }
    } 
    
}


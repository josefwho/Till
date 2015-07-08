//check string for length and (remix)
String checkString(String incoming) {
  String outgoing;
  //include paragraph only when total length of string > 32
  if (incoming.length() > 32) {
    int firstPara = incoming.indexOf('(');
    if (firstPara > -1) {
      //      Serial.println("Klammer!");
      outgoing = incoming.substring(0, firstPara) + ("\n") + incoming.substring(firstPara);
    } 
    else outgoing = incoming;
  } 
  else outgoing = incoming;
  return outgoing;
}

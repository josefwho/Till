void printTicket() {
  printer.feed(whitespace);
  printer.printBitmap(chesto_width, chesto_height, chesto_logo);
  
  printer.doubleWidthOn();    
  printer.println("AT THE CHECKOUT");
  printer.feed(1);
  printer.doubleWidthOff();  
  printer.println("#CEGC15");
  printer.println("20150122");   
  printer.println("--------------------------------");     
  printer.feed(1);
  printer.boldOn();
  printer.println(player);  
  printer.boldOff();
    printer.feed(1);
  printer.println("daily wage:");  
  printer.print(score/21.5);
  printer.println(" EUR");
  printer.println("monthly wage:");    
  printer.print(score);
  printer.println(" EUR");  
  printer.feed(1);
  printer.println("********************************");  
  printer.feed(1);
  diff = (minWage-score);
  if (diff > 0) {
    printer.println("Working poor:");
    printer.print(diff);
    printer.println (" EUR");
    printer.println("below legal minimum wage");
  } else {
    printer.println("Congrats, you are");
    printer.print(diff*-1);
    printer.println (" EUR");
    printer.println("above legal minimum wage");
  }
   
  score = 0;
  printer.feed(1);
  printer.println("********************************");  
  printer.feed(1);
  printer.println("At the Checkout");
  printer.println("A game by");
  printer.println("@peregrinustyss");
  printer.println("@brokenrules");
  printer.feed(1);  
  printer.println("********************************");  
  printer.feed(2);
  printer.doubleWidthOn(); 
  printer.println("THANK YOU FOR");  
  printer.println("WORKING FOR"); 
  printer.println("CHESTO!");
  printer.doubleWidthOff();    
  printer.feed(2);
  printer.println("--------------------------------"); 
  printer.println("ticket serviced by");
  printer.println("hotelbanana.com");
  printer.feed(whitespace*2);
  
}

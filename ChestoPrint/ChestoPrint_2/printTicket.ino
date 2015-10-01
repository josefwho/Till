void printTicket() {
  printer.feed(whitespace);
  printer.printBitmap(chesto_width, chesto_height, chesto_logo);
  
  
  printer.doubleWidthOn();    
  printer.println("AT THE CHECKOUT");
  printer.feed(1);
  printer.doubleWidthOff();  
  printer.println("SUBOTRON Spielhimmel");
  printer.println("September 26th 2015");
  printer.println("--------------------------------");      
  printer.feed(1);
  diff = (minWage-score);
  if (diff > 0) {
    printer.boldOn();
    printer.println(player + ",");  
    printer.println(" you are working poor");
    printer.boldOff();
    printer.feed(1);
    printer.print(diff);
    printer.println (" EUR");
    printer.println("below legal minimum wage");
  } else {
    printer.boldOn();
    printer.println("Congrats " + player +",");
    printer.boldOff();
    printer.println(" you are");
    printer.print(diff*-1);
    printer.println (" EUR");
    printer.println("above legal minimum wage!");
  }
   
  score = 0;
  printer.feed(1);
  printer.println("********************************");  
  printer.feed(1);
  printer.println("CHESTO - At the Checkout");
  printer.println("A game by");
  printer.println("Josef Who & Broken Rules");
  printer.println("@peregrinustyss & @brokenrules");
  printer.feed(1);   
  printer.println("Support CHESTO");
  printer.println("with a download on itch.io:");
  printer.feed(1);  
  printer.printBitmap(qr_width, qr_height, qr_itchio);
  printer.feed(1);  
  printer.println("********************************"); 
  printer.feed(2);
  printer.doubleWidthOn(); 
  printer.println("THANK YOU FOR");  
  printer.println("WORKING FOR"); 
  printer.println("CHESTO!");
  printer.doubleWidthOff();    
  printer.feed(2);
  printer.feed(whitespace);  
  printer.feed(2);
}

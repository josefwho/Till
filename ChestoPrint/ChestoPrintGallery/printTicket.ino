void printTicket() {
  printer.feed(whitespace);
 
  
  
 
  printer.feed(1);  
  printer.printBitmap(qr_width, qr_height, qr_itchio);
  printer.feed(1);  
  printer.feed(whitespace);
 
}

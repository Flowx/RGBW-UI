/*
 * Florian Wallner 12.11.2021
 * 
 * Simple Driver to control RGBW
 * (Red Green Blue White) LEDS
 * over a serial interface.
 * 
 * Simply send 4 bytes to set each color.
 * 
 * 
 */

#define PIN_R 3
#define PIN_G 5
#define PIN_B 6
#define PIN_W 9 // White LED

void setup() {
  pinMode(PIN_R, OUTPUT);
  pinMode(PIN_G, OUTPUT);
  pinMode(PIN_B, OUTPUT);
  pinMode(PIN_W, OUTPUT);
 
  analogWrite(PIN_R, 255);
  analogWrite(PIN_G, 255);
  analogWrite(PIN_B, 255);
  analogWrite(PIN_W, 255);

  Serial.begin(115200);

  Serial.println("Initialized!");
}

void SetColor(int r, int g, int b)
{
  analogWrite(PIN_R, 255 - r);
  analogWrite(PIN_G, 255 - g);
  analogWrite(PIN_B, 255 - b);
}

void SetWhite(int w)
{
  analogWrite(PIN_W, 255 - w);
}

void loop() 
{
  if(Serial.available() > 4)
  {
    int r = Serial.read();
    int g = Serial.read();
    int b = Serial.read();
    int w = Serial.read();
    SetColor(r,g,b);
    SetWhite(w);
  }
}

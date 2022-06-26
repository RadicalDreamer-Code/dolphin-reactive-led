#include <Adafruit_NeoPixel.h>

#define PIN D1

const int BUFFER_SIZE = 4;
char buf[BUFFER_SIZE];

enum command {
    turnOff = 0,
    turnOn = 1,
    changeColor = 2,
    changeHealth = 3,
};

Adafruit_NeoPixel strip = Adafruit_NeoPixel(11, PIN, NEO_GRB + NEO_KHZ800);
int NUM_LEDS = strip.numPixels();
int LED_BRIGHTNESS = 10;

// serial
char serialInput;

void setLEDStartColor() {
  Serial.println("Set Color");
  for (int i = 0; i < NUM_LEDS; i++) {
    strip.setPixelColor(i, strip.Color(255, 255, 255));
  }
  strip.show();
}

void clearLEDs() {
  for (int i = 0; i <= NUM_LEDS; i++) {
    strip.setPixelColor(i, strip.Color(0, 0, 0));
  }
  strip.show();
}

void setHealthBar(char health) {
  //TODO: over 10 Hearts (> 40 health)
  clearLEDs();

  int leftOverHeart = health % 4;
  int barCeil = floor(health / 4) + 1;

  for (int i = 0; i <= barCeil; i++) {
      if (barCeil > i) {
        strip.setPixelColor((NUM_LEDS - (i % 10)), strip.Color(255, (i > 10) ? 255 : 0, 0));  
      } else {
        strip.setPixelColor((NUM_LEDS - (i % 10)), strip.Color((i > 10) ? 255 : 60 * leftOverHeart, (i > 10) ? 60 * leftOverHeart : 0, 0));  
      }
  }
  strip.show();
}

void setup() {
  Serial.begin(9600);
  Serial.println("Start");

  strip.begin();
  strip.setBrightness(LED_BRIGHTNESS);
  setLEDStartColor();
}
 
 
void loop() {
  if (Serial.available() > 0) {
    int rlen = Serial.readBytes(buf, BUFFER_SIZE);

    // TODO: Verify
    char commandByte = buf[0];
    char valueByte = buf[1];
    
    switch(commandByte){
      case 0:
        setHealthBar(valueByte);
        break;
  
      default:
        break;
    }

    // just for testing
    Serial.write(buf, BUFFER_SIZE);
  }
}

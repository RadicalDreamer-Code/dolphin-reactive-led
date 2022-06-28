#include <Adafruit_NeoPixel.h>

#define PIN D1

const int BUFFER_SIZE = 4;
char buf[BUFFER_SIZE];

enum Commands {
    turnOff = 0x00,
    turnOn = 0x01,
    changeColor = 0x02,
    setHealth = 0x03,
    setWindwakerMode = 0x04,
    setWindwakerBeat = 0x05,
    openDoor = 0x06
} command;

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

void midToSideClear() {
  setLEDStartColor();
  int mid = ceil(NUM_LEDS / 2) + 1;
  for (int i = 0; i < mid; i++) {
    strip.setPixelColor((NUM_LEDS - mid) - i, strip.Color(0, 0, 0));
    strip.setPixelColor((NUM_LEDS - mid) + i, strip.Color(0, 0, 0));
    strip.show();
    delay(50);
  }
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

void blinkLEDs() {
  for (int i = 0; i < NUM_LEDS; i++) {
    strip.setPixelColor(i, strip.Color(255, 255, 255));
  }
  strip.show();
  delay(100);
  // fade out
  for (int j = 0; j <= 10; j++) {
    for (int i = 0; i < NUM_LEDS; i++) {
     strip.setPixelColor(i, strip.Color(25 * j, 25 * j, 25 * j));
    }
    strip.show();
    delay(30);  
  }
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
      case setHealth:
        setHealthBar(valueByte);
        break;

      case turnOff:
        clearLEDs();
        break;

      case turnOn:
        blinkLEDs();
        break;

      case openDoor:
        midToSideClear();
        break;
  
      default:
        break;
    }

    // just for testing
    Serial.write(buf, BUFFER_SIZE);
  }
}

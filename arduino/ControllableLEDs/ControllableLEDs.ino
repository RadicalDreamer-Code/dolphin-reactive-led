#include <Adafruit_NeoPixel.h>

#define PIN D1

const int BUFFER_SIZE = 4;
char buf[BUFFER_SIZE];

enum Commands {
    TURN_OFF = 0x00,
    TURN_ON = 0x01,
    CHANGE_COLOR = 0x02,
    SET_HEALTH = 0x03,
    ANIMATION_WINDWAKER_START = 0x04,
    ANIMATION_WINDWAKER_BEAT = 0x05,
    ANIMATION_OPEN = 0x06,
    ANIMATION_OPEN_CHEST = 0x07,
    ANIMATION_SWIMMING = 0x08
} command;

Adafruit_NeoPixel strip = Adafruit_NeoPixel(11, PIN, NEO_GRB + NEO_KHZ800);
int NUM_LEDS = strip.numPixels();
int LED_BRIGHTNESS = 10;

// serial
char serialInput;

void SetLEDStartColor(int red, int green, int blue) {
  for (int i = 0; i < NUM_LEDS; i++) {
    strip.setPixelColor(i, strip.Color(red, green, blue));
  }
  strip.show();
}

void ClearLEDs() {
  for (int i = 0; i <= NUM_LEDS; i++) {
    strip.setPixelColor(i, strip.Color(0, 0, 0));
  }
  strip.show();
}

void MidToSideClear() {
  SetLEDStartColor(255, 255, 255);
  int mid = ceil(NUM_LEDS / 2) + 1;
  for (int i = 0; i < mid; i++) {
    strip.setPixelColor((NUM_LEDS - mid) - i, strip.Color(0, 0, 0));
    strip.setPixelColor((NUM_LEDS - mid) + i, strip.Color(0, 0, 0));
    strip.show();
    delay(50);
  }
}

void SetHealthBar(char health) {
  //TODO: over 10 Hearts (> 40 health)
  ClearLEDs();

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

void BlinkLEDs() {
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

void RunningLights(byte red, byte green, byte blue, int waveDelay) {
  int position = 0;
  int duration = 9;
  
  for(int j=0; j< NUM_LEDS * duration; j++)
  {
      position++; // = 0; //position + Rate;
      for(int i=0; i<NUM_LEDS; i++) {
        // sine wave, 3 offset waves make a rainbow!
        //float level = sin(i+position) * 127 + 128;
        //setPixel(i,level,0,0);
        //float level = sin(i+position) * 127 + 128;
        strip.setPixelColor(i, ((sin(i + position) * 127 + 128)/255) * red,
                   ((sin(i + position) * 127 + 128)/255) * green,
                   ((sin(i + position) * 127 + 128)/255) * blue);
      }
      
      strip.show();
      delay(waveDelay);
  }

  for (int i = 0; i < NUM_LEDS; i++) {
    strip.setPixelColor(i, strip.Color(red, green, blue));
  }
  strip.show();
}

void setPixelHeatColor (int pixel, byte temperature) {
  // Scale 'heat' down from 0-255 to 0-191
  byte t192 = round((temperature/255.0)*191);
 
  // calculate ramp up from
  byte heatramp = t192 & 0x3F; // 0..63
  heatramp <<= 2; // scale up to 0..252
 
  // figure out which third of the spectrum we're in:
  if( t192 > 0x80) {                     // hottest
    strip.setPixelColor(pixel, 255, 255, heatramp);
  } else if( t192 > 0x40 ) {             // middle
    strip.setPixelColor(pixel, 255, heatramp, 0);
  } else {                               // coolest
    strip.setPixelColor(pixel, heatramp, 0, 0);
  }
}

void setup() {
  Serial.begin(9600);
  Serial.println("Start");

  strip.begin();
  strip.setBrightness(LED_BRIGHTNESS);
  SetLEDStartColor(255, 255, 255);
}
 
 
void loop() {
  if (Serial.available() > 0) {
    int rlen = Serial.readBytes(buf, BUFFER_SIZE);

    // TODO: Verify
    char commandByte = buf[0];
    char valueByte = buf[1];
    
    switch(commandByte){
      case SET_HEALTH:
        SetHealthBar(valueByte);
        break;

      case TURN_OFF:
        ClearLEDs();
        break;

      case TURN_ON:
        BlinkLEDs();
        break;

      case ANIMATION_WINDWAKER_START:
        MidToSideClear();
        break;

      case ANIMATION_WINDWAKER_BEAT:
        BlinkLEDs();
        break;

      case ANIMATION_OPEN:
        MidToSideClear();
        break;

      case ANIMATION_OPEN_CHEST:
        RunningLights(0x00, 0xff, 0, 50);
        break;

      case ANIMATION_SWIMMING:
        SetLEDStartColor(0, 0, 255);
        break;
  
      default:
        break;
    }

    // just for testing
    Serial.write(buf, BUFFER_SIZE);
  }
}

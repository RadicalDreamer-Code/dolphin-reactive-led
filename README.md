# dolphin-reactive-led

Let your Arduino-Controlled LEDs (WS2812b) react to your Dolphin game session. This application is in an alpha state and is currently only working with the NTSC version of Windwaker. Also compatible with the randomizer mod.


# How to start?
Modify inside the .ino file the pin variable so that you have the correct pin for the data line of the LED strip and upload it to your arduino. Start the executable and load windwaker into Dolphin. Inside the application click on "Connect to Dolphin" and choose which in-game events you want your LEDs react to. Finally set under the section "Serial" the correct serial port and baudrate (which should be 9600) and click run.

# Notes
In the future I will add games which I like to play again sometime and I try to make the UI a little bit nicer. If have any suggestions feel free to contact me.
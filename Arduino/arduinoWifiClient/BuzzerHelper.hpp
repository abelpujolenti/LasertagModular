#pragma once
#include <Arduino.h>

class BuzzerHelper
{
private:
  int _pin;
  bool _state = false;
  float _buzzerKhz = 1500.f;

  //Variables for non-blocking buzzer
  bool _buzzing = false;
  unsigned long _startTime = 0;
  unsigned long _totalDuration = 0;
  unsigned int _totalToggles = 0;
  unsigned int _togglesDone = 0;
  unsigned long _toggleInterval = 0;
  unsigned long _lastToggleTime = 0;

public:
  BuzzerHelper() { }

  void Init(int pin)
  {
    _pin = pin;
    pinMode(_pin, OUTPUT);
    noTone(_pin);
  }

  void On()
  {
    _state = HIGH;
    tone(_pin, _buzzerKhz);
  }

  void Off()
  {
    _state = LOW;
    noTone(_pin);
  }

  //Non-blocking buzzer: buzzes "times" times in "duration" total ms
  void Buzz(unsigned long duration, unsigned int times)
  {
    if(times == 0 || duration == 0)
      return;

    _buzzing = true;
    _startTime = millis();
    _totalDuration = duration;
    _totalToggles = times * 2;   //ON and OFF count as individual toggles
    _togglesDone = 0;
    _toggleInterval = duration / _totalToggles;
    _lastToggleTime = millis();
  }

  void Update()
  {
    if(!_buzzing)
      return;

    unsigned long now = millis();
    if(now - _lastToggleTime >= _toggleInterval)
    {
      //Change state
      _state = !_state;
      
      if(_state == HIGH)
        tone(_pin, _buzzerKhz);
      else
        noTone(_pin);

      _lastToggleTime = now;
      _togglesDone++;

      if(_togglesDone >= _totalToggles)
      {
        _buzzing = false;
        Off();
      }
    }
  }
};
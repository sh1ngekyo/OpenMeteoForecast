# OpenMeteoForecast
Weather forecast using OpenMeteo API with telegram integration (work in progress)

[![Build](https://github.com/sh1ngekyo/OpenMeteoForecast/actions/workflows/build.yml/badge.svg)](https://github.com/sh1ngekyo/OpenMeteoForecast/actions/workflows/build.yml)
[![Tests](https://github.com/sh1ngekyo/OpenMeteoForecast/actions/workflows/tests.yml/badge.svg)](https://github.com/sh1ngekyo/OpenMeteoForecast/actions/workflows/tests.yml)

# Features
* Registration
* Getting weather information (now, today, next day, next 7 days) in image format from bot
* Could set a reminder
* The application can notify in case of bad weather (rain, snow, thunderstorm, etc.)

# Currently implemented
* [CRUD] Web api that manages general information about users (Id, Location, etc)
* Api wrapper for Open Meteo
* Service for communication of client applications with user api
* Telegram bot as one of the clients
* Image generation from HTML with wkhtmltoimage (QtWebKit)

# Left to do:
* Add logging & redis for cache
* Fix 7-days avg temperature calculation
* Replace wkhtmltoimage with something newer
* Dynamically change the language of the bot depending on the user's language
* Some refactoring 

# Examples

<details> 
  <summary><h2>General structure</h2></summary>
  <img src="https://github.com/sh1ngekyo/OpenMeteoForecast/blob/master/Docs/diagram.png">
</details>

<details> 
  <summary><h2>Registration</h2></summary>
  <img src="https://github.com/sh1ngekyo/OpenMeteoForecast/blob/master/Docs/register.gif" width=25% height=25%>
</details>

<details>  
  <summary><h2>Get weather forecast</h2></summary>
  <img src="https://github.com/sh1ngekyo/OpenMeteoForecast/blob/master/Docs/forecast.gif" width=25% height=25%>
</details>

<details>  
  <summary><h2>Setup alarms</h2></summary>
  <img src="https://github.com/sh1ngekyo/OpenMeteoForecast/blob/master/Docs/alarms.gif" width=25% height=25%>
</details>

<details>  
  <summary><h2>Turn on warnings</h2></summary>
  <img src="https://github.com/sh1ngekyo/OpenMeteoForecast/blob/master/Docs/warnings.gif" width=25% height=25%>
</details>

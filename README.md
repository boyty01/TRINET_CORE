# TRINET_CORE

## This is in early development and has limited functionality. 
## This repository is public only for educational purposes and is not intended as a distribution of functional software. 


TriNet server app. Part of the TriNet smart home controller family.

## What is trinet?
TriNet is a personal project. As an avid tech hound, I naturally love smart devices.  What I don't love is having to navigate a hundred different apps to control individual devices.
The concept of TriNet, is to amalgamate all possible communication routes with my Smart Home Devices,  Client-to-Device, Server-to-Device and ManufacturerApi-to-Device. 

![Untitled Diagram drawio](https://github.com/user-attachments/assets/c8c0d964-c927-4bd2-92cb-1a69eda27a59)

It originally came about from frustrations where controlling our smart lights was a chore. Sometimes I wouldn't have my phone, or even when I did, I had to sift through my apps to find the correct brand app, boot it up and then adjust the lighting. This has never struck me as a convient replacement to a light switch.  The features of smart bulbs are great on the surface, but I just didn't have a quick way to control them.  In an attempt to resolve this, I mounted a 7" tablet next to the light switch in our living room that served as a permanent quick access to our light controllers.  This still left the problem of sifting through different apps, which is where Trinet came in.


## What is Core
Core, at it's - well, core - is the home server for the trinet system.  It runs on ASP.NET CORE, using Entity Framework and Minimal API.

### Central database
As I add extra tablets to rooms throughout my home, it makes little sense to have to setup all of my devices onto each one. Core acts as the repository for locations, rooms and devices added by all of my controllers, so that they can sync their data from a central database. 

### Reduce network congestion 
If all trinet clients were always sending requests directly to devices on the network, there's potential for chaos and race conditions, having Core be a central hub for relaying requests from clients helps streamline the control process to a more predictable and managagable flow.

### Cloud access
Eventually, Core will be a node for WAN connections to hook into the home devices. (As it stands, Core runs purely on LAN, so you must be on the same network to connect.) It will also serve as the relay to any manufacturer web API's as and where needed.


## Modules
Supported device manufacturers each have their own module.  Modules should be made up of a main module, which receives all commands directed at the manufacturer device, and device classes for each individual device that should receive the command from the main module, and process it appropriately.

## Existing modules

### Wiz
Wiz bulbs are currently fully supported.

### Nanoleaf
Nanoleaf module is currently incomplete.


## Planned Future modules

- Ring Doorbell
- Yale smart door lock
- NEST thermostat


Trinet Client is required to interface with Core. 

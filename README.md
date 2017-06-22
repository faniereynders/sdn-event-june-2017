# The Cloud Driven Build Lamp
## SDN Event June 23rd 2017

This repo contains all the goodness from my demo at SDN Event on June 23rd 2017.

There are four parts to this repo, each within their own branch:

- [Assets](https://github.com/faniereynders/sdn-event-june-2017/tree/assets): Containing all Azure resources in the form of ARM templates as well as the build definition for the sample project
- [Webhook](https://github.com/faniereynders/sdn-event-june-2017/tree/webhook): The Azure Function deployable from source code
- [Device](https://github.com/faniereynders/sdn-event-june-2017/tree/device): The code for receiving build statusses from the IoT Hub directly on the Raspberry Pi Device and lights up an LED accordingly


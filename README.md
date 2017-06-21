# Azure Function for receiving build status webhooks
This branch contains the code for the Azure Function receiving the webhook containing a payload with the status of the build.

The status is stripped out from the payload and sent to the configured IoT Hub

> Remember to add the connection string of the destination IoT Hub in the `local.settings.json` file
## **ASP .NET Core 6 Image Server with SignalR**

## Introduction

I find it very handy to serve my images through a web application so that I can find new ways to make use of them. This is a simple project which can serve an image folder and automatically create smaller image sizes for use in other applications. It can receive (upload) and delete images on disk, and it features real-time notifications when the images are changed on the server side.

## Prerequisites

This project is written in [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0) and runs on most modern operating systems. It can be hosted directly through the [.NET command line interface](https://docs.microsoft.com/en-us/dotnet/core/tools/), in the [Visual Studio IDE](https://visualstudio.microsoft.com/) or via [Docker](https://docs.docker.com/get-started/). I like to run most of my applications through Docker together with [Docker-Compose](https://docs.docker.com/compose/install/).

## Installation and usage

At the bare minimum, you will need the [ASP.NET 6.0 runtime](https://dotnet.microsoft.com/download/dotnet/6.0) to run this application.
For Windows, the easiest alternative is to clone this repo and upen up the project in Visual Studio. You will need to set some properties yourself related to IP, port and which directory to look for the images.
- Properties/launchSettings.json => specify IP and port. The application runs by default on http only.
- appsettings.json => specify ImageFolder, MobileFolder and ThumbnailFolder.
    -   The image service will find the images located in the ImageFolder directory
    -   It will then create resized images in the MobileFolder and ThumbnailFolder specified, following the same folder structure as the original folder
    -   These three folders will be available on their own endpoints

After changing these two files, you should now be able to run the application, assuming that you have no problems with file permissions and/or the network firewall. The image service will start before the actual web application and go through the images and create resized versions. It will take some time to start up the first time you run the application depending on your CPU. The console output will log the progress.

If you are running on Mac/Linux and have Docker and Docker-Compose installed, you need to change only the docker-compose.yml file to get the app up and running.
- docker-compose.yml -> specify image, mobile and thumbnail directories for volume bindings

Build the docker image: `docker build -t imageserver-dev .` and run with docker-compose: `docker-compose up -d`

The web application has 7 endpoints.
- [HttpGet]  images => returns a list of all the image paths found by the image server
- [HttpGet]  update => this makes the image service go through all the images to update the list
- [HttpPost] thumbnail => returns an image with maximum 200 pixels in either direction. Input: one image path from the list
- [HttpPost] mobile => returns an image with maximum 1000 pixels in either direction. Input: one image path from the list
- [HttpPost] image => returns the true image in full resolution. Input: one image path from the list
- [HttpPost] upload => expects to receive an "ImageRequest" which includes a file path and a form (IFormFile)
- [HttpPost] delete => expects the path of one of the images from the list

This project also includes [SignalR](https://dotnet.microsoft.com/apps/aspnet/signalr) to notify connected clients about changes when uploading or deleting images.
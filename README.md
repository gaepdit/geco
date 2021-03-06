# Georgia Environmental Connections Online

Georgia Environmental Connections Online ("GECO") is an online service allowing public access to various Georgia Air
Protection Branch applications.

The main components are the Emission Inventory System (EIS), Permit Fees, Emission Statement, Event Registration, and
Test Notifications.

## Application

GECO is a Web Forms Application targeting the .NET Framework version 4.8.

## Prerequisites for developing

* [Visual Studio](https://www.visualstudio.com/)

* Copy the configBuilder files from the app-config repo or create new ones:
    * Copy `Web.configBuilder.Server-sample.xml` to `Web.configBuilder.Debug.xml` and add the connection string and API
      keys.
    * Before deploying, create `Web.configBuilder.Staging.xml` and `Web.configBuilder.Release.xml` files as needed.

## History

In September 2018, this repository was created by converting the previous GECO "Web Site" to a "Web Application"
Project. For various reasons, the original git repository was abandoned but is still available for reference elsewhere.

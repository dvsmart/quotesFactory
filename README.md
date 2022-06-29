# quotesFactory
Product Quotes Import and Export system

The project is an implementation of a toolset to import Product Quotes, storing and querying the market data.

The toolset has been built in .NET version 6.0 using Visual Studio IDE with C# Language.

## Whats Included in this Repository

I have implemented the below features over this quotesFactory Repository,
    
### Quotes.Import.Client which includes,

- Console Application - main Client app
- Reads the *.csv files from input location and the storage location from user's input
- Validates the input and storage location using FluentValidation
- Register and Resolves the Dependency injections using AutoFac DI Package.
- Extention classes, to read file contents and write file contents.
- ServiceModels, consists of entity objects - ProductQuote & ProductQuoteGroup (.cs files)
- Calls Quotes.Import.Service - REST API service using HTTPClientFactory 
- Pass API key from appSettings.json to Rest API using HTTPClientFactory for basic authentication
- REST API service reads the QuotesGroupMapping.csv File path location and returns ProductQuoteGroup list
- Reads the .csv files parallelly to process the files
- Group the data using the ProductQuoteGroup response from RestAPI
- Store the grouped data as JSON files(for example, Group1.json, Group2.json)

I am using QuotesGroupMapping data to store the files as json format in the above console Application, which means, I intended to group the data and save the 
file with Group name to improve reading the data instead of grouping while reading the file.

I believe this will significantly improve the performance whilst reading the files as mentioned, read is quite frequent than an Import.

### Quotes.Import.Service

- Web API Rest Service, Being called by Quotes.Import.Client application and authenticated using API Key sent from client application.
- Reads the QuotesGroupMapping file from the given location or file location path from AppSettings.Json file
- File location path given in the AppSettings.Json that benefits of changing the file location path later from different location.
- Single API to read the csv file content and deserialise into c# class Objects and returns Product Quotes Group List.
- Improvement or changes can be made here by adding different file format in which .csv is currently being used, however (.json, .xml) or any database 
  can be used to get the group mapping

### Quotes.Common.Contract

- Consists of Common entities such as Result objects that uses Generic class
- idea behind this project to hold common entities like customExceptions, etc.

### Quotes.Reader.Client

- Console Application, reads input parameters - storageDirectory, output directory, date and group.
- Validates the input parameters
- Verify if Output location exists or create one if not
- Throws and returns with error if Storage location does not exists
- Parse the input parameters -  asOfDate and group name
- Read the JSON file which are previously stored using Quotes.Import.Client app with group name and filters out the content using the asofDate value
- Serialise and Save the desired outfile as JSON file (<group_name-<asOfDate>.json) in the output directory
          
### Quotes.Import.Client.Integration.Test

- Used xUnit Framework
- End to End testing covering from import to reader functions including process the files.

## Tools

1. Visual Studio 2022

### Nuget Packages

1. Autofac (DI)
2. xUnit (Unit Testing framework)
3. Swagger UI

### Steps to Build and Run the tool

1. Build the Solution

          



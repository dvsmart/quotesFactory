# quotesFactory
Product Quotes Import and Export system

The project is an implementation of a toolset to import Product Quotes, storing and querying the market data.

The toolset has been built in .NET version 6.0 using Visual Studio IDE with C# Language.

## Whats Included in this Repository

I have implemented the below features over this quotesFactory Repository,
    
### Quotes.Import.Client which includes,

- Console Application - main Client app
- Reads the *.csv files from location and the storage location path ARGUMENTS from user's input
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

- Console Application, reads commandline ARGUMENTS parameters - storageDirectory, output directory, date and group.
- Validates the arguement values
- Verify if Output location exists or create one if not
- Throws and returns with error if Storage location does not exists
- Parse the input parameters -  asOfDate and group name
- Read the JSON file which are previously stored using Quotes.Import.Client app with group name and filters out the content using the asofDate value
- Serialise and Save the desired outfile as JSON file (<group_name-<asOfDate>.json) in the output directory
          
### Quotes.Import.Client.Integration.Test

- Used xUnit Framework
- End to End testing covering from import to reader functions including process the files.
- Contains Input & Output directory or folders for files storage
- Following test methods,
    - When_InputDirectoryHasFiles_ShouldStoreQuotesGroupFileIntoStorageLocation
    - When_InValidInputDirectoryNamePassed_ShouldNotStoreAnyFileIntoStorageLocation
    - When_Error_OnCallingQuotesImportService_ShouldNotStoreAnyFileIntoStorageLocation
- QuotesImportServiceHelper
    - GetQuotesImportServiceInstance : Register & Resolve Dependencies
    - SpinUpQuotesServiceApi : Host & Spin up the Quotes Service Web API

### Quotes.Reader.Client.Integration.Test
- xUnit Framework
- Following test methods,
    - When_StorageDirectoryHasFiles_ShouldStoreQuotesGroupResultIntoOutputDirectoryLocation
    - When_GroupNameDoesntExist_ShouldNotStoreAnyFile
    - When_GroupNameExistButNoQuotesExists_ShouldStoreAFileWithEmptyQuotes
    
**        NOTE: There are more test cases which can be covered for this implementation, Due to Time Constraint, I have covered few tests that covers the basic flow and scenarios.** 
    
## Project Folder Structure
    
![image](https://user-images.githubusercontent.com/36995044/176546812-9a51260b-6fc0-422c-919a-9561c11a5813.png)
    
## Tools

1. Visual Studio 2022

### Nuget Packages

1. Autofac (DI)
2. xUnit (Unit Testing framework)
3. Swagger UI

### Steps to Build and Run the tool

1.  Open Visual Studio IDE 2019 or 2022
2.  Clean and build the solution 
3.  Expand Services -> Quotes.Import.Service and Set as Start up project
4.  Run the above service (Swagger UI will be opened with one Action method to ensure service is up and running - https://localhost:5000/swagger)
5.  Open CommandPrompt and locate the Quotes.Import.Client.exe and pass the input location arguments and storage location as below
    
            "C:\Users\vijayk\Desktop\quotesFactory-master\quotesFactory-master\Quotes.Import.Client\bin\Debug\net6.0>Quotes.Import.Client.exe          C:\Users\vijayk\Desktop\quotesFactory-master\quotesFactory-master\SampleData\Input C:\Users\vijayk\Desktop\quotesFactory-master\quotesFactory-master\SampleData\Output"
    
6.  Verify if the output json files are stored in the output location    
7.  Open the CMD and locate Quotes.Reader.Client.exe path and pass the arguments as below,
            "C:\Users\vijayk\Desktop\quotesFactory-master\quotesFactory-master\Quotes.Reader.Client\bin\Debug\net6.0>Quotes.Reader.Client.exe           C:\Users\vijayk\Desktop\quotesFactory-master\quotesFactory-master\SampleData\Output C:\Users\vijayk\Desktop\quotesFactory-master\quotesFactory-master\SampleData\Output 30-12-2019 "Eonia" "
8.  Verify if desired output file is created in the output location as JSON format (<group-name-asOfDate.json)
    

## High Level Requirement

The following picture illustrates the high level requirement for implementing the toolset.

![image](https://user-images.githubusercontent.com/36995044/176513422-1b0ec32f-a2cd-4aae-ab99-560834039a23.png)


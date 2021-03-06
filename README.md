# Digital First Careers - Apprenticeship vacancy import
 
## Introduction
The “Find a Career” digital product (https://github.com/SkillsFundingAgency/dfc-integration-faa) showcases available apprenticeship vacancies for a given job profile (such as nurse).  The component in this repository implements the import process which loads apprenticeship vacancies against the appropriate job profile.  The import process has the following logical steps.
1.	Identify job profiles applicable for apprenticeships
2.	Search for appropriate vacancies using the “Find an Apprenticeship” API.
3.	Executes logic to determine which two vacancies to showcase against a job profile
4.	Pushes the showcased vacancies into the “Find a Career” digital product.

The import process is built in c# and the .NET framework, and runs within the Azure Functions (Microsoft’s Severless product offering).  Azure storage queues are used to message between functions within the processing pipeline.

## List of dependencies

|Item					| Purpose			|
|-----------------------|:------------------|
|Azure Function			| Modular functions for ETL process, one function trigger the other using storage queues.|
|Azure CosmosDB			| Used for audit trail |
|Azure Storage			| Used for messaging between functions |
|Find a Career	| Used for mapping the job profile with available apprenticeships and publishing the showcases to the “Find a Career” website |
|Find an Apprenticeship | API to extract available apprenticeships |

## Running Locally
To run the application locally, you need to have Azure SDK, Azure Function, DocumentDB Explorer(Can be used to see the CosmosDB data) all of them can be downloaded from Microsoft website. And for "Find my career", download and setup the product from (https://github.com/SkillsFundingAgency/dfc-digital).

Once you have cloned the public repo you need to remove the .template part from the configuration file names listed below.


| Location | Repo Filename | Rename to |
|-------|-------|-------|
| DFC.Integration.AVFeed.Function.GetAVForSoc.Console| app.config.template | app.config |
| DFC.Integration.AVFeed.Function.GetMappings.Console | app.config.template | app.config |
| DFC.Integration.AVFeed.Function.PublishVacancies.Console | app.config.template | app.config |
| DFC.Integration.AVFeed.Service.AVSoapAPI | app.config.template | app.config |
| DFC.Integration.AVFeed.Function.GetServiceHealthStatus.Console | app.config.template | app.config |
| DFC.Integration.AVFeed.Function.GetAVDetailsForProjected.Console | app.config.template | app.config |
| DFC.Integration.AVFeed.Function.DeleteOrphanedAVs.Console | app.config.template | app.config |

These files contain keys and other configuration that is particular to your local instance of the application.
You will need to set these as detailed in the configuration  sections below.

#### Azure Function
The azure functions depends on other dependencies listed below. To configure the development machine to run azure function locally use [How to use Azure Function](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local)

#### Azure CosmosDB

Cosmos DB is a database service that is globally distributed. It allows you to manage your data even if you keep them in data centers that are scattered throughout the world.
The Apprenticeship Feed is inserted in the CosmosDB for Audit Trail and Soc Mapping.

You can learn more about CosmosDB [How to use CosmosDB](https://docs.microsoft.com/en-us/azure/cosmos-db/introduction). 

In your Visual Studio 2017 you can monitor CosmosDB repository .[Exploring CosmosDB](https://azure.microsoft.com/en-gb/blog/exploring-azure-documentdb-in-visual-studio/)

|File                                       |Setting                |Example value                   |
|------------------------------------------:|----------------------:|-------------------------------:|
|DFC.Integration.AVFeed.AzureFunctions      | AVAuditCosmosDB       |AccountEndpoint=< copy from Azure ComosDB Resource>;AccountKey=< copy SAS Token generated at Azure ComosDB resource>   |

#### Azure Storage

Azure Queue storage provides cloud messaging between application components.  The functions is triggered by the Queue item.

Queue storage also supports managing asynchronous tasks and building process work flows. You can learn more about Azure storage [How to use Azure Queue](https://docs.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues)

To monitor locally you need Azure storage Explorer which can be downloaded from [Download Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/)



|File                                       |Setting                |Example value                  |
|------------------------------------------:|----------------------:|------------------------------:|
| DFC.Integration.AVFeed.AzureFunctions     | AzureWebJobsStorage   | DefaultEndpointsProtocol=https;AccountName=< your account name>;AccountKey=< get it from azure function SAS token>;EndpointSuffix=core.windows.net                   |

#### Find a Career API

The “Find a career” product provides the digital front end for citizens to self-serve information and advice on careers. The product enables users to explore options for career goals and make a confident and informed choice of one that suits them.

The product uses the Sitefinity CMS solution at is core, and uses the add-on model to extend the core Sitefinity functionality to meet the needs of citizens.

|File                                       |Setting                        |Example value                      |
|------------------------------------------:|------------------------------:|----------------------------------:|
| DFC.Integration.AVFeed.AzureFunctions     | Sitefinity.AVApiEndPoint      | http://< your domain api endpoint > |
| DFC.Integration.AVFeed.AzureFunctions     | Sitefinity.ClientId           | < can be obtained from sitefinity  >|
| DFC.Integration.AVFeed.AzureFunctions     | Sitefinity.ClientSecret       | < generate it with sitefinity >     |
| DFC.Integration.AVFeed.AzureFunctions     | Sitefinity.Username           | < sitefinity username >             |
| DFC.Integration.AVFeed.AzureFunctions     | Sitefinity.Password           | < sitefinity password >             |
| DFC.Integration.AVFeed.AzureFunctions     | Sitefinity.Scopes             | < authentication protocol> OpenId   |
| DFC.Integration.AVFeed.AzureFunctions     | Sitefinity.SocApiEndPoint     |http://< your domain api endpoint >  |
| DFC.Integration.AVFeed.AzureFunctions     | Sitefinity.SocMappingEndpoint |http://< your domain Soc Mapping Api endpoint >  |
| DFC.Integration.AVFeed.AzureFunctions     | Sitefinity.TokenEndpoint      |http://< your domain authentication endpoint >  |

#### Find an Apprenticeship API

Its a goverment API which provides you the list of all Apprenticeship Available in UK. 
https://developers.apprenticeships.sfa.bis.gov.uk/


| File |Setting |Example value |
|-|-|-|
| DFC.Integration.AVFeed.AzureFunctions     | FAA.URL |https:// < apprenticeships Api endpoint >|
| DFC.Integration.AVFeed.AzureFunctions     | FAA.SubscriptionKey      | d0b6b658469e425ca133168370960a1a |
| DFC.Integration.AVFeed.AzureFunctions     | FAA.PageSize       | 10 |
| DFC.Integration.AVFeed.AzureFunctions     | FAA.MaxPagesToTryPerMapping | 100 |

## More Information

* ### Testing Framework

    1.  **FakeItEasy**
        
        *A .Net dynamic fake framework for creating all types of fake objects, mocks, stubs etc . Please find more info on [How to use FakeItEasy](https://fakeiteasy.github.io/)* 
    2.  **xUnit**
     
        *xUnit.net is a free, open source, community-focused unit testing tool for the .NET Framework . Please find more info on [How to use xUnit](https://xunit.github.io/)*

* ### Inversion of Control Container
    1.  **Autofac**
    
        *Autofac is an IoC container for Microsoft .NET. It manages the dependencies between classes so that applications stay easy to change as they grow in size and complexity. This is achieved by treating regular .NET classes as components.Please find more info on [How to use Autofac](https://autofac.org/)*

* ### Entity To DTO Mapping Framework
    1.  **AutoMapper**
    
        *Automapper is a simple reusable component which helps you to copy data from object type to other . Please find more info on [How to use AutoMapper](http://automapper.org/)*

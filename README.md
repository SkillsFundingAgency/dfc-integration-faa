# Digital First Careers - Apprenticeship vacancy import
 
## Introduction
The “Find a Career” website (https://github.com/SkillsFundingAgency/dfc-digital) showcases available apprenticeship vacancies for a given job profile (such as nurse).  The component in this repository implements the import process which loads apprenticeship vacancies against the appropriate job profile.  The import process has the following logical steps.
1.	Identify job profiles applicable for apprenticeships
2.	Search for appropriate vacancies using the “Find an Apprenticeship” API.
3.	Executes logic to determine which two vacancies to showcase against a job profile
4.	Pushes the showcased vacancies into the “Find a Career” website.

The import process is built in c# and the .NET framework, and runs within the Azure Functions (Microsoft’s Severless product offering).  Azure storage queues are used to message between functions within the processing pipeline.

## List of dependencies in a table

|Item					| Purpose			|
|-----------------------|:------------------|
|Azure Function			| Modular functions for ETL process, one function trigger the other using storage queues.|
|Azure CosmosDB			| Used for audit trail |
|Azure Storage			| Used for messaging between functions |
|Find a Career	| Used for mapping the job profile with available apprenticeships and publishing the showcases to the “Find a Career” website |
|Find an Apprenticeship | API to extract available apprenticeships |

## Running Locally

#### Azure Function
|File                                       |Setting                |Example value                                                              |
|-------------------------------------------:|---------------------:|--------------------------------------------------------------------------:|
| DFC.Integration.AVFeed.AzureFunctions     | AzureWebJobsStorage   |DefaultEndpointsProtocol=https;AccountName=< your account name>;AccountKey=< get it from azure function SAS token>==;EndpointSuffix=core.windows.net                   |

#### Azure CosmosDB
|File                                       |Setting                |Example value                   |
|------------------------------------------:|----------------------:|-------------------------------:|
|DFC.Integration.AVFeed.AzureFunctions      | AVAuditCosmosDB       |AccountEndpoint=< copy from Azure ComosDB Resource>;AccountKey=< copy SAS Token generated at Azure ComosDB resource>==;    |

#### Azure Storage
|File                                       |Setting                |Example value                  |
|------------------------------------------:|----------------------:|------------------------------:|
| DFC.Integration.AVFeed.AzureFunctions     | AzureWebJobsStorage   | DefaultEndpointsProtocol=https;AccountName=< your account name>;AccountKey=< get it from azure function SAS token>==;EndpointSuffix=core.windows.net                   |

#### Find a Career API
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
|File                                       |Setting                    |Example value              |
|-                                          :|-                         :|-                         |
| DFC.Integration.AVFeed.AzureFunctions     | FAA.Endpoint              | https://soapapi.findapprenticeship.service.gov.uk/services/VacancyDetails/VacancyDetails51.svc|
| DFC.Integration.AVFeed.AzureFunctions     | FAA.ExternalSystemId      | xxxxxx-xxfb-xxxx-xxxe-1xxx|
| DFC.Integration.AVFeed.AzureFunctions     | FAA.MaxBufferSize         | 6553600                   |
| DFC.Integration.AVFeed.AzureFunctions     | FAA.MaxReceivedMessageSize| 6553600                   |
| DFC.Integration.AVFeed.AzureFunctions     | FAA.PublicKey              |xx-\_xxxxxx\_             |

## More Information

* ### Azure
    You need to have an Azure Account to Create Resources . Please find more info on [Microsoft Azure](https://azure.microsoft.com/en-gb/)
    
    1.  **Azure Function**
    
         *To Run it locally one needs to install Azure SDK. 
        Please find more info on [How to use Azure Function](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local)*                
    
    2.  **Azure CosmosDB**
    
        *To monitor the Azure CosmosDB repository one needs to have cloud explorer
        Please find more info on [How to use CosmosDB Explorer](https://dzone.com/articles/documentdb-in-visual-studio-with-cloud-explorer)*
    
    3.  **Azure Storage Queue**
    
        *To feed the queue with the AV Feed data which the get filtered and mapped with the SOC Code.
        Please find more info on [How to use Azure Storage Queue](https://docs.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues)*
* ### CMS
    1. **Progress Sitefinity(Licence Required)**

        *To populate the Sitefinity repository with the Mapped SOC apprenticeship feed
        Please find more info on [How to use Sitefinity](https://www.sitefinity.com/)*

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

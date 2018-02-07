## Digital First Careers – Apprenticeship vacancy import
 
Introduction / overview text. ROY
List of dependencies in a table

|Item					| Purpose			|
|-----------------------|:------------------|
|Azure Function			| Its a Timer based trigger Function. The idea behind is to schedule for AV Feed fetch and insert in the CosmosDB and Sitefinity Repository
|Azure CosmosDB			| Its been used for AVFeed Audit Trail repository																							|
|Azure Storage			| Its been used for the Azure Storage queue , as the feed data is stored in the queue as the Json data|
|SiteFinity Repository	| Its used to map the soc code with the actual feed received from the AVFeed. The soc code is mapped and the data from the AV Feed is stored against the soc code					|
|Xunit-FakeItEasy		| Its and Unit Test Framework and Unit Test Mock Tool used for this project					|
|AutoFac				| Dependency Injection Tools used for this project					|
|AutoMapper				| Entity Model to DTo mapping tool used for this project					|
|AVFeed					| The Apprenticeship feed (External API based on OData used to receive feed for Sitefinity SocMapping					|


Running Locally

Introductory text about running local…

Then a section for each dependency, stating what someone would need to do to stand up the dependency, and then config settings which would need to be updated in the repo.

#####	Azure Function 

To Run it locally one needs to install Azure SDK. 
Please find more info on https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local


|Config File	| Key	| Token	
|---------------|:------|:-------
|				|		|		

##### Azure CosmosDB 

To monitor the Azure CosmosDB repository one needs to have cloud explorer
Please find more info on https://dzone.com/articles/documentdb-in-visual-studio-with-cloud-explorer

|Config File	                                    | Key	                | Token	                    |
|-------------------------------------------------- |:----------------------|:-------
|DFC.Integration.AVFeed.AzureFunctions				|	AVAuditCosmosDB	|	\_\_avAuditCosmosDB__ |

#####  DFC.Integration.AVFeed.Function.GetAVForSoc.Console 

This is an Console based app through which the AVFeed data can be fetched and be stored as Json. This
application can be run locally and can be debugged. One can see the complete flow of Feed data coming and how
the json is getting generated which eventualy will be stored in CosmosDB and can be pushed to the Sitefinity
Repository for the Soc Mapping.


|Config File	                                    | Key	                | Token	                    |
|-------------------------------------------------- |:----------------------|:-------
|DFC.Integration.AVFeed.AzureFunctions				|	AVAuditCosmosDB	|	\_\_avAuditCosmosDB__ |

##### DFC.Integration.AVFeed.Function.GetMappings.Console 

This application gets the mapping data for every Soc Code.

|Config File	                                    | Key	                | Token	                    |
|-------------------------------------------------- |:----------------------|:-------
|DFC.Integration.AVFeed.AzureFunctions				|	AVAuditCosmosDB	|	\_\_avAuditCosmosDB__ |

#####  DFC.Integration.AVFeed.Function.PublishVacancies.Console 

This application push the data to the Sitefinity Repository for the Soc Code which is mapped , as the end result
the Sitefinity displays the Mapped Apprenticeship Soc Data on the client (nationalcareersservice.gov.uk).

|Config File	                                    | Key	                | Token	                    |
|-------------------------------------------------- |:----------------------|:-------
|DFC.Integration.AVFeed.AzureFunctions				|	AVAuditCosmosDB	|	\_\_avAuditCosmosDB__ |


Create dependency instructions / overview

Update the following configuration sections with the appropriate details

|File					| Setting			|Example value		|
|-----------------------|:------------------|-------------------|
|	< Configuration >	|					|					|
|						|					|					|

##### AV Feed

-  WCF Connected Service used 

|Config File	| Key	| Token	
|---------------|:------|:-------
|				|		|	

##### Sitefinity Repository (Azure SQL)

From the management studio you can connect to the Azure SQL to see the data for the Sitefinity Repository


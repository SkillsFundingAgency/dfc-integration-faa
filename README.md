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
|Find a Career	| Used to map the job profile with available apprenticeships |
|Find an Apprenticeship | API to extract available apprenticeships |

## Running Locally

### Azure Function
|File|Setting|Example value|
### Azure CosmosDB
|File|Setting|Example value|
### Azure Storage
|File|Setting|Example value|
### Find a Career API
|File|Setting|Example value|
### Find an Apprenticeship API
|File|Setting|Example value|

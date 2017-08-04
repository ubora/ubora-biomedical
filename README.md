# [UBORA](http://ubora-biomedical.org/) #

Documents are under `Documents` folder.

Important documents are found [here](https://www.dropbox.com/home/UBORA%20consortium/e-infrastructure/project%20managment%20architecture?preview=project+structure+proposal.doc).
*Best of luck finding the right one!*

## Starting project
You need:  
* Visual Studio 2017 (*required*)
* Docker
* Node (`LTS` is minimal, best is `Current`)

Recipe:  
1. set `docker-compose` as start-up project  
2. ensure that these steps are done in case of errors:  
  - *Hyper-V > MobyLinuxVm State on*  
  - *Switch to Linux containers*  
  - *Shared Docker Drives (project's)*

## Running UBORA on Atom *under Windows*
Navigate to `src\Ubora.Web` in Explorer and run `Run Web.ps1`. Server should be up and running on `http://localhost:5000`  

## Running Functional tests locally: ##
* You need **Visual Studio Code** installed
* You need **Docker** installed
 * Open up project in VS Code.
 * Open up the folder you have **Ubora\src** in TERMINAL in VS Code.
 * Run the tests using `docker-compose -f docker-compose.ci.functional-tests.yml up`
 If you have errors do one of the following:
 * `docker-compose -f docker-compose.ci.functional-tests.yml down` and run tests again
 * `CMD` and command `docker rm -f postgres` and run tests again
 * If nothing works Restarting the computer is always a good option
 Getting the latest changes to the test environment when you are running tests locally you need to build them locally in VS Code using these two commands:
 * `docker-compose -f src\docker-compose.ci.build.yml up` in folder **\ubora** (Repo) in VS Code
 * `docker-compose -f docker-compose.ci.functional-tests.yml build` in folder **\ubora\src** in VS Code

## User accounts
Currently, there exists a predefined `System Administrator` account:  
```
admin@agileworks.eu
ChangeMe123!
```
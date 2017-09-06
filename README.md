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
### If you have errors do one of the following:
 * `docker-compose -f docker-compose.ci.functional-tests.yml down` and run tests again
 * `CMD` and command `docker rm -f postgres` and run tests again
 * If nothing works Restarting the computer is always a good option
### Getting the latest changes to the test environment when you are running tests locally you need to build them locally in VS Code using these two commands:
 * `docker-compose -f src\docker-compose.ci.build.yml up` in folder **\ubora** (Repo) in VS Code
 * `docker-compose -f docker-compose.ci.functional-tests.yml build` in folder **\ubora\src** in VS Code
### If you have this error: 
 * `standard_init_linux.go:187: exec user process caused "no such file or directory"`
### Then do the following:
### Open git bash in Ubora repo folder and insert those three commands:
 * `dos2unix RunCIBuild.sh`
 * `dos2unix RunTests.sh`
 * `dos2unix wait-for-it.sh`

## User accounts
Currently, there exists a predefined `System Administrator` account:  
```
admin@agileworks.eu
ChangeMe123!
```

## Developing functional tests (or running functional tests locally without using Docker)
Generally speaking, running through Docker, you always create the database, you always start up the application, you always run all the tests, not to mention the re-building of Docker images which takes ages... That's a lot of time just to run tests. You can develop functional tests much faster by hosting the application separately from running tests, and only running the single test you are writing. Here's how:

**1. Set up Selenium environment with Chrome driver**

1. Download the latest Selenium Standalone Server (.jar) from http://www.seleniumhq.org/download/
2. Download the latest ChromeDriver from https://sites.google.com/a/chromium.org/chromedriver/
3. Start the selenium standalone server by `java -jar -Dwebdriver.chrome.driver=PATH_TO_CHROMEDRIVER PATH_TO_SELENIUM_STANDALONE_SERVER`. 
_Example when files are located in the same folder and the Command Prompt points to it:_ `java -jar -Dwebdriver.chrome.driver=chromedriver.exe selenium-server-standalone-3.0.1.jar`

NOTES: When you go to http://localhost:4444/ you should see text about Selenium. And more info about setting up the environment here: http://webdriver.io/guide/getstarted/install.html#Set-up-your-Selenium-environment

**2. Host the UBORA web application**

1. Start the application through Visual Studio or PowerShell script and take note on what port it's hosted.

**3. Specify port in WebDriverIO config**

1. Make sure the `baseUrl` in `wdio.conf.js` (which is located under `Ubora/src/Ubora.Web.FunctionalTests`) is set to "http://localhost:YOUR_PORT/". _Example:_ `baseUrl: 'http://localhost:32769/'`

WARNING: Don't commit this! This would break the build in TeamCity.

**4. Run the tests **

1. Open Git Bash window which points to `../Ubora.Web.FunctionalTests`. If you have the extension, you can just navigate to the folder, right-click and select "Git Bash".
2. To run all the tests: `node_modules/.bin/wdio --port 4444`
3. To run a single test: `node_modules/.bin/wdio --port 4444 --spec ./features/ApplicableRegulations.feature`
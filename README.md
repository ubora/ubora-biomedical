# [UBORA](http://ubora-biomedical.org/) #

Documents are under `Documents` folder.

## Starting project
You need:

* Docker
* Node

Simplest way to run UBORA locally (_updated 21 December 2018_):

 1. Run `./Ubora.Web/InitializeDependencies.[bat/ps1]`.
 2. Run `./Ubora.Web/RunWeb.[bat/ps1]`.
 3. That's it! Navigate to "http**s**://localhost:5001" on your browser.

NOTE: Don't forget to `git pull` latest changes! :)

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
mentor@agileworks.eu
ChangeMe123!
test@agileworks.eu
ChangeMe123!
```

## Developing functional tests (or running functional tests locally without using Docker)
Generally speaking, running through Docker, you always create the database, you always start up the application, you always run all the tests, not to mention the re-building of Docker images which takes ages... That's a lot of time just to run tests. You can develop functional tests much faster by hosting the application separately from running tests, and only running the single test you are writing. Here's how:

**1. Set up Selenium environment with Chrome driver**

1. Download the latest Selenium Standalone Server (.jar) from http://www.seleniumhq.org/download/
2. Download the latest ChromeDriver from https://sites.google.com/a/chromium.org/chromedriver/ (NOTE: Unpack the .zip)
3. Start the selenium standalone server by entering `java -jar -Dwebdriver.chrome.driver=PATH_TO_CHROMEDRIVER PATH_TO_SELENIUM_STANDALONE_SERVER` on your favourite command line tool. 
_Example when files are located in the same folder and the Command Prompt points to it:_ `java -jar -Dwebdriver.chrome.driver=chromedriver.exe selenium-server-standalone-3.0.1.jar`

NOTES: When you go to http://localhost:4444/ you should see text about Selenium. And more info about setting up the environment here: http://webdriver.io/guide/getstarted/install.html#Set-up-your-Selenium-environment

**2. Host the UBORA web application**

1. Start the application through Visual Studio or PowerShell script and take note on what port it's hosted.

**3. Specify port in WebDriverIO config**

 1. Make sure the _baseUrl_ in _wdio.conf.js_ (which is located under _Ubora/src/Ubora.Web.FunctionalTests_) is set to "http://localhost:YOUR_PORT/". _Example:_ `baseUrl: 'http://localhost:32769/'`

WARNING: Don't commit this! This would break the build in TeamCity.

**4. Run the tests **

1. Open Git Bash window which points to _../Ubora.Web.FunctionalTests_. If you have the extension, you can just navigate to the folder, right-click and select "Git Bash".
2. Do "npm install" if you haven't done so before.
3. To run all the tests: `./node_modules/.bin/wdio --port 4444`
4. To run a single test: `./node_modules/.bin/wdio --port 4444 --spec ./features/YOURSPECNAME.feature`

NOTE: If you have problems, you might need to update Node. (Find out your version by entering `node -v`.) Or you have wrong port specified under the _wdio.conf.js_ --- the application has to be running and hosted on exactly that URL.


## Information
### 1. Nexus
 
 Nexus is not available on npm.
 Used nexus 4.1.6 in 3D file viewer from https://github.com/cnr-isti-vclab/nexus

Nexus is a c++/javascript library for creation and visualization. Because 3DHOP uses the NEXUS multi-resolution format.

http://vcg.isti.cnr.it/nexus/#overview


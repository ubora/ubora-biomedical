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
Navigate to `src\Ubora.Web` in Explorer and run `Run Web.ps1`. Server should be up and running on `http://localhost:5000` in `Production` mode!

### Atom notes:
* Have a good linter
  * `.cshtml` linter is bad/evil/buggy - **do not** use it!
* Be aware that when adding files, you have to run `run` again for the server to register a new file. However, this case might be fixed soon with `dotnet watch` implementation.
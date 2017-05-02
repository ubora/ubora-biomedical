# [UBORA](http://ubora-biomedical.org/) #

Documents are under `Documents` folder.

Important documents are found [here](https://www.dropbox.com/home/UBORA%20consortium/e-infrastructure/project%20managment%20architecture?preview=project+structure+proposal.doc).
*Best of luck finding the right one!*

## Starting project
You need:
- Visual Studio 2017 (*required*)
- Docker

Recipe:
- set `docker-compose` as start-up project
- ensure that these steps are done in case of errors:
  - Hyper-V > MobyLinuxVm State on
  - Switch to Linux containers
  - Shared Docker Drives (project's)

## Running UBORA on Atom *(and potentially on other platforms!)*
Under `.atom` folder, there exists `terminal-commands.json` file which you can use with terminal integration to run tasks.
When changing branches, it is generally a good idea to do a `bower install` *(and npm install for task runners but dependencies should not change often)*.

## Tasks:
- `restore` *for restoring all NuGet and .NETCore dependencies (alias for `dotnet restore`)*
- `run` *which makes the server run in **PRODUCTION** environment, unless specified (alias for `dotnet run`)*
- `db` *which is for Docker configuration*

## Notes:
* Have a good linter
  * `.cshtml` linter is bad/evil/buggy - **do not** use it!
* Be aware that when adding files, you have to run `run` again for the server to register a new file.
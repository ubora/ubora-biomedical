FROM microsoft/aspnetcore:1.1
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
COPY ./wait-for-postgres.sh wait-for-postgres.sh
CMD /bin/bash -c "/wait-for-postgres.sh && dotnet Ubora.Web.dll"

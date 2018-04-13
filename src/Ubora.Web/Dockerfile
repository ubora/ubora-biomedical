FROM microsoft/aspnetcore:2.0.6
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Ubora.Web.dll"]
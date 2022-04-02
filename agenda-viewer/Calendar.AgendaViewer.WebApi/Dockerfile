FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "agenda-viewer/Calendar.AgendaViewer.WebApi/Calendar.AgendaViewer.WebApi.csproj"
COPY . .
WORKDIR "/src/agenda-viewer/Calendar.AgendaViewer.WebApi/"
RUN dotnet build "Calendar.AgendaViewer.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Calendar.AgendaViewer.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Calendar.AgendaViewer.WebApi.dll"]
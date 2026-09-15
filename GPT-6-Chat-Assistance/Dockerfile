FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/ChatAssistantCore.API/ChatAssistantCore.API.csproj", "src/ChatAssistantCore.API/"]
COPY ["src/ChatAssistantCore.Service/ChatAssistantCore.Service.csproj", "src/ChatAssistantCore.Service/"]
COPY ["src/ChatAssistantCore.Repository/ChatAssistantCore.Repository.csproj", "src/ChatAssistantCore.Repository/"]
COPY ["src/ChatAssistantCore.Entities/ChatAssistantCore.Entities.csproj", "src/ChatAssistantCore.Entities/"]
COPY ["src/ChatAssistantCore.DTO/ChatAssistantCore.DTO.csproj", "src/ChatAssistantCore.DTO/"]
RUN dotnet restore "src/ChatAssistantCore.API/ChatAssistantCore.API.csproj"
COPY . .
WORKDIR "/src/src/ChatAssistantCore.API"
RUN dotnet build "ChatAssistantCore.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatAssistantCore.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatAssistantCore.API.dll"]

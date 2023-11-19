FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ARG mongodb_connection_string
ENV MONGODB_CONNECTION_STRING=$mongodb_connection_string

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LoRModule/LoRModule.csproj", "LoRModule/"]
RUN dotnet restore "LoRModule/LoRModule.csproj"
COPY . .
WORKDIR "/src/LoRModule"
RUN dotnet build "LoRModule.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoRModule.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoRModule.dll"]

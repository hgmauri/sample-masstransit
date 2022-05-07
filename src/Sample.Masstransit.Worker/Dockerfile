FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Sample.Masstransit.Worker/Sample.Masstransit.Worker.csproj", "src/Sample.Masstransit.Worker/"]
RUN dotnet restore "src/Sample.Masstransit.Worker/Sample.Masstransit.Worker.csproj"

COPY . .
WORKDIR "/src/src/Sample.Masstransit.Worker"
RUN dotnet build "Sample.Masstransit.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.Masstransit.Worker.csproj" -c Release -o /app/publish

ENV TZ=America/Sao_Paulo
ENV LANG pt-BR
ENV LANGUAGE pt-BR
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.Masstransit.Worker.dll"]

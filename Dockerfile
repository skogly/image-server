FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app

RUN ls
COPY Imageserver/*.csproj ./
RUN dotnet restore

COPY Imageserver/ ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80
ENV ASPNETCORE_URLS=http://*

ENTRYPOINT ["dotnet", "Imageserver.dll"]

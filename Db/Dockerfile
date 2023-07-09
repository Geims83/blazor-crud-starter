FROM mcr.microsoft.com/dotnet/sdk:7.0 as build

WORKDIR /build
COPY . .
RUN dotnet build . -c Release -o /dacpac

FROM mcr.microsoft.com/dotnet/sdk:6.0 as pub

WORKDIR /sqlpackage

RUN dotnet tool install microsoft.sqlpackage --tool-path .

COPY --from=build /dacpac ./db

ENV DBNAME=blazorcrud
ENV DBPASS=1VerySecurePassword!
ENV DBSERVER=db

ENTRYPOINT [ "sh", "-c", "./sqlpackage /a:Publish /sf:./db/Database.dacpac /tsn:${DBSERVER} /tdn:${DBNAME} /tu:sa /tp:${DBPASS} /TargetEncryptConnection:False" ]
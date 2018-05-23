FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY TempDockerService.csproj .
RUN dotnet restore TempDockerService.csproj
COPY . .
#RUN dotnet build TempDockerService.csproj -c Release -o /app
RUN dotnet publish TempDockerService.csproj -c Release -o /app

# Creat the runtime image.
FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 443

COPY localhost.conf .
# Set password for the certificate as testPassword
# I'm using Environment Variable here to simplify the .NET Core sample.
ENV certPassword testPassword

# Use opnssl to generate a self signed certificate cert.pfx with password $env:certPassword
RUN openssl genrsa -des3 -passout pass:${certPassword} -out server.key 2048
RUN openssl rsa -passin pass:${certPassword} -in server.key -out server.key
RUN openssl req -config localhost.conf -sha256 -new -key server.key -out server.csr -subj '/CN=localhost'
RUN openssl x509 -req -sha256 -days 1825 -in server.csr -signkey server.key -out server.crt
RUN openssl pkcs12 -export -out cert.pfx -inkey server.key -in server.crt -certfile server.crt -passout pass:${certPassword}

##Install the cert utils
#RUN apt-get install libnss3-tools  
## Trust the certificate for SSL 
#RUN pk12util -d sql:$HOME/.pki/nssdb -i cert.pfx  
## Trust a self-signed server certificate
#RUN certutil -d sql:$HOME/.pki/nssdb -A -t "P,," -n 'dev cert' -i server.crt  

#RUN cat server.crt >> /etc/ssl/certs/ca-certificates.crt

RUN  cp server.crt /usr/local/share/ca-certificates/server.crt
RUN update-ca-certificates

COPY --from=build /app .
ENTRYPOINT dotnet TempDockerService.dll


# ビルドイメージ
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .

COPY TactInvoker/*.csproj ./  TactInvoker/

# copy everything else and build app

COPY TactInvoker ./TactInvoker/
RUN dotnet restore TactInvoker

WORKDIR /app/TactInvoker
RUN dotnet publish -c Release -o out

# ランタイムイメージ
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-bionic-arm32v7

# WiringPiをインストールする
RUN apt-get update
RUN apt-get install -y libi2c-dev
RUN apt-get install -y git-core
RUN apt-get install -y sudo
RUN apt-get install -y make
RUN apt-get install -y gcc
RUN git clone https://github.com/WiringPi/WiringPi.git
WORKDIR /WiringPi
RUN ./build

WORKDIR /app

COPY --from=build /app/TactInvoker/out ./

ENTRYPOINT ["dotnet", "TactInvoker.dll"]


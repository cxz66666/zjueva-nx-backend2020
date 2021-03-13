#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM registry.cn-hangzhou.aliyuncs.com/zjueva/dotnet-core-3.1:0.0.2 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM registry.cn-hangzhou.aliyuncs.com/zjueva/dotnet-sdk-3.1:0.0.1 AS build
WORKDIR /src
COPY ["2020-backend.csproj", ""]
RUN dotnet restore "./2020-backend.csproj"

COPY . .
WORKDIR "/src/."
RUN dotnet build "2020-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "2020-backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
RUN echo 'Asia/Shanghai' >/etc/timezone

ENTRYPOINT ["dotnet", "2020-backend.dll"]
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as core
FROM eeacms/haproxy:latest
COPY haproxy.cfg /etc/haproxy/haproxy.cfg
FROM openjdk:8u212-jre

WORKDIR /src/GettingData
COPY . .

ADD https://archive.apache.org/dist/spark/spark-2.4.4/spark-2.4.4-bin-hadoop2.7.tgz /tmp
RUN tar xf /tmp/spark-2.4.4-bin-hadoop2.7.tgz -C /usr/local/

ADD Conf/log4j.properties /usr/local/spark-2.4.4-bin-hadoop2.7/conf/

COPY --from=core /usr/share/dotnet /usr/share/dotnet

ENV \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

ENV SPARK_HOME=/usr/local/spark-2.4.4-bin-hadoop2.7
ENV PATH=:${PATH}:${SPARK_HOME}/bin:/usr/share/dotnet

RUN dotnet --info

CMD ["haproxy","-f","/etc/haproxy/haproxy.cfg","-p","/run/haproxy.pid"]

EXPOSE 80
FROM ubuntu:16.04
RUN apt-get update && apt-get install -y varnish
COPY ./default.vcl /etc/varnish/default.vcl
CMD varnishd -F -f /etc/varnish/default.vcl

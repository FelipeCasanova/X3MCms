FROM mono:5.18.1.3

ENV APP_WORKDIR=/app

# Create workDir
RUN mkdir -p $APP_WORKDIR

# Put Init File into image
ADD src/Web/WebSPA_AngularJS/init.sh /usr/local/bin/init.sh
ADD src/Web/WebSPA_AngularJS/start-nginx.sh /usr/local/bin/start-nginx.sh

# Get package 
RUN chmod a+x /usr/local/bin/init.sh \
    && chmod a+x /usr/local/bin/start-nginx.sh \
    && apt-get update \
    && apt-get install ca-certificates wget mc git procps nginx mono-fastcgi-server openssh-server\
                       mc git less nano \
                       # autoconf libtool make \
                       -y --no-install-recommends \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/* /var/tmp/* /tmp/* \
    && mkdir -p /etc/mono/registry /etc/mono/registry/LocalMachine \
    && wget --ca-directory=/etc/ssl/certs -O /usr/local/bin/dumb-init https://github.com/Yelp/dumb-init/releases/download/v1.0.0/dumb-init_1.0.0_amd64 \
    && chmod +x /usr/local/bin/dumb-init

#ENV PKG_CONFIG_PATH /usr/lib/pkgconfig


# Put nginx default
ADD src/Web/WebSPA_AngularJS/config/default /etc/nginx/sites-available/
# Put Web
ADD src/Web/WebSPA_AngularJS/WebSPA_AngularJS $APP_WORKDIR

# Define fastcgi_params
RUN sed -i '$a fastcgi_param  PATH_INFO   \"\";' /etc/nginx/fastcgi_params
RUN sed -i '$a fastcgi_param  SCRIPT_FILENAME    $document_root$fastcgi_script_name;' /etc/nginx/fastcgi_params

# Define using port 
EXPOSE 80

# change to work directory
WORKDIR $APP_WORKDIR

# Define Volume
VOLUME $APP_WORKDIR

ENTRYPOINT ["/usr/local/bin/init.sh"]
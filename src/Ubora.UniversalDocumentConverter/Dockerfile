FROM node:8.2.1

# Create app directory
RUN mkdir -p /usr/src/app
COPY . /usr/src/app
WORKDIR /usr/src/app

# Failed to fetch http://deb.debian.org/debian/dists/jessie-updates/InRelease  Unable to find expected entry 'main/binary-amd64/Packages' in Release file 
RUN sed -i '/jessie-updates/d' /etc/apt/sources.list

# Install packages
RUN apt-get update --fix-missing \
  && wget https://github.com/jgm/pandoc/releases/download/2.7.2/pandoc-2.7.2-1-amd64.deb \
  && dpkg -i pandoc-2.7.2-1-amd64.deb \
  && apt-get clean \
  && rm -rf /var/lib/apt/lists/* \
  && npm install

EXPOSE 1337

CMD ["npm", "start"]
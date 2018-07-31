FROM node:8.2.1

# Create app directory
RUN mkdir -p /usr/src/app
COPY . /usr/src/app
WORKDIR /usr/src/app

# Install packages
RUN apt-get update --fix-missing \
  && wget https://github.com/jgm/pandoc/releases/download/2.2.2.1/pandoc-2.2.2.1-1-amd64.deb \
  && dpkg -i pandoc-2.2.2.1-1-amd64.deb \
  && apt-get clean \
  && rm -rf /var/lib/apt/lists/* \
  && npm install

EXPOSE 1337

CMD ["npm", "start"]
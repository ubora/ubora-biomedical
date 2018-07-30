FROM node:8.2.1

# Create app directory
RUN mkdir -p /usr/src/app
COPY . /usr/src/app
WORKDIR /usr/src/app

# Install packages
RUN apt-get update --fix-missing \
  && apt-get install -y pandoc \
  && apt-get clean \
  && rm -rf /var/lib/apt/lists/* \
  && npm install

EXPOSE 1337

CMD ["npm", "start"]
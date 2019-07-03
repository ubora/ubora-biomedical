const express = require('express')
const timeout = require('connect-timeout')
const fs = require('fs');
const crypto = require('crypto');
const path = require('path')
const pdc = require('pdc')
const helpers = require('./helpers')

const PORT = process.env.PORT || 1337;
const KEY = process.env['CONVERTER_API_KEY'];

const app = express()

const timeoutMilliseconds = 1 * 60 * 1000;
app.use(timeout(timeoutMilliseconds));

app.get('/', function(req, res) {
    res.send('Pandoc service. Example: curl -H "Content-Type: text/markdown" -X POST -d "# hello" http://localhost/output/html')
});

app.post('/output/:format', function(req, res) {
    if (req.headers['privateapikey'] !== KEY) {
        res.status(403).send("Invalid key or no key provided");
    
        return;
    }

    const contentType = req.get('Content-Type');
    if (!contentType) { 
        res.status(400).send("Missed content type");

        return;
    }

    const from = contentType.split("/")[1];
    const to = req.params.format;
  
    helpers.getBody(req, function(body) {
      pdc(body, from, to, function(err, result) {
        if (err) res.sendStatus(400)
          else res.append('Content-Type', 'text/' + to).send(result);
      });
    });

  });

app.post('/download/docx', function(req, res) {
    if (req.headers['privateapikey'] !== KEY) {
        res.status(403).send("Invalid key or no key provided");
    
        return;
    }

    helpers.getBody(req, function(body) {
        const random = crypto.randomBytes(8).toString('hex');
        const fileName = random + '.docx';

        pdc(body, "html", "docx", [ '-o', fileName, '--reference-doc=custom-reference.docx', '--toc' ], function(err) {
            const absolutePath = path.join(__dirname, fileName);
            if (err) {
                fs.unlink(absolutePath);
                res.status(404).send(err.message)
                return
            }

            res.download(absolutePath, fileName, function(err){
                if (err) {
                    fs.unlink(absolutePath);
                    res.status(404).send(err.message)
                    return
                }

                fs.unlink(absolutePath);
            });
        });
    }); 
});

const server = app.listen(PORT)
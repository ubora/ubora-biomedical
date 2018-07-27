const express = require('express')
const timeout = require('connect-timeout')
const fs = require('fs');
const crypto = require('crypto');
const path = require('path')
const pdc = require('pdc')
const helpers = require('./helpers')

const PORT = 5001;

const app = express()

var timeoutMilliseconds = 1 * 60 * 1000;
app.use(timeout(timeoutMilliseconds));

app.get('/', function(req, res) {
    res.send('Pandoc service. Example: curl -H "Content-Type: text/markdown" -X POST -d "# hello" http://localhost/output/html')
});

app.post('/output/:format', function(req, res) {
    var contentType = req.get('Content-Type');
    
    if (contentType) {
      var from = contentType.split("/")[1];
      var to = req.params.format;
    
      helpers.getBody(req, function(body) {
        pdc(body, from, to, function(err, result) {
          if (err) res.sendStatus(400)
            else res.append('Content-Type', 'text/' + to).send(result);
        });
      });  
    } else res.sendStatus(400) 
  });

app.post('/download/docx', function(req, res) {
    helpers.getBody(req, function(body) {
        const random = crypto.randomBytes(8).toString('hex');
        const fileName = random + '.docx';

        pdc(body, "html", "docx", [ '-o', fileName, '--reference-doc=custom-reference.docx', '--toc' ], function(err) {
          if (err){
            res.sendStatus(400)  
          } else {
            const absolutePath = path.join(__dirname, fileName);
            res.download(absolutePath, fileName, function(err){
                if (err) {
                  throw err;
                }

                fs.unlink(absolutePath);
            });
          }
        });
    }); 
});

var server = app.listen(PORT)
'use strict';

var path = require('path');

module.exports = function(app) {
  app.get('/*', function(req, res, next) {
    if(req.url.indexOf('assets') == 1 || req.url.indexOf('api') == 1 || req.url.indexOf('js') == 1 || req.url.indexOf('css') == 1) {
      next();
    } else {
      res.sendFile(path.resolve(process.cwd(), 'client/index.html'));
    }
  });
};

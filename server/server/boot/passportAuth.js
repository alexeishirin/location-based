var PassportConfigurator = require('loopback-component-passport').PassportConfigurator;

module.exports = function (app) {
  var passportConfigurator = new PassportConfigurator(app);

  var config = {};
  try {
    config = require('../../providers.json');
  } catch (err){
    console.log("No config for 3rd party login");
    process.exit(1);
  }
  
  passportConfigurator.init();

  passportConfigurator.setupModels({
    userModel: app.models.account,
    userIdentityModel: app.models.userIdentity,
    userCredentialModel: app.models.userCredential
  });

  for(var s in config){
    var c = config[s];
    c.session = c.session !== false;
    passportConfigurator.configureProvider(s, c);
  }
  
};

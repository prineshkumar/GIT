"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var Observable_1 = require("rxjs/Observable");
require("rxjs/add/operator/do");
require("rxjs/add/operator/catch");
require("rxjs/add/operator/map");
require("rxjs/add/observable/throw");
var LoginService = (function () {
    function LoginService(_http) {
        this._http = _http;
        this._loginUrl = 'http://localhost:6271/api/login/GetUser';
    }
    LoginService.prototype.getUser = function (userName, password) {
        var bodyString = JSON.stringify({ UserName: userName, Password: password }); // Stringify payload
        var headers = new http_1.Headers({ 'Content-Type': 'application/json' }); // ... Set content type to JSON
        var options = new http_1.RequestOptions({ headers: headers }); // Create a request option
        return this._http.post(this._loginUrl, bodyString, options)
            .map(function (response) { return response.json(); })["do"](function (data) { return JSON.stringify(data); })["catch"](this.handleError);
    };
    LoginService.prototype.handleError = function (error) {
        // in a real world app, we may send the server to some remote logging infrastructure
        // instead of just logging it to the console
        console.error(error);
        return Observable_1.Observable["throw"](error.json().error || 'Server error');
    };
    return LoginService;
}());
LoginService = __decorate([
    core_1.Injectable()
], LoginService);
exports.LoginService = LoginService;

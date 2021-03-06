﻿module.exports = function (callback, deltaString) {
    var sanitizeHtml = require('sanitize-html');

    // Sanitize from '<' to '>' so not to HTML escape Quill's Delta JSON.

    var sanitizedDeltaString = deltaString.replace(/<(.*?)>/g, function (val) {
        return sanitizeHtml(val, {
            allowedTags: [],
            allowedAttributes: []
        });
    });

    callback(null, sanitizedDeltaString);
};
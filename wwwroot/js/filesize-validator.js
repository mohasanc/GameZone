$.validator.addMethod('filesize', function (value, element, param) {
    return isValid = element.optional(element) || element.files[0].size <= param;
});
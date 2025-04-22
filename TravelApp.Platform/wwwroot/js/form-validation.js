$(document).ready(function () {
    var form = $("form");
    var submitButton = $("input[type=submit]");
    var validationSummary = $(".alert-danger");

    $.validator.setDefaults({
        ignore: [],
        errorClass: "is-invalid",
        validClass: "is-valid",
        errorPlacement: function (error, element) {
            error.appendTo(element.next(".text-danger"));
        },
        onfocusout: false,
        onkeyup: false,
        onclick: false
    });

    form.removeData("validator");
    form.removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse(form);

    $(".text-danger").hide();
    submitButton.prop("disabled", false);

    var fieldInteractions = {};

    $("input").on("focus change", function () {
        var fieldName = $(this).attr("name");

        if (!fieldInteractions[fieldName]) {
            fieldInteractions[fieldName] = true;

            $(this).on("blur change", function () {
                $(this).valid();
                checkFormValidity();
            });
        }
    });

    function checkFormValidity() {
        var formValid = true;

        $.each(fieldInteractions, function (fieldName, interacted) {
            if (interacted) {
                var field = $("[name='" + fieldName + "']");
                if (!field.valid()) {
                    formValid = false;
                }
            }
        });

        submitButton.prop("disabled", !formValid);
    }

    form.on("submit", function (e) {
        var validator = form.validate();
        validator.form();

        if (!form.valid()) {
            validationSummary.show();
            e.preventDefault();
        }
    });
});
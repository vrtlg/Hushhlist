// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//HostEvent
//create
function ShowHideDateInput(Status) {
    if ($(Status).val() == "Upcoming") {
        $(".DateInput").prop("disabled", false).prop("hidden", false);
    }
    else{
        $(".DateInput").prop("disabled", true).prop("hidden", true);
    }
}
function openFormEdit() {

    window.localStorage.formEdit = "true";

    //var edit = window.localStorage.getItem("formEdit");
    //if (edit == undefined) {
    //    window.localStorage.setItem("formEdit", flag);
    //}
}

function closeFormEdit() {
    window.localStorage.formEdit = "false";
}

function isFormEdit() {
    return "true" == window.localStorage.formEdit;
}
$(document).ready(function () {
    var dialog, form,
        placeName = $("#placeName"),
        address = $("#address"),
        allFields = $([]).add(placeName).add(address),
        tips = $(".validateTips");

    function updateTips(t) {
        tips.text(t).addClass("ui-state-highlight");
        setTimeout(function () {
            tips.removeClass("ui-state-highlight", 1500);
        }, 500);
    }

    function checkRegexp(o, regexp, n) {
        if (!(regexp.test(o.val()))) {
            o.addClass("ui-state-error");
            updateTips(n);
            return false;
        }
        return true;
    }

    function addPlace() {
        var valid = true;
        allFields.removeClass("ui-state-error");

        valid = valid && checkRegexp(placeName, /^\S*[AĂÂÁẮẤÀẰẦẢẲẨÃẴẪẠẶẬĐEÊÉẾÈỀẺỂẼỄẸỆIÍÌỈĨỊOÔƠÓỐỚÒỒỜỎỔỞÕỖỠỌỘỢUƯÚỨÙỪỦỬŨỮỤỰYÝỲỶỸỴAĂÂÁẮẤÀẰẦẢẲẨÃẴẪẠẶẬĐEÊÉẾÈỀẺỂẼỄẸỆIÍÌỈĨỊOÔƠÓỐỚÒỒỜỎỔỞÕỖỠỌỘỢUƯÚỨÙỪỦỬŨỮỤỰYÝỲỶỸỴAĂÂÁẮẤÀẰẦẢẲẨÃẴẪẠẶẬĐEÊÉẾÈỀẺỂẼỄẸỆIÍÌỈĨỊOÔƠÓỐỚÒỒỜỎỔỞÕỖỠỌỘỢUƯÚỨÙỪỦỬŨỮỤỰYÝỲỶỸỴAĂÂÁẮẤÀẰẦẢẲẨÃẴẪẠẶẬĐEÊÉẾÈỀẺỂẼỄẸỆIÍÌỈĨỊOÔƠÓỐỚÒỒỜỎỔỞÕỖỠỌỘỢUƯÚỨÙỪỦỬŨỮỤỰYÝỲỶỸỴAĂÂÁẮẤÀẰẦẢẲẨÃẴẪẠẶẬĐEÊÉẾÈỀẺỂẼỄẸỆIÍÌỈĨỊOÔƠÓỐỚÒỒỜỎỔỞÕỖỠỌỘỢUƯÚỨÙỪỦỬŨỮỤỰYÝỲỶỸỴAĂÂÁẮẤÀẰẦẢẲẨÃẴẪẠẶẬĐEÊÉẾÈỀẺỂẼỄẸỆIÍÌỈĨỊOÔƠÓỐỚÒỒỜỎỔỞÕỖỠỌỘỢUƯÚỨÙỪỦỬŨỮỤỰYÝỲỶỸỴA-Z]+\S*/i, "Place names can include a-z, 0-9, spaces and must start with a letter.");

        if (valid) {
            $.ajax({
                url: "/Places/create",
                method: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    placeName: placeName.val(),
                    address: address.val(),
                }),
                success: res => {
                    if (res.msg == "successed") {
                        dialog.dialog("close");
                        window.location.href = res.url;
                    }
                    else {
                        placeName.addClass("ui-state-error");
                        updateTips("Location already exists");
                    }
                }
            });
        }
        return valid;
    }

    dialog = $("#dialog-form").dialog({
        autoOpen: false,
        height: 400,
        width: 400,
        modal: true,
        buttons: {
            CreatePlace: {
                class: "CreateButton btn btn-primary",
                text: "Create",
                click: addPlace
            },
            Cancel: {
                class: "CancelButton btn btn-primary",
                text: "Cancel",
                click: () => {
                    tips.text("Fill in the required fields.");
                    dialog.dialog("close");
                }
            }
        },
        close: function () {
            form[0].reset();
            tips.text("Fill in the required fields.");
            allFields.removeClass("ui-state-error");
        }
    });

    form = dialog.find("form").on("submit", function (event) {
        event.preventDefault();
        addPlace();
    });

    $("#create-place").button().on("click", function () {
        dialog.dialog("open");
    });
});

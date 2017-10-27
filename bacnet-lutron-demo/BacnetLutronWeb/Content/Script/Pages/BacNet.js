
var deviceId = '1761035';
$(document).ready(function () {

    $("#loader").removeClass('displaynone');
    //StartBacknetProtocol();
    RangeSlider();
    BindDeviceDetai();

    $('#ddllightScene').on('change', function () {

        var selectedValue = $('#ddllightScene').val();
        if (selectedValue != 0) {
            $("#loader").removeClass('displaynone');
            SetLightingScene(selectedValue);
        }
    });
});

//Start backnet service.
function StartBacknetProtocol() {
    $.post("api/BacNet/StartBackNetProtocol", function () {

    }).success(function () {

    });
}

function BindDeviceDetai() {
    $("#loader").addClass('displaynone');
    var selectedValue = $('#exampleFormControlSelect1').val();
    if (selectedValue != 0) {
        GetDeviceDetails(selectedValue);
    }
}

$('#exampleFormControlSelect1').on('change', function () {

    var selectedValue = $('#exampleFormControlSelect1').val();
    if (selectedValue != 0) {
        GetDeviceDetails(selectedValue);
    }
});

function GetDeviceDetails(selectedValue) {
    //var deviceId = '1761035';
    $("#loader").removeClass('displaynone');
    $.get("api/BacNet/GetDeviceDetails/" + deviceId, function (deviceDetail) {
        var detail = deviceDetail;
    }).success(function (detail) {
        SetLightStatus(detail);
    });
}



$('#floorSlidar1').on('change', function () {
    var selectedValue = this.value;
    SetLightingLevel(selectedValue, deviceId);
});

$('#btnRefresh').on('click', function () {
    var selectedValue = $('#exampleFormControlSelect1').val();
    if (selectedValue != 0)
        GetDeviceDetails(selectedValue);
});

function SetLightingScene(selectedValue) {

    var lightscene = {
        DeviceID: deviceId,
        LightScene: selectedValue
    }
    $.post("api/BacNet/SetLightingScene/", lightscene, function (deviceDetail) {
        var detail = deviceDetail;
    }).success(function (detail) {
        SetLightStatus(detail);
    });
}

function SetLightingLevel(selectedValue, deviceId) {

    var lightLevel = {
        DeviceID: deviceId,
        LightLevel: selectedValue
    }
    $.post("api/BacNet/SetLightingLevel/", lightLevel, function (deviceDetail) {
        var detail = deviceDetail;
    }).success(function (detail) {
        SetLightStatus(detail);
    });
}

function SetSlidarValue(cLightLevel, deviceId) {

    $("#lightLevel").css({ "color": "hsl(43," + parseInt(cLightLevel) + "%," + "57%)" })



    $('input[type="range"]').val(cLightLevel);
    $(".range-slider__value").html(cLightLevel);
    $("#loader").addClass('displaynone');
}

function SetLightStatus(deviceDetail) {
    $('#ddllightScene').val(deviceDetail.LightScene);
    document.getElementById("ddllightScene").value = deviceDetail.LightSceneValue;
    SetSlidarValue(deviceDetail.LightLevel, deviceDetail.DeviceID);
}


var RangeSlider = function () {
    var slider = $('.range-slider'),
        range = $('.range-slider__range'),
        value = $('.range-slider__value');

    slider.each(function () {

        value.each(function () {
            var value = $(this).prev().attr('value');
            $(this).html(value);

        });

        range.on('input', function () {

            $(this).next(value).html(this.value);
            $("#lightLevel").css({ "color": "hsl(43," + this.value + "%," + "57%)" })
        });

        range.on("change", function (event, ui) {
            $("#loader").removeClass('displaynone');

            var selectedValue = this.value;
            SetLightingLevel(selectedValue, deviceId);


        });
    });
};
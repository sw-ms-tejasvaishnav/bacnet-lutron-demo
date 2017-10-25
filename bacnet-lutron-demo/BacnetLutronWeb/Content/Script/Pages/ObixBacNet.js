
var deviceId = '1761035';
$(document).ready(function () {
    debugger;
    StartBacknetProtocol();
});

//Start backnet service.
function StartBacknetProtocol() {
    $.post("api/ObixBacNet/StartBackNetProtocol", function () {

    }).success(function () {
        
    });
}

$('#exampleFormControlSelect1').on('change', function () {

    var selectedValue = $('#exampleFormControlSelect1').val();
    if (selectedValue != 0) {
        GetDeviceDetails(selectedValue);
    }
});

function GetDeviceDetails(selectedValue) {
    //var deviceId = '1761035';
    $.get("api/ObixBacNet/GetDeviceDetails/" + deviceId, function (deviceDetail) {
        var detail = deviceDetail;
    }).success(function (detail) {
        SetLightStatus(detail);
    });
}

$('#ddllightScene').on('change', function () {

    var selectedValue = $('#ddllightScene').val();
    if (selectedValue != 0) {
        SetLightingScene(selectedValue);
    }
});

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
    //  var deviceId = '1761035';
    alert(selectedValue);
    var lightscene = {
        DeviceID: deviceId,
        LightScene: selectedValue
    }
    $.post("api/ObixBacNet/SetLightingScene/", lightscene, function (deviceDetail) {
        var detail = deviceDetail;
    }).success(function (detail) {
        SetLightStatus(detail);
    });
}

function SetLightingLevel(selectedValue, deviceId) {
    //  var deviceId = '1761035';
    //   alert(selectedValue);
    var lightLevel = {
        DeviceID: deviceId,
        LightLevel: selectedValue
    }
    $.post("api/ObixBacNet/SetLightingLevel/", lightLevel, function (deviceDetail) {
        var detail = deviceDetail;
    }).success(function (detail) {
        SetLightStatus(detail);
    });
}

function SetSlidarValue(cLightLevel, deviceId) {
    if (cLightLevel != 0) {
        $(".basecolor" + deviceId).css({ "background-color": "hsl(0," + cLightLevel + "%," + "88%)" })
    }
    else {
        var cFloor = 1;
        for (var i = 0; i < totalFloor.length; i++) {
            $(".basecolor" + cFloor).css({ "background-color": "lightgray" })
            cFloor++;
        }

    }
    $('input[type="range"]').val(cLightLevel).change();
    $(".range-slider__value").html(cLightLevel);
}

function SetLightStatus(deviceDetail) {
    $('#ddllightScene').val(deviceDetail.LightScene);
    // document.getElementById("ddllightScene").value = deviceDetail.LightScene;
    SetSlidarValue(deviceDetail.LightLevel, deviceDetail.DeviceID);
}
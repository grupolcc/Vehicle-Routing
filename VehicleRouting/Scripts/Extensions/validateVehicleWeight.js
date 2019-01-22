validateVehicleWeights = function (vehiclesAndWeights) {

    $(document).ready(function () {
        $('#postForm').submit(function (e) {

            resetWeight(vehiclesAndWeights);

            calculateWeight(vehiclesAndWeights);

            if (isWeightExceeded(vehiclesAndWeights)) {

                printExceededWeightMessage(vehiclesAndWeights);

                e.preventDefault();

                return false;
            }

            return true;
        });
    });
}

resetWeight = function (vehiclesAndWeights) {

    vehiclesAndWeights.forEach(function (v) {
        v.packWeight = 0;
    });
}

calculateWeight = function (vehiclesAndWeights) {

    $(".packsAndVehicles").each(function () {

        let vehicleID = parseInt($(this).find("select option:selected").val(), 10);
        let packageWeight = parseInt($(this).find(".pack-weight").text(), 10);

        for (let i = 0; i < vehiclesAndWeights.length; i++) {
            if (vehiclesAndWeights[i].id == vehicleID) {
                vehiclesAndWeights[i].packWeight += packageWeight;
            }
        }
    });
}

isWeightExceeded = function (vehiclesAndWeights) {

    let rval = false;

    vehiclesAndWeights.forEach(function (v) {

        if (v.packWeight > v.capacity) {
            rval = true;
        }
    });

    return rval;
}

printExceededWeightMessage = function (vehiclesAndWeights) {

    let exceededWeightMessage = "";

    vehiclesAndWeights.forEach(function (e) {
        if (e.capacity < e.packWeight) {

            let exceededWeight = e.packWeight - e.capacity;

            exceededWeightMessage += e.vname;
            exceededWeightMessage += ": vehicles capacity (";
            exceededWeightMessage += e.capacity;
            exceededWeightMessage += " kg) is exceeded by: ";
            exceededWeightMessage += exceededWeight;
            exceededWeightMessage += " kg.\n\n"
        }
    });
    
    alert(exceededWeightMessage);
}
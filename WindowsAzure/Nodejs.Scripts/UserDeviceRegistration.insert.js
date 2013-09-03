function insert(item, user, request) {

    var deviceRegistrationTable = tables.getTable('UserDeviceRegistration');
    deviceRegistrationTable.where({ Name: item.Name })
                           .read({ success: insertIfNewRegistration });

    function insertIfNewRegistration(matchedRegistrations) {
        // if we found any matches for this user we will delete them first
        // primarily solves re-registraton for Windows Phone devices.
        if (matchedRegistrations.length > 0) {
            matchedRegistrations.forEach(function (row) {
                deviceRegistrationTable.del(row.id)
                console.log('Before Insert, deleted device registration for ' + row.Name);
            });
        }

        // insert our new record            
        request.execute();

    }
}
function insert(item, user, request) {

    request.execute({
        success: function () {
            request.respond();

            var sqlQuery = "SELECT TOP 1 t.id, t.Name, d.ServiceType, d.ServiceKey FROM UserTask as t INNER JOIN UserDeviceRegistration as d ON t.Assignee = d.Name WHERE t.id = ?";
            mssql.query(sqlQuery, [item.id], {
                success: function (results) {
                    if (results.length > 0) {
                        console.log('Success', results)
                        //send notification
                        var serviceType = results[0].ServiceType;
                        var recipientIdentifier = results[0].ServiceKey;
                        var messageContent = results[0].Name;

                        switch (serviceType) {
                            case "GCM":
                                // Google Cloud Messaging
                                push.gcm.send(recipientIdentifier, messageContent,
                                {
                                    success: function (response) {
                                        console.log('GCM: Push sent: ', response)
                                    }, error: function (error) {
                                        console.log('GCM: Error on push: ', error)
                                    }
                                });
                                break;
                            case "APNS":
                                // Apple Push Notification Service
                                push.apns.send(recipientIdentifier, { alert: "New Task: " + messageContent }, {
                                    success: function (pushResponse) {
                                        console.log("APNS: Sent push: ", pushResponse);
                                    }, error: function (error) {
                                        console.log("APNS: Error on push: ", error);
                                    }
                                });
                                break;
                            case "MPNS":
                                // Microsoft Push Notification Sevice
                                push.mpns.sendToast(recipientIdentifier, { text1: messageContent },
                                {
                                    success: function (pushResponse) {
                                        console.log("MPNS: Sent push: ", pushResponse);
                                    }, error: function (error) {
                                        console.log("MPNS: Error on push: ", error);
                                    }
                                });
                                break;
                        }
                    } else {
                        console.log('Could not find registration for supplied user: ', item.assignee);
                    }

                }, error: function (error) {
                    console.log('Error sending notification: ', error);
                }
            });
        },
        error: function (error) {
            console.log('Error inserting record', error);
            request.respond(500, 'Unable to insert record');
        }
    });
}
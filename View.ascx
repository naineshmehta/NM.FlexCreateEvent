<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="NM.Modules.NM.FlexCreateEvent.View" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>

<link href="https://code.jquery.com/ui/1.11.3/themes/smoothness/jquery-ui.css" rel="stylesheet"/>

<form id="formCreate" name="formCreate" method="post">
    <fieldset>
        <legend>Location Details</legend>
    <div class="row" style="width: 100%; display: block; float: left;">
        <div id="pnlLocations" class="medium-12 columns">
            <label>Check if the location of your event is in our pre-populated list below<asp:DropDownList runat="server" ID="drpLocation"/></label>
        </div>
    </div>

    <div class="row" style="margin-bottom: 20px;">
    
        <a class="medium-12 columns" id="btnShowMap" onclick="DisplayMap();">Cannot find your location? Click here to search for a new location</a>
    
        <div style="width: 100%; display: block; float: left; display: none;" id="pnlMap">
            <div class="medium-12 columns">
                <input id="pac-input" type="text" placeholder="Search for your event location">
            </div>

            <div class="medium-6 columns">
                <div id="map" style="width: 100%; height: 400px; display: block; float: left"></div>
            </div>
            <div class="medium-6 columns"  style="display: block; float: left; padding-left: 20px;">
                Name: <input type="text" id="locationName"/>
                Location: <input type="text" id="locationTitle" name="locationTitle" readonly="true"/>
                Latitude: <input type="text" id="latitude" name="latitude" readonly="true"/>
                Longitude: <input type="text" id="longitude" name="longitude" readonly="true"/>
            </div>
        </div>
    </div>
    </fieldset>

    
    <fieldset>
        <legend>Event Details</legend>
        <div class="row">
            <div class="large-12 columns">
                <label>Event Name <input type="text" id="txtEventName" name="txtEventName"  placeholder="Enter your event name" runat="server"/></label>
                <asp:RequiredFieldValidator runat=server ControlToValidate="txtEventName" ErrorMessage="Event Name is required." Display="Dynamic" CssClass="ErrorMessage" ></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row">
            <div class="large-12 columns">
                <label>Short Description <textarea id="txtShortDesc" name="txtShortDesc" placeholder="Enter a summary of event" runat="server"></textarea></label>
                <asp:RequiredFieldValidator runat=server ControlToValidate="txtShortDesc" ErrorMessage="Short description is required." Display="Dynamic" CssClass="ErrorMessage" ></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row">
            <div class="large-12 columns" id="divFullDesc">
                <label>Full Description
                </label>
                <dnn:TextEditor ID="txtFullDesc" name="txtFullDesc" width="100%" runat="server"></dnn:TextEditor>
            </div>
        </div>
        <div class="row">
            <div class="large-6 columns">
                <label>Start Date <input type="text" id="startdatepicker" name="startdatepicker"  placeholder="Enter the start date of Event" ></label>
            </div>
            <div class="large-6 columns">
                <label>End Date <input type="text" id="enddatepicker" name="enddatepicker" placeholder="Enter the end date of Event" ></label>
            </div>
        </div>
        <div class="row">
            <div class="large-12 columns">
                <label>Upload Logo (logo should be at least 400px x 400px in order to display properly on the website)</label>
                <input type="file" id="fileUpload" name="fileUpload" accept="image/gif, image/jpeg, image/png" />
            </div>
        </div>
        <div class="row">
            <div class="large-12 columns">
                <input class="button" type="submit" id="btnUploadEvent" value="Upload your event"/>
            </div>
        </div>
    </fieldset>
</form>

<script type="text/javascript">
    $(function () {
        $("#startdatepicker, #enddatepicker").datepicker({
            changeMonth: true,
            changeYear: true
        });
    });

    $('#btnUploadEvent').click(function () {
        Page_ClientValidate();
        if (Page_IsValid) {
            alert('it is valid');
            return true;
        }
        else {
            alert('No valid');
            return false;
        }
    });


    function DisplayMap() {
        var panel = $('#pnlMap');
        var pnlLocations = $('#pnlLocations');
        if (panel.is(':visible')) {
            panel.hide();
            pnlLocations.show();
            $('#btnShowMap').text('Cannot find your location? Click here to search for a new location');

        } else {
            panel.show();
            pnlLocations.hide();
            initAutocomplete();
            $('#btnShowMap').text("Click here to view pre-selected locations");
        }
    }

    function initAutocomplete() {
        var map = new google.maps.Map(document.getElementById('map'),
        {
            center: { lat: 51.509865, lng: -0.118092 },
            zoom: 10,
            mapTypeId: 'roadmap'
        });

        // Create the search box and link it to the UI element.
        var input = document.getElementById('pac-input');
        var searchBox = new google.maps.places.SearchBox(input);
        //map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

        // Bias the SearchBox results towards current map's viewport.
        map.addListener('bounds_changed',
            function() {
                searchBox.setBounds(map.getBounds());
            });

        var markers = [];

        // Listen for the event fired when the user selects a prediction and retrieve
        // more details for that place.
        searchBox.addListener('places_changed',
            function() {
                var places = searchBox.getPlaces();
                var txtName = $('#locationName');
                var txtTitle = $('#locationTitle');
                var txtLat = $('#latitude');
                var txtLon = $('#longitude');

                if (places.length == 0) {
                    return;
                }

                if (places.length === 1) {
                    document.getElementById("locationName").value = places[0].name;
                    document.getElementById("locationTitle").value = places[0].formatted_address;
                    document.getElementById("latitude").value = places[0].geometry.location.lat();
                    document.getElementById("longitude").value = places[0].geometry.location.lng();
                }

                // Clear out the old markers.
                markers.forEach(function(marker) {
                    marker.setMap(null);
                });
                markers = [];

                // For each place, get the icon, name and location.
                var bounds = new google.maps.LatLngBounds();
                places.forEach(function(place) {
                    if (!place.geometry) {
                        console.log("Returned place contains no geometry");
                        return;
                    }

                    // Create a marker for each place.
                    var marker = new google.maps.Marker({
                        map: map,
                        title: place.name,
                        position: place.geometry.location,
                        draggable: true,
                        title: 'Drag me to move'
                    });

                    markers.push(marker);

                    google.maps.event.addListener(marker,
                        "click",
                        function(event) {
                            console.log(event);
                            console.log(event.latLng.lat());
                            document.getElementById("locationName").value = place.name;
                            document.getElementById("locationTitle").value = place.formatted_address;
                            document.getElementById("latitude").value = event.latLng.lat();
                            document.getElementById("longitude").value = event.latLng.lng();
                        });

                    google.maps.event.addListener(marker,
                        'dragend',
                        function(event) {
                            document.getElementById("latitude").value = event.latLng.lat();
                            document.getElementById("longitude").value = event.latLng.lng();
                        });

                    if (place.geometry.viewport) {
                        // Only geocodes have viewport.
                        bounds.union(place.geometry.viewport);
                    } else {
                        bounds.extend(place.geometry.location);
                    }
                });
                map.fitBounds(bounds);
            });
    }

</script>

<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAQkYL6wzVaHIjoGR8DfuRbTZxI6YEDc1A&callback=initAutocomplete&libraries=places"></script>
<script src="https://cdn.jsdelivr.net/jquery.validation/1.16.0/jquery.validate.min.js"></script>

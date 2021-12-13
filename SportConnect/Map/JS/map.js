var myMap;
var wv = window.chrome.webview;
var tempMarker;

/*Trevor Abel
 *Gets a new map marker
 * Unnecessary in its current state, but allows
 * for general manipulation of the new marker
 * in every case.
 */
function getMarker(latlng) {
	var mark = L.marker(latlng);
	return mark;
}
/*
 * Trevor Abel
 * Map onclick
 * Adds a temporary marker to the map and sends
 * the program a CreateEvent message.
 */
function onMapClick(e) {
	tempMarker = getMarker(e.latlng);
	tempMarker.addTo(myMap);
	wv.postMessage(`CreateEvent@${e.latlng}`);
}
/*
 *Trevor Abel 
 * If event creation is successful, the temporary marker
 * becomes a permanent one, with a popup filled with 
 * temporary information.
 * If event creation is unsuccessful, the temporary marker
 * is removed.
 */
function createEvent(response) {
	if (response.length !== 0) {
		response = JSON.parse(response);
		var popupMessage = '<p>' + response.Name + '</p>' +
			'<p>' + response.Sport + '</p>' +
			'<p>' + response.Start + '  ' + response.End + '</p>' +
			'<p>' + response.MaxPlayers + '</p>' +
			'<p>' + response.SkillLevel + '</p>' +
			'<p>' + response.Location + '</p>';
		tempMarker.bindPopup(popupMessage);
	}
	else {
		tempMarker.removeFrom(myMap);
	}
}
/*
 * Trevor Abel
 * Creates a map and adds its onclick event
 */
function setup() {
	myMap = L.map('mapid').setView([44.0262, -88.5517], 15);
	L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
		attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
		maxZoom: 18,
		id: 'mapbox/streets-v11',
		accessToken: 'pk.eyJ1IjoiYWJlbHQiLCJhIjoiY2t2aDVodHpzMmZzZjMxczFjbGgxZXM3dSJ9.kSVKQeL9NP2AICuS_dUAzA'
	}).addTo(myMap);

	myMap.on('click', onMapClick);
}
/*
 * Trevor Abel
 * Sends a message to the program to attend an event
 */
function attendEvent(e) {
	wv.postMessage(`AttendEvent@${e}`);
}

//Run Setup on pageload
setup();
//Listen for messages from program
wv.addEventListener('message', event => createEvent(event.data));







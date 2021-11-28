//window.chrome.webview.postMessage()
//window.chrome.webview.addEventListener(\'message\', event => alert(event.data));"
var myMap;
var wv = window.chrome.webview;
var tempMarker;

function getMarker(latlng) {
	var mark = L.marker(latlng);
	//mark.on('click', function () { window.external.ViewEvent() });
	return mark;
}

function onMapClick(e) {
	tempMarker = getMarker(e.latlng);
	tempMarker.addTo(myMap);
	wv.postMessage(`CreateEvent,${e.latlng}`);
}

function CreateEvent(response) {
	//var eventInfo = window.external.CreateEvent(e.latlng);
	if (response.length === 0) {
		response = JSON.parse(response);
		var popupMessage = '<p>' + eventInfo.Name + '</p>' +
			'<p>' + response.Sport + '</p>' +
			'<p>' + response.Start + '  ' + response.End + '</p>' +
			'<p>' + response.MaxPlayers + '</p>' +
			'<p>' + response.SkillLevel + '</p>' +
			'<p>' + response.Location + '</p>';
		tempMarker.bindPopup(popupMessage);
		//tempMarker.bindPopup(eventInfo);
	}
	else {
		tempMarker.removeFrom(myMap);
	}
}

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


setup();

wv.addEventListener("message", event => CreateEvent(event.data));






